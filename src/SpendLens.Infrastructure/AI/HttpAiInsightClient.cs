using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using SpendLens.Application.AI;
using SpendLens.Domain.Analysis;

namespace SpendLens.Infrastructure.AI;

public sealed class HttpAiInsightClient(HttpClient httpClient) : IAiInsightClient
{
    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web);

    public async Task<AiTransactionEnrichment> EnrichTransactionAsync(string description, decimal amount, string currency, CancellationToken cancellationToken)
    {
        var requestContent = BuildJsonContent(new
        {
            description,
            amount,
            currency
        });

        var response = await httpClient.PostAsync(
            "/api/enrich",
            requestContent,
            cancellationToken);
        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<EnrichResponse>(cancellationToken: cancellationToken)
            ?? throw new InvalidOperationException("AI enrich response was empty.");

        return new AiTransactionEnrichment(
            payload.MerchantName,
            payload.NormalizedDescription,
            payload.Category,
            payload.IsSubscriptionCandidate,
            payload.Confidence);
    }

    public async Task<MonthlyReport> GenerateMonthlyReportAsync(MonthlyReportNarrativeRequest request, CancellationToken cancellationToken)
    {
        var payload = new
        {
            summary = new
            {
                year = request.Summary.Year,
                month = request.Summary.Month,
                totalExpense = request.Summary.TotalExpense,
                totalIncome = request.Summary.TotalIncome,
                currency = request.Summary.Currency,
                transactionCount = request.Summary.TransactionCount,
                topCategory = request.Summary.TopCategory,
                subscriptionTotal = request.Summary.SubscriptionTotal,
                subscriptionCount = request.Summary.SubscriptionCount
            },
            topCategories = request.TopCategories.Select(item => new
            {
                category = item.Category,
                amount = item.Amount,
                transactionCount = item.TransactionCount,
                percentage = item.Percentage
            }),
            subscriptions = request.Subscriptions.Select(item => new
            {
                merchantName = item.MerchantName,
                estimatedMonthlyAmount = item.EstimatedMonthlyAmount,
                currency = item.Currency,
                paymentCount = item.PaymentCount,
                firstPaymentDate = item.FirstPaymentDate.ToString("yyyy-MM-dd"),
                lastPaymentDate = item.LastPaymentDate.ToString("yyyy-MM-dd"),
                confidence = item.Confidence
            })
        };

        var response = await httpClient.PostAsync(
            "/api/report",
            BuildJsonContent(payload),
            cancellationToken);
        response.EnsureSuccessStatusCode();

        var reportResponse = await response.Content.ReadFromJsonAsync<ReportResponse>(cancellationToken: cancellationToken)
            ?? throw new InvalidOperationException("AI report response was empty.");

        return new MonthlyReport(reportResponse.Title, reportResponse.Summary, reportResponse.Highlights);
    }

    private sealed record EnrichResponse(
        [property: JsonPropertyName("merchantName")] string MerchantName,
        [property: JsonPropertyName("normalizedDescription")] string NormalizedDescription,
        [property: JsonPropertyName("category")] string Category,
        [property: JsonPropertyName("isSubscriptionCandidate")] bool IsSubscriptionCandidate,
        [property: JsonPropertyName("confidence")] decimal Confidence);

    private sealed record ReportResponse(
        [property: JsonPropertyName("title")] string Title,
        [property: JsonPropertyName("summary")] string Summary,
        [property: JsonPropertyName("highlights")] IReadOnlyList<string> Highlights);

    private static StringContent BuildJsonContent<T>(T value)
    {
        return new StringContent(
            JsonSerializer.Serialize(value, SerializerOptions),
            Encoding.UTF8,
            "application/json");
    }
}
