namespace SpendLens.Domain.Analysis;

public sealed record TopMerchantItem(
    string MerchantName,
    decimal Amount,
    int TransactionCount);
