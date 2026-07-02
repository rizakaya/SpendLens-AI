namespace SpendLens.Api.Contracts.Imports;

public sealed record ImportPreviewRequestContract(
    string Content,
    string? FileName = null);
