namespace SpendLens.Domain.Transactions;

public sealed record SpendTransaction(
    Guid Id,
    Guid ImportBatchId,
    DateOnly TransactionDate,
    string RawDescription,
    string MerchantName,
    string NormalizedDescription,
    string Category,
    decimal Amount,
    string Currency,
    TransactionDirection Direction,
    bool IsSubscriptionCandidate,
    decimal Confidence,
    DateTimeOffset CreatedAtUtc);
