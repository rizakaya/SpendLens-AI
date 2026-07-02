using SpendLens.Domain.Imports;
using SpendLens.Domain.Transactions;

namespace SpendLens.Application.Imports;

public sealed record ImportTransactionsResult(
    ImportBatch ImportBatch,
    IReadOnlyList<SpendTransaction> Transactions,
    IReadOnlyList<string> Warnings);
