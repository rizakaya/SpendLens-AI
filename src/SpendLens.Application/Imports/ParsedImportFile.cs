namespace SpendLens.Application.Imports;

public sealed record ParsedImportFile(
    string FileName,
    string Delimiter,
    IReadOnlyList<string> Headers,
    IReadOnlyList<ParsedTransactionRow> Rows,
    IReadOnlyList<string> Warnings);
