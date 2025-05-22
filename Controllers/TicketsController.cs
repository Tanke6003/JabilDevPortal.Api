
using System.Security.Cryptography;
using JabilDevPortal.Api.DTOs.Ticket;
using JabilDevPortal.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/tickets")]

public class TicketsController : ControllerBase
{
    private readonly ITicketService _tickets;
    public TicketsController(ITicketService tickets) => _tickets = tickets;

    [HttpPost]
    public async Task<IActionResult> Create(TicketCreateDto dto)
    {
        var id = await _tickets.CreateAsync(dto);
        return CreatedAtAction(nameof(Get), new { id }, null);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int? appId, [FromQuery] string? status)
        => Ok(await _tickets.GetAllAsync(appId, status));

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
        => Ok(await _tickets.GetByIdAsync(id));

    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateStatus(int id, TicketUpdateStatusDto dto)
    {
        await _tickets.UpdateStatusAsync(id, dto.Status);
        return NoContent();
    }
    [HttpGet("my-tickets")]
    public async Task<IActionResult> GetMyTickets(int userId)
    {
        try
        {
            return Ok(await _tickets.GetMyTickets(userId));
        }
        catch (Exception ex)
        {

            return StatusCode(500, ex.Message);
        }
    }
}
