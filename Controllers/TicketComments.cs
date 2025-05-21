using JabilDevPortal.Api.DTOs.Comment;
using JabilDevPortal.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/tickets/{ticketId}/comments")]
[Authorize]
public class TicketCommentsController : ControllerBase
{
    private readonly ICommentService _comments;
    public TicketCommentsController(ICommentService comments) => _comments = comments;

    [HttpGet]
    public async Task<IActionResult> GetAll(int ticketId)
        => Ok(await _comments.GetByTicketAsync(ticketId));

    [HttpPost]
    public async Task<IActionResult> Create(int ticketId, CommentCreateDto dto)
    {
        await _comments.CreateAsync(ticketId, dto);
        return StatusCode(201);
    }
}
