namespace SpendLens.Api.Contracts.Imports;

public sealed record ImportTransactionsRequestContract(
    string Content,
    string FileName);
