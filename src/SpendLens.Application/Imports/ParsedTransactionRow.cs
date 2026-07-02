namespace SpendLens.Application.Imports;

public sealed record ParsedTransactionRow(
    int SourceLineNumber,
    DateOnly TransactionDate,
    string Description,
    decimal Amount,
    string Currency);
