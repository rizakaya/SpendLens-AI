namespace SpendLens.Domain.Imports;

public sealed record ImportedTransactionPreview(
    DateOnly? TransactionDate,
    string Description,
    decimal Amount,
    string Currency,
    int SourceLineNumber);
