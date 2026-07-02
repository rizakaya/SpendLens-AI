namespace SpendLens.Domain.Imports;

public sealed record ImportBatch(
    Guid Id,
    string FileName,
    DateTimeOffset ImportedAtUtc,
    int TransactionCount);
