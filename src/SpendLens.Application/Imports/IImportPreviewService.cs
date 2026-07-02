namespace SpendLens.Application.Imports;

public interface IImportPreviewService
{
    ImportPreviewResponse BuildPreview(ImportPreviewRequest request);
}
