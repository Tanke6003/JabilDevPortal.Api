using JabilDevPortal.Api.DTOs.Application;
using JabilDevPortal.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/applications")]

public class ApplicationsController : ControllerBase
{
    private readonly IApplicationService _svc;
    public ApplicationsController(IApplicationService svc) => _svc = svc;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? department, [FromQuery] int? ownerId)
        => Ok(await _svc.GetAllAsync(department, ownerId));

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
        => Ok(await _svc.GetByIdAsync(id));

    [HttpPost]
    [Authorize(Roles = "Developer,Admin")]
    public async Task<IActionResult> Create(ApplicationCreateDto dto)
    {
        var id = await _svc.CreateAsync(dto);
        return CreatedAtAction(nameof(Get), new { id }, null);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Developer,Admin")]
    public async Task<IActionResult> Update(int id, ApplicationCreateDto dto)
    {
        await _svc.UpdateAsync(id, dto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        await _svc.DeleteAsync(id);
        return NoContent();
    }
}
