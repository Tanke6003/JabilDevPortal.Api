using JabilDevPortal.Api.DTOs.Report;
using JabilDevPortal.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/reports")]
[Authorize(Roles = "Admin,Support")]
public class ReportsController : ControllerBase
{
    private readonly IReportService _reports;
    public ReportsController(IReportService reports) => _reports = reports;

    [HttpGet("incidents")]
    public async Task<IActionResult> Incidents([FromQuery] IncidentReportDto dto)
        => Ok(await _reports.GetIncidentsAsync(dto.From, dto.To, dto.Department));
}
