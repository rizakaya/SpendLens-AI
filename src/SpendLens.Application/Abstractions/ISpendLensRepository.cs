using SpendLens.Domain.Imports;
using SpendLens.Domain.Transactions;

namespace SpendLens.Application.Abstractions;

public interface ISpendLensRepository
{
    IReadOnlyList<ImportBatch> GetImports();

    IReadOnlyList<SpendTransaction> GetTransactions();

    void SaveImport(ImportBatch importBatch, IReadOnlyList<SpendTransaction> transactions);
}
