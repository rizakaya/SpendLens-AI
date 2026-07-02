using SpendLens.Application.Abstractions;
using SpendLens.Application.AI;
using SpendLens.Application.Analysis;
using SpendLens.Application.Imports;
using SpendLens.Domain.Analysis;
using SpendLens.Domain.Imports;
using SpendLens.Domain.Transactions;

namespace SpendLens.UnitTests;

public class ImportAndAnalysisTests
{
    [Fact]
    public void BuildPreview_ParsesCommaSeparatedRows()
    {
        var service = new CsvImportPreviewService(new CsvImportParser());
        var request = new ImportPreviewRequest(
            """
            Date,Description,Amount,Currency
            2026-07-01,MARKET,125.45,TRY
            2026-07-02,SPOTIFY,59.99,TRY
            """,
            "sample.csv");

        var response = service.BuildPreview(request);

        Assert.Equal("sample.csv", response.FileName);
        Assert.Equal("comma", response.Delimiter);
        Assert.Equal(2, response.TotalRows);
        Assert.Equal(125.45m, response.PreviewRows[0].Amount);
        Assert.Equal("SPOTIFY", response.PreviewRows[1].Description);
        Assert.Empty(response.Warnings);
    }

    [Fact]
    public async Task ImportAsync_SavesTransactions_WithAiEnrichment()
    {
        var repository = new InMemoryRepository();
        var service = new TransactionImportService(new CsvImportParser(), new FakeAiClient(), repository);

        var result = await service.ImportAsync(
            new ImportTransactionsRequest(
                """
                Date,Description,Amount,Currency
                2026-07-01,SPOTIFY PREMIUM,-59.99,TRY
                2026-07-02,MAAS ODEMESI,75000,TRY
                """,
                "sample.csv"),
            CancellationToken.None);

        Assert.Equal(2, result.Transactions.Count);
        Assert.Equal(2, repository.GetTransactions().Count);
        Assert.Equal("Subscription", result.Transactions[0].Category);
        Assert.Equal(TransactionDirection.Expense, result.Transactions[0].Direction);
        Assert.Equal(TransactionDirection.Income, result.Transactions[1].Direction);
    }

    [Fact]
    public async Task GetMonthlySummary_AggregatesImportedTransactions()
    {
        var repository = new InMemoryRepository();
        var importer = new TransactionImportService(new CsvImportParser(), new FakeAiClient(), repository);

        await importer.ImportAsync(
            new ImportTransactionsRequest(
                """
                Date,Description,Amount,Currency
                2026-07-01,SPOTIFY PREMIUM,-59.99,TRY
                2026-07-03,MIGROS SANAL MARKET,-800.00,TRY
                2026-07-05,MAAS ODEMESI,50000,TRY
                2026-06-01,SPOTIFY PREMIUM,-59.99,TRY
                """,
                "sample.csv"),
            CancellationToken.None);

        var service = new SpendAnalysisService(repository, new FakeAiClient());
        var summary = service.GetMonthlySummary(2026, 7);
        var categories = service.GetCategoryBreakdown(2026, 7);

        Assert.Equal(859.99m, summary.TotalExpense);
        Assert.Equal(50000m, summary.TotalIncome);
        Assert.Equal("Groceries", summary.TopCategory);
        Assert.Equal(2, categories.Count);
    }

    private sealed class InMemoryRepository : ISpendLensRepository
    {
        private readonly List<ImportBatch> _imports = [];
        private readonly List<SpendTransaction> _transactions = [];

        public IReadOnlyList<ImportBatch> GetImports() => _imports;

        public IReadOnlyList<SpendTransaction> GetTransactions() => _transactions;

        public void SaveImport(ImportBatch importBatch, IReadOnlyList<SpendTransaction> transactions)
        {
            _imports.Add(importBatch);
            _transactions.AddRange(transactions);
        }
    }

    private sealed class FakeAiClient : IAiInsightClient
    {
        public Task<AiTransactionEnrichment> EnrichTransactionAsync(string description, decimal amount, string currency, CancellationToken cancellationToken)
        {
            var normalized = description.ToUpperInvariant();
            var category = normalized.Contains("SPOTIFY") ? "Subscription" :
                normalized.Contains("MIGROS") ? "Groceries" :
                normalized.Contains("MAAS") ? "Income" : "Other";

            return Task.FromResult(new AiTransactionEnrichment(
                normalized.Contains("MIGROS") ? "Migros" : description,
                $"{description} normalized",
                category,
                normalized.Contains("SPOTIFY"),
                0.9m));
        }

        public Task<MonthlyReport> GenerateMonthlyReportAsync(MonthlyReportNarrativeRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new MonthlyReport("title", "summary", ["point"]));
        }
    }
}
