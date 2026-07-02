using Microsoft.AspNetCore.Mvc;
using SpendLens.Application.Analysis;

namespace SpendLens.Api.Controllers;

[ApiController]
[Route("api/dashboard")]
public sealed class DashboardController(ISpendAnalysisService spendAnalysisService) : ControllerBase
{
    [HttpGet("summary")]
    public IActionResult GetSummary([FromQuery] int year, [FromQuery] int month)
    {
        return Ok(spendAnalysisService.GetMonthlySummary(year, month));
    }

    [HttpGet("category-breakdown")]
    public IActionResult GetCategoryBreakdown([FromQuery] int year, [FromQuery] int month)
    {
        return Ok(spendAnalysisService.GetCategoryBreakdown(year, month));
    }

    [HttpGet("top-merchants")]
    public IActionResult GetTopMerchants([FromQuery] int year, [FromQuery] int month, [FromQuery] int take = 5)
    {
        return Ok(spendAnalysisService.GetTopMerchants(year, month, take));
    }

    [HttpGet("report")]
    public async Task<IActionResult> GetReport([FromQuery] int year, [FromQuery] int month, CancellationToken cancellationToken)
    {
        return Ok(await spendAnalysisService.BuildMonthlyReportAsync(year, month, cancellationToken));
    }
}
