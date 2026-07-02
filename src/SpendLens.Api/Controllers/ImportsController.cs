using Microsoft.AspNetCore.Mvc;
using SpendLens.Api.Contracts.Imports;
using SpendLens.Application.Analysis;
using SpendLens.Application.Imports;

namespace SpendLens.Api.Controllers;

[ApiController]
[Route("api/imports")]
public sealed class ImportsController : ControllerBase
{
    [HttpGet]
    public IActionResult GetImports([FromServices] ISpendAnalysisService spendAnalysisService)
    {
        return Ok(spendAnalysisService.GetImports());
    }

    [HttpPost("preview")]
    [ProducesResponseType(typeof(ImportPreviewResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<ImportPreviewResponse> Preview(
        [FromBody] ImportPreviewRequestContract request,
        [FromServices] IImportPreviewService importPreviewService)
    {
        if (string.IsNullOrWhiteSpace(request.Content))
        {
            return ValidationProblem("Content is required.");
        }

        var response = importPreviewService.BuildPreview(new ImportPreviewRequest(request.Content, request.FileName));
        return Ok(response);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ImportTransactionsResponseContract), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ImportTransactionsResponseContract>> Import(
        [FromBody] ImportTransactionsRequestContract request,
        [FromServices] ITransactionImportService transactionImportService,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Content))
        {
            return ValidationProblem("Content is required.");
        }

        if (string.IsNullOrWhiteSpace(request.FileName))
        {
            return ValidationProblem("FileName is required.");
        }

        var result = await transactionImportService.ImportAsync(
            new ImportTransactionsRequest(request.Content, request.FileName),
            cancellationToken);

        return Ok(new ImportTransactionsResponseContract(
            result.ImportBatch.Id,
            result.ImportBatch.FileName,
            result.Transactions.Count,
            result.Warnings));
    }
}
