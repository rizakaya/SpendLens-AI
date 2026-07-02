namespace SpendLens.Application.Imports;

public sealed record ImportPreviewRowResponse(
    int SourceLineNumber,
    string? TransactionDate,
    string Description,
    decimal Amount,
    string Currency);
