using SpendLens.Application.Abstractions;
using SpendLens.Application.AI;
using SpendLens.Application.Transactions;
using SpendLens.Domain.Analysis;
using SpendLens.Domain.Imports;
using SpendLens.Domain.Subscriptions;
using SpendLens.Domain.Transactions;

namespace SpendLens.Application.Analysis;

public sealed class SpendAnalysisService(
    ISpendLensRepository repository,
    IAiInsightClient aiInsightClient) : ISpendAnalysisService
{
    public IReadOnlyList<ImportBatch> GetImports()
    {
        return repository.GetImports()
            .OrderByDescending(item => item.ImportedAtUtc)
            .ToArray();
    }

    public TransactionListResponse GetTransactions(int? year, int? month, string? category)
    {
        var filtered = ApplyPeriodFilter(repository.GetTransactions(), year, month);

        if (!string.IsNullOrWhiteSpace(category))
        {
            filtered = filtered
                .Where(item => string.Equals(item.Category, category, StringComparison.OrdinalIgnoreCase))
                .ToArray();
        }

        var items = filtered
            .OrderByDescending(item => item.TransactionDate)
            .ThenByDescending(item => item.Amount)
            .Select(item => new TransactionListItem(
                item.Id,
                item.TransactionDate.ToString("yyyy-MM-dd"),
                item.RawDescription,
                item.MerchantName,
                item.Category,
                item.Amount,
                item.Currency,
                item.Direction,
                item.IsSubscriptionCandidate,
                item.Confidence))
            .ToArray();

        return new TransactionListResponse(items.Length, items);
    }

    public MonthlySummary GetMonthlySummary(int year, int month)
    {
        var transactions = ApplyPeriodFilter(repository.GetTransactions(), year, month);
        var expenses = transactions.Where(item => item.Direction == TransactionDirection.Expense).ToArray();
        var incomes = transactions.Where(item => item.Direction == TransactionDirection.Income).ToArray();
        var currency = transactions.FirstOrDefault()?.Currency ?? "TRY";
        var topCategory = expenses
            .GroupBy(item => item.Category)
            .OrderByDescending(group => group.Sum(item => item.Amount))
            .Select(group => group.Key)
            .FirstOrDefault() ?? "N/A";
        var subscriptions = BuildSubscriptions(expenses);

        return new MonthlySummary(
            year,
            month,
            expenses.Sum(item => item.Amount),
            incomes.Sum(item => item.Amount),
            currency,
            transactions.Count,
            topCategory,
            subscriptions.Sum(item => item.EstimatedMonthlyAmount),
            subscriptions.Count);
    }

    public IReadOnlyList<CategoryBreakdownItem> GetCategoryBreakdown(int year, int month)
    {
        var expenses = ApplyPeriodFilter(repository.GetTransactions(), year, month)
            .Where(item => item.Direction == TransactionDirection.Expense)
            .ToArray();

        var total = expenses.Sum(item => item.Amount);
        if (total <= 0)
        {
            return [];
        }

        return expenses
            .GroupBy(item => item.Category)
            .Select(group => new CategoryBreakdownItem(
                group.Key,
                group.Sum(item => item.Amount),
                group.Count(),
                decimal.Round(group.Sum(item => item.Amount) / total * 100m, 2)))
            .OrderByDescending(item => item.Amount)
            .ToArray();
    }

    public IReadOnlyList<TopMerchantItem> GetTopMerchants(int year, int month, int take)
    {
        return ApplyPeriodFilter(repository.GetTransactions(), year, month)
            .Where(item => item.Direction == TransactionDirection.Expense)
            .GroupBy(item => item.MerchantName)
            .Select(group => new TopMerchantItem(
                group.Key,
                group.Sum(item => item.Amount),
                group.Count()))
            .OrderByDescending(item => item.Amount)
            .Take(Math.Max(1, take))
            .ToArray();
    }

    public IReadOnlyList<SubscriptionCandidate> GetSubscriptions()
    {
        var expenses = repository.GetTransactions()
            .Where(item => item.Direction == TransactionDirection.Expense)
            .ToArray();

        return BuildSubscriptions(expenses)
            .OrderByDescending(item => item.Confidence)
            .ThenByDescending(item => item.EstimatedMonthlyAmount)
            .ToArray();
    }

    public async Task<MonthlyReport> BuildMonthlyReportAsync(int year, int month, CancellationToken cancellationToken)
    {
        var summary = GetMonthlySummary(year, month);
        var topCategories = GetCategoryBreakdown(year, month).Take(5).ToArray();
        var subscriptions = GetSubscriptions().Take(5).ToArray();

        return await aiInsightClient.GenerateMonthlyReportAsync(
            new MonthlyReportNarrativeRequest(summary, topCategories, subscriptions),
            cancellationToken);
    }

    private static IReadOnlyList<SpendTransaction> ApplyPeriodFilter(IEnumerable<SpendTransaction> source, int? year, int? month)
    {
        var filtered = source;

        if (year is not null)
        {
            filtered = filtered.Where(item => item.TransactionDate.Year == year.Value);
        }

        if (month is not null)
        {
            filtered = filtered.Where(item => item.TransactionDate.Month == month.Value);
        }

        return filtered.ToArray();
    }

    private static IReadOnlyList<SubscriptionCandidate> BuildSubscriptions(IEnumerable<SpendTransaction> expenses)
    {
        return expenses
            .Where(item => item.IsSubscriptionCandidate)
            .GroupBy(item => item.MerchantName)
            .Where(group => group.Count() >= 2)
            .Select(group =>
            {
                var ordered = group.OrderBy(item => item.TransactionDate).ToArray();
                var averageAmount = decimal.Round(group.Average(item => item.Amount), 2);
                var confidence = decimal.Round(group.Average(item => item.Confidence), 2);

                return new SubscriptionCandidate(
                    group.Key,
                    averageAmount,
                    group.First().Currency,
                    group.Count(),
                    ordered.First().TransactionDate,
                    ordered.Last().TransactionDate,
                    confidence);
            })
            .ToArray();
    }
}
