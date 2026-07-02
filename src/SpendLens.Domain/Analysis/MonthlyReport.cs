namespace SpendLens.Domain.Analysis;

public sealed record MonthlyReport(
    string Title,
    string Summary,
    IReadOnlyList<string> Highlights);
