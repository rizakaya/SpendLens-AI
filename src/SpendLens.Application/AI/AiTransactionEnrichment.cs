namespace SpendLens.Application.AI;

public sealed record AiTransactionEnrichment(
    string MerchantName,
    string NormalizedDescription,
    string Category,
    bool IsSubscriptionCandidate,
    decimal Confidence);
