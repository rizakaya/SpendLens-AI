namespace SpendLens.Application.Transactions;

public sealed record TransactionListResponse(
    int TotalCount,
    IReadOnlyList<TransactionListItem> Items);
