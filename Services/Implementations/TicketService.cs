// Services/Implementations/TicketService.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JabilDevPortal.Api.DTOs.Ticket;
using JabilDevPortal.Api.Data;
using JabilDevPortal.Api.Data.Entities;
using JabilDevPortal.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JabilDevPortal.Api.Services.Implementations
{
    public class TicketService : ITicketService
    {
        private readonly ApplicationDbContext _db;
        public TicketService(ApplicationDbContext db) => _db = db;

        public async Task<int> CreateAsync(TicketCreateDto dto)
        {
            // Verificar que la aplicación exista y obtener al owner por defecto
            var application = await _db.Applications.FindAsync(dto.ApplicationId)
                ?? throw new KeyNotFoundException($"Application with id {dto.ApplicationId} not found.");

            var ticket = new Ticket
            {
                ApplicationId = dto.ApplicationId,
                Title         = dto.Title,
                Description   = dto.Description,
                CreatedById   = dto.CreatedById,
                CreatedDate   = DateTime.UtcNow,
                Status        = "Abierto",
                AssignedToId  = application.OwnerUserId
            };

            _db.Tickets.Add(ticket);
            await _db.SaveChangesAsync();
            return ticket.Id;
        }

        public async Task<List<TicketReadDto>> GetAllAsync(int? appId, string? status)
        {
            var query = _db.Tickets.AsQueryable();

            if (appId.HasValue)
                query = query.Where(t => t.ApplicationId == appId.Value);
            if (!string.IsNullOrEmpty(status))
                query = query.Where(t => t.Status == status);

            return await query
                .Select(t => new TicketReadDto
                {
                    Id             = t.Id,
                    ApplicationId  = t.ApplicationId,
                    Title          = t.Title,
                    Description    = t.Description,
                    Status         = t.Status,
                    CreatedById    = t.CreatedById,
                    CreatedDate    = t.CreatedDate,
                    AssignedToId   = t.AssignedToId
                })
                .ToListAsync();
        }

        public async Task<TicketReadDto> GetByIdAsync(int id)
        {
            var t = await _db.Tickets.FindAsync(id)
                ?? throw new KeyNotFoundException($"Ticket with id {id} not found.");

            return new TicketReadDto
            {
                Id             = t.Id,
                ApplicationId  = t.ApplicationId,
                Title          = t.Title,
                Description    = t.Description,
                Status         = t.Status,
                CreatedById    = t.CreatedById,
                CreatedDate    = t.CreatedDate,
                AssignedToId   = t.AssignedToId
            };
        }

        public async Task UpdateStatusAsync(int id, string status)
        {
            var ticket = await _db.Tickets.FindAsync(id)
                ?? throw new KeyNotFoundException($"Ticket with id {id} not found.");

            ticket.Status = status;
            await _db.SaveChangesAsync();
        }
    }
}
