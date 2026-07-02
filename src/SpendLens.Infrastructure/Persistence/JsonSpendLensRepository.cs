using System.Text.Json;
using SpendLens.Application.Abstractions;
using SpendLens.Domain.Imports;
using SpendLens.Domain.Transactions;

namespace SpendLens.Infrastructure.Persistence;

public sealed class JsonSpendLensRepository : ISpendLensRepository
{
    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web)
    {
        WriteIndented = true
    };

    private readonly string _filePath;
    private readonly object _syncRoot = new();

    public JsonSpendLensRepository(string dataDirectory)
    {
        Directory.CreateDirectory(dataDirectory);
        _filePath = Path.Combine(dataDirectory, "spendlens-data.json");
    }

    public IReadOnlyList<ImportBatch> GetImports()
    {
        return Load().Imports.ToArray();
    }

    public IReadOnlyList<SpendTransaction> GetTransactions()
    {
        return Load().Transactions.ToArray();
    }

    public void SaveImport(ImportBatch importBatch, IReadOnlyList<SpendTransaction> transactions)
    {
        lock (_syncRoot)
        {
            var store = LoadUnsafe();
            store.Imports.Add(importBatch);
            store.Transactions.AddRange(transactions);
            SaveUnsafe(store);
        }
    }

    private SpendLensDataStore Load()
    {
        lock (_syncRoot)
        {
            return LoadUnsafe();
        }
    }

    private SpendLensDataStore LoadUnsafe()
    {
        if (!File.Exists(_filePath))
        {
            return new SpendLensDataStore();
        }

        var json = File.ReadAllText(_filePath);
        return JsonSerializer.Deserialize<SpendLensDataStore>(json, SerializerOptions) ?? new SpendLensDataStore();
    }

    private void SaveUnsafe(SpendLensDataStore store)
    {
        var json = JsonSerializer.Serialize(store, SerializerOptions);
        File.WriteAllText(_filePath, json);
    }
}
