// Services/Implementations/CommentService.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JabilDevPortal.Api.DTOs.Comment;
using JabilDevPortal.Api.Data;
using JabilDevPortal.Api.Data.Entities;
using JabilDevPortal.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JabilDevPortal.Api.Services.Implementations
{
    public class CommentService : ICommentService
    {
        private readonly ApplicationDbContext _db;
        public CommentService(ApplicationDbContext db) => _db = db;

        public async Task<List<CommentReadDto>> GetByTicketAsync(int ticketId)
        {
            // Verifica que el ticket exista
            if (!await _db.Tickets.AnyAsync(t => t.Id == ticketId))
                throw new KeyNotFoundException($"Ticket with id {ticketId} not found.");

            // Obtiene los comentarios ordenados por fecha
            var comments = await _db.TicketComments
                .Where(c => c.TicketId == ticketId)
                .Include(c => c.Author)
                .OrderBy(c => c.CreatedAt)
                .ToListAsync();

            // Mapea a DTO
            return comments.Select(c => new CommentReadDto
            {
                Id         = c.Id,
                TicketId   = c.TicketId,
                AuthorId   = c.AuthorId,
                AuthorName = c.Author.FullName,
                Comment    = c.Comment,
                CreatedAt  = c.CreatedAt
            }).ToList();
        }

        public async Task<int> CreateAsync(int ticketId, CommentCreateDto dto)
        {
            // Verifica que el ticket exista
            var ticket = await _db.Tickets.FindAsync(ticketId)
                ?? throw new KeyNotFoundException($"Ticket with id {ticketId} not found.");

            // Verifica que el autor exista
            if (!await _db.Users.AnyAsync(u => u.Id == dto.AuthorId))
                throw new KeyNotFoundException($"User with id {dto.AuthorId} not found.");

            // Crea y guarda el comentario
            var comment = new TicketComment
            {
                TicketId  = ticketId,
                AuthorId  = dto.AuthorId,
                Comment   = dto.Comment,
                CreatedAt = DateTime.UtcNow
            };

            _db.TicketComments.Add(comment);
            await _db.SaveChangesAsync();

            return comment.Id;
        }
    }
}
