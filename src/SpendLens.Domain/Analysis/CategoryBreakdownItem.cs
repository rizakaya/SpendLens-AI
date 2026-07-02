namespace SpendLens.Domain.Analysis;

public sealed record CategoryBreakdownItem(
    string Category,
    decimal Amount,
    int TransactionCount,
    decimal Percentage);
