namespace SpendLens.Application.Imports;

public sealed record ImportPreviewRequest(
    string Content,
    string? FileName = null);
