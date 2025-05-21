// Services/Interfaces/ICommentService.cs
using JabilDevPortal.Api.DTOs.Comment;

namespace JabilDevPortal.Api.Services.Interfaces
{
    public interface ICommentService
    {
        Task<List<CommentReadDto>> GetByTicketAsync(int ticketId);
        Task<int> CreateAsync(int ticketId, CommentCreateDto dto);
    }
}