using JabilDevPortal.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/dashboard")]
[Authorize(Roles = "Admin,Support")]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _dash;
    public DashboardController(IDashboardService dash) => _dash = dash;

    [HttpGet("apps-summary")]
    public async Task<IActionResult> GetSummary()
        => Ok(await _dash.GetSummaryAsync());
}
