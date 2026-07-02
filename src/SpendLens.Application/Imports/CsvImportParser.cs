using System.Globalization;
using SpendLens.Domain.Imports;

namespace SpendLens.Application.Imports;

public sealed class CsvImportParser
{
    private static readonly char[] CandidateDelimiters = [',', ';', '\t'];
    private static readonly string[] DateFormats =
    [
        "yyyy-MM-dd",
        "dd.MM.yyyy",
        "d.M.yyyy",
        "dd/MM/yyyy",
        "d/M/yyyy",
        "MM/dd/yyyy",
        "M/d/yyyy"
    ];

    public ParsedImportFile Parse(string content, string? fileName)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            throw new ArgumentException("Import content is required.", nameof(content));
        }

        var lines = content
            .Split(["\r\n", "\n"], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        if (lines.Length < 2)
        {
            throw new ArgumentException("At least a header row and one data row are required.", nameof(content));
        }

        var delimiter = DetectDelimiter(lines[0]);
        var headers = ParseLine(lines[0], delimiter)
            .Select(NormalizeCell)
            .ToArray();

        var headerMap = BuildHeaderMap(headers);
        var warnings = new List<string>();

        if (headerMap.DateIndex is null)
        {
            warnings.Add("No date column detected. Today's date was used for missing values.");
        }

        if (headerMap.DescriptionIndex is null)
        {
            warnings.Add("No description column detected. Parsed rows may be incomplete.");
        }

        if (headerMap.AmountIndex is null)
        {
            throw new ArgumentException("No amount column could be detected.", nameof(content));
        }

        var parsedRows = new List<ParsedTransactionRow>();

        for (var index = 1; index < lines.Length; index++)
        {
            var cells = ParseLine(lines[index], delimiter).Select(NormalizeCell).ToArray();
            if (cells.All(string.IsNullOrWhiteSpace))
            {
                continue;
            }

            var parsed = ParseRow(cells, headerMap, index + 1, warnings);
            parsedRows.Add(new ParsedTransactionRow(
                parsed.SourceLineNumber,
                parsed.TransactionDate ?? DateOnly.FromDateTime(DateTime.UtcNow),
                parsed.Description,
                parsed.Amount,
                parsed.Currency));
        }

        return new ParsedImportFile(
            fileName ?? "inline-upload.csv",
            DescribeDelimiter(delimiter),
            headers,
            parsedRows,
            warnings.Distinct(StringComparer.OrdinalIgnoreCase).ToArray());
    }

    private static ImportedTransactionPreview ParseRow(
        IReadOnlyList<string> cells,
        HeaderMap headerMap,
        int lineNumber,
        ICollection<string> warnings)
    {
        var description = GetCell(cells, headerMap.DescriptionIndex) ?? string.Empty;
        var currency = GetCell(cells, headerMap.CurrencyIndex) ?? "TRY";
        var amountText = GetCell(cells, headerMap.AmountIndex) ?? "0";

        if (!TryParseAmount(amountText, out var amount))
        {
            warnings.Add($"Line {lineNumber}: amount '{amountText}' could not be parsed and was set to 0.");
            amount = 0;
        }

        DateOnly? transactionDate = null;
        var dateText = GetCell(cells, headerMap.DateIndex);
        if (!string.IsNullOrWhiteSpace(dateText) && !TryParseDate(dateText, out transactionDate))
        {
            warnings.Add($"Line {lineNumber}: date '{dateText}' could not be parsed.");
        }

        return new ImportedTransactionPreview(transactionDate, description, amount, currency, lineNumber);
    }

    private static HeaderMap BuildHeaderMap(IReadOnlyList<string> headers)
    {
        int? FindIndex(params string[] aliases)
        {
            for (var index = 0; index < headers.Count; index++)
            {
                var normalized = NormalizeHeader(headers[index]);
                if (aliases.Contains(normalized, StringComparer.OrdinalIgnoreCase))
                {
                    return index;
                }
            }

            return null;
        }

        return new HeaderMap(
            FindIndex("date", "transactiondate", "tarih"),
            FindIndex("description", "aciklama", "merchant", "islemaciklamasi"),
            FindIndex("amount", "tutar", "price", "harcama"),
            FindIndex("currency", "curr", "para", "parabirimi"));
    }

    private static bool TryParseDate(string value, out DateOnly? result)
    {
        if (DateOnly.TryParseExact(value, DateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out var exact))
        {
            result = exact;
            return true;
        }

        if (DateOnly.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsed))
        {
            result = parsed;
            return true;
        }

        result = null;
        return false;
    }

    private static bool TryParseAmount(string value, out decimal amount)
    {
        var normalized = value
            .Replace(" ", string.Empty, StringComparison.Ordinal)
            .Replace("TRY", string.Empty, StringComparison.OrdinalIgnoreCase)
            .Replace("TL", string.Empty, StringComparison.OrdinalIgnoreCase);

        if (normalized.Contains(',') && normalized.Contains('.'))
        {
            normalized = normalized.Replace(".", string.Empty, StringComparison.Ordinal).Replace(",", ".", StringComparison.Ordinal);
        }
        else if (normalized.Contains(','))
        {
            normalized = normalized.Replace(",", ".", StringComparison.Ordinal);
        }

        return decimal.TryParse(
                normalized,
                NumberStyles.Number | NumberStyles.AllowCurrencySymbol | NumberStyles.AllowLeadingSign,
                CultureInfo.InvariantCulture,
                out amount)
            || decimal.TryParse(
                normalized,
                NumberStyles.Number | NumberStyles.AllowCurrencySymbol | NumberStyles.AllowLeadingSign,
                CultureInfo.GetCultureInfo("tr-TR"),
                out amount);
    }

    private static string? GetCell(IReadOnlyList<string> cells, int? index)
    {
        if (index is null || index.Value < 0 || index.Value >= cells.Count)
        {
            return null;
        }

        return cells[index.Value];
    }

    private static char DetectDelimiter(string headerLine)
    {
        return CandidateDelimiters
            .OrderByDescending(delimiter => ParseLine(headerLine, delimiter).Count)
            .First();
    }

    private static string NormalizeCell(string value)
    {
        return value.Trim().Trim('"');
    }

    private static string NormalizeHeader(string value)
    {
        return new string(value.Where(char.IsLetterOrDigit).ToArray()).ToLowerInvariant();
    }

    private static string DescribeDelimiter(char delimiter)
    {
        return delimiter switch
        {
            ';' => "semicolon",
            '\t' => "tab",
            _ => "comma"
        };
    }

    private static List<string> ParseLine(string line, char delimiter)
    {
        var cells = new List<string>();
        var current = new System.Text.StringBuilder();
        var inQuotes = false;

        foreach (var character in line)
        {
            if (character == '"')
            {
                inQuotes = !inQuotes;
                continue;
            }

            if (character == delimiter && !inQuotes)
            {
                cells.Add(current.ToString());
                current.Clear();
                continue;
            }

            current.Append(character);
        }

        cells.Add(current.ToString());
        return cells;
    }

    private sealed record HeaderMap(
        int? DateIndex,
        int? DescriptionIndex,
        int? AmountIndex,
        int? CurrencyIndex);
}
