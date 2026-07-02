namespace SpendLens.Domain.Analysis;

public sealed record MonthlySummary(
    int Year,
    int Month,
    decimal TotalExpense,
    decimal TotalIncome,
    string Currency,
    int TransactionCount,
    string TopCategory,
    decimal SubscriptionTotal,
    int SubscriptionCount);
