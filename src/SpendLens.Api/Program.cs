using SpendLens.Application.Abstractions;
using SpendLens.Application.AI;
using SpendLens.Application.Analysis;
using SpendLens.Application.Imports;
using SpendLens.Infrastructure.AI;
using SpendLens.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);
var dataDirectory = builder.Configuration["Storage:DataDirectory"]
    ?? Path.Combine(builder.Environment.ContentRootPath, "App_Data");
var aiBaseUrl = builder.Configuration["AI:BaseUrl"] ?? "http://localhost:8081";

builder.Services.AddControllers();
builder.Services.AddSingleton(new CsvImportParser());
builder.Services.AddSingleton<IImportPreviewService, CsvImportPreviewService>();
builder.Services.AddSingleton<ISpendLensRepository>(_ => new JsonSpendLensRepository(dataDirectory));
builder.Services.AddSingleton<ITransactionImportService, TransactionImportService>();
builder.Services.AddSingleton<ISpendAnalysisService, SpendAnalysisService>();
builder.Services.AddHttpClient<IAiInsightClient, HttpAiInsightClient>(client =>
{
    client.BaseAddress = new Uri(aiBaseUrl);
});

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseAuthorization();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
