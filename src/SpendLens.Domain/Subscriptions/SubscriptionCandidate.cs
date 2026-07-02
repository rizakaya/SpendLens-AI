namespace SpendLens.Domain.Subscriptions;

public sealed record SubscriptionCandidate(
    string MerchantName,
    decimal EstimatedMonthlyAmount,
    string Currency,
    int PaymentCount,
    DateOnly FirstPaymentDate,
    DateOnly LastPaymentDate,
    decimal Confidence);
