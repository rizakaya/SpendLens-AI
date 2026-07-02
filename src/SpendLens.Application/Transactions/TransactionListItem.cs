using SpendLens.Domain.Transactions;

namespace SpendLens.Application.Transactions;

public sealed record TransactionListItem(
    Guid Id,
    string Date,
    string RawDescription,
    string MerchantName,
    string Category,
    decimal Amount,
    string Currency,
    TransactionDirection Direction,
    bool IsSubscriptionCandidate,
    decimal Confidence);
