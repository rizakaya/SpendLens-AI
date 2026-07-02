namespace SpendLens.Api.Contracts.Imports;

public sealed record ImportTransactionsResponseContract(
    Guid ImportId,
    string FileName,
    int ImportedTransactions,
    IReadOnlyList<string> Warnings);
