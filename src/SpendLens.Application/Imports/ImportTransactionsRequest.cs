namespace SpendLens.Application.Imports;

public sealed record ImportTransactionsRequest(
    string Content,
    string FileName);
