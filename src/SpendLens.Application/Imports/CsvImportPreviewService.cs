namespace SpendLens.Application.Imports;

public sealed class CsvImportPreviewService(CsvImportParser parser) : IImportPreviewService
{
    public ImportPreviewResponse BuildPreview(ImportPreviewRequest request)
    {
        var parsed = parser.Parse(request.Content, request.FileName);

        return new ImportPreviewResponse(
            parsed.FileName,
            parsed.Delimiter,
            parsed.Rows.Count,
            parsed.Headers,
            parsed.Rows
                .Take(10)
                .Select(row => new ImportPreviewRowResponse(
                    row.SourceLineNumber,
                    row.TransactionDate.ToString("yyyy-MM-dd"),
                    row.Description,
                    row.Amount,
                    row.Currency))
                .ToArray(),
            parsed.Warnings);
    }
}
