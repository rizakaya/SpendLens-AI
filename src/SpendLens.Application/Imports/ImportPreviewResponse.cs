namespace SpendLens.Application.Imports;

public sealed record ImportPreviewResponse(
    string FileName,
    string Delimiter,
    int TotalRows,
    IReadOnlyList<string> Headers,
    IReadOnlyList<ImportPreviewRowResponse> PreviewRows,
    IReadOnlyList<string> Warnings);
