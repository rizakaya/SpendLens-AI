namespace SpendLens.Application.Imports;

public interface ITransactionImportService
{
    Task<ImportTransactionsResult> ImportAsync(ImportTransactionsRequest request, CancellationToken cancellationToken);
}
