using SpendLens.Domain.Imports;
using SpendLens.Domain.Transactions;

namespace SpendLens.Infrastructure.Persistence;

internal sealed class SpendLensDataStore
{
    public List<ImportBatch> Imports { get; init; } = [];

    public List<SpendTransaction> Transactions { get; init; } = [];
}
