using SpendLens.Application.Abstractions;
using SpendLens.Application.AI;
using SpendLens.Domain.Imports;
using SpendLens.Domain.Transactions;

namespace SpendLens.Application.Imports;

public sealed class TransactionImportService(
    CsvImportParser parser,
    IAiInsightClient aiInsightClient,
    ISpendLensRepository repository) : ITransactionImportService
{
    public async Task<ImportTransactionsResult> ImportAsync(ImportTransactionsRequest request, CancellationToken cancellationToken)
    {
        var parsedFile = parser.Parse(request.Content, request.FileName);
        var importBatch = new ImportBatch(Guid.NewGuid(), parsedFile.FileName, DateTimeOffset.UtcNow, parsedFile.Rows.Count);
        var transactions = new List<SpendTransaction>();

        foreach (var row in parsedFile.Rows)
        {
            var direction = row.Amount < 0 ? TransactionDirection.Expense : TransactionDirection.Income;
            var enrichment = await aiInsightClient.EnrichTransactionAsync(row.Description, row.Amount, row.Currency, cancellationToken);

            transactions.Add(new SpendTransaction(
                Guid.NewGuid(),
                importBatch.Id,
                row.TransactionDate,
                row.Description,
                enrichment.MerchantName,
                enrichment.NormalizedDescription,
                enrichment.Category,
                Math.Abs(row.Amount),
                row.Currency,
                direction,
                enrichment.IsSubscriptionCandidate && direction == TransactionDirection.Expense,
                enrichment.Confidence,
                DateTimeOffset.UtcNow));
        }

        repository.SaveImport(importBatch, transactions);
        return new ImportTransactionsResult(importBatch, transactions, parsedFile.Warnings);
    }
}
