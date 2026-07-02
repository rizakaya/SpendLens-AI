using Microsoft.AspNetCore.Mvc;
using SpendLens.Application.Analysis;

namespace SpendLens.Api.Controllers;

[ApiController]
[Route("api/subscriptions")]
public sealed class SubscriptionsController(ISpendAnalysisService spendAnalysisService) : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(spendAnalysisService.GetSubscriptions());
    }
}
