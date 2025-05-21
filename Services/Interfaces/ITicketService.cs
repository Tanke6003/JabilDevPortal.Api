// Services/Interfaces/ITicketService.cs
using JabilDevPortal.Api.DTOs.Ticket;

namespace JabilDevPortal.Api.Services.Interfaces
{
    public interface ITicketService
    {
        Task<int> CreateAsync(TicketCreateDto dto);
        Task<List<TicketReadDto>> GetAllAsync(int? appId, string? status);
        Task<TicketReadDto> GetByIdAsync(int id);
        Task UpdateStatusAsync(int id, string status);
    }
}