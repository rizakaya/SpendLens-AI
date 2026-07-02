using SpendLens.Domain.Analysis;
using SpendLens.Domain.Subscriptions;

namespace SpendLens.Application.AI;

public sealed record MonthlyReportNarrativeRequest(
    MonthlySummary Summary,
    IReadOnlyList<CategoryBreakdownItem> TopCategories,
    IReadOnlyList<SubscriptionCandidate> Subscriptions);
