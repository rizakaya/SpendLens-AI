using Microsoft.AspNetCore.Mvc;
using SpendLens.Application.Analysis;
using SpendLens.Application.Transactions;

namespace SpendLens.Api.Controllers;

[ApiController]
[Route("api/transactions")]
public sealed class TransactionsController(ISpendAnalysisService spendAnalysisService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(TransactionListResponse), StatusCodes.Status200OK)]
    public ActionResult<TransactionListResponse> GetTransactions(
        [FromQuery] int? year,
        [FromQuery] int? month,
        [FromQuery] string? category)
    {
        return Ok(spendAnalysisService.GetTransactions(year, month, category));
    }
}
