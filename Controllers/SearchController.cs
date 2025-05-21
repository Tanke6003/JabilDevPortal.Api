using JabilDevPortal.Api.DTOs.Search;
using JabilDevPortal.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/search")]
[Authorize]
public class SearchController : ControllerBase
{
    private readonly IApplicationService _apps;
    public SearchController(IApplicationService apps) => _apps = apps;

    [HttpGet("applications")]
    public async Task<IActionResult> SearchApps([FromQuery] ApplicationSearchDto dto)
        => Ok(await _apps.SearchAsync(dto.Query, dto.Department, dto.OwnerId));
}
