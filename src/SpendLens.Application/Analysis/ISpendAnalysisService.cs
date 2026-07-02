using SpendLens.Application.Transactions;
using SpendLens.Domain.Analysis;
using SpendLens.Domain.Imports;
using SpendLens.Domain.Subscriptions;

namespace SpendLens.Application.Analysis;

public interface ISpendAnalysisService
{
    IReadOnlyList<ImportBatch> GetImports();

    TransactionListResponse GetTransactions(int? year, int? month, string? category);

    MonthlySummary GetMonthlySummary(int year, int month);

    IReadOnlyList<CategoryBreakdownItem> GetCategoryBreakdown(int year, int month);

    IReadOnlyList<TopMerchantItem> GetTopMerchants(int year, int month, int take);

    IReadOnlyList<SubscriptionCandidate> GetSubscriptions();

    Task<MonthlyReport> BuildMonthlyReportAsync(int year, int month, CancellationToken cancellationToken);
}
