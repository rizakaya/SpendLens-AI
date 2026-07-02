using SpendLens.Domain.Analysis;

namespace SpendLens.Application.AI;

public interface IAiInsightClient
{
    Task<AiTransactionEnrichment> EnrichTransactionAsync(string description, decimal amount, string currency, CancellationToken cancellationToken);

    Task<MonthlyReport> GenerateMonthlyReportAsync(MonthlyReportNarrativeRequest request, CancellationToken cancellationToken);
}
