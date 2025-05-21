


namespace JabilDevPortal.Api.Services.Implementations
{
   using System.Data;
    using BackEnd.Infrastructure.Plugins;
    using JabilDevPortal.Api.DTOs.Comment;
using JabilDevPortal.Api.Services.Interfaces;

public class CommentService : ICommentService
{
    private readonly PGSQLConnectionPlugin _db;

    public CommentService(PGSQLConnectionPlugin db) => _db = db;

    public async Task<List<CommentReadDto>> GetByTicketAsync(int ticketId)
    {
        // Verifica que el ticket exista
        var ticketCheck = _db.ExecDataTable($"SELECT 1 FROM tickets WHERE id = {ticketId} AND available = true");
        if (ticketCheck.Rows.Count == 0)
            throw new KeyNotFoundException($"Ticket with id {ticketId} not found.");

        // Obtiene los comentarios junto con el nombre del autor
        string sql = $@"
            SELECT c.id, c.ticket_id, c.user_id, u.name AS author_name, c.comment, c.created_at
            FROM ticket_comments c
            INNER JOIN users u ON c.user_id = u.id
            WHERE c.ticket_id = {ticketId} AND c.available = true
            ORDER BY c.created_at;
        ";

        var dt = await Task.Run(() => _db.ExecDataTable(sql));
        return dt.Rows.Cast<DataRow>().Select(row => new CommentReadDto
        {
            Id         = Convert.ToInt32(row["id"]),
            TicketId   = Convert.ToInt32(row["ticket_id"]),
            AuthorId   = Convert.ToInt32(row["user_id"]),
            AuthorName = row["author_name"].ToString()!,
            Comment    = row["comment"].ToString()!,
            CreatedAt  = Convert.ToDateTime(row["created_at"])
        }).ToList();
    }

    public async Task<int> CreateAsync(int ticketId, CommentCreateDto dto)
    {
        // Verifica que el ticket exista
        var ticketCheck = _db.ExecDataTable($"SELECT 1 FROM tickets WHERE id = {ticketId} AND available = true");
        if (ticketCheck.Rows.Count == 0)
            throw new KeyNotFoundException($"Ticket with id {ticketId} not found.");

        // Verifica que el usuario (autor) exista
        var userCheck = _db.ExecDataTable($"SELECT 1 FROM users WHERE id = {dto.AuthorId} AND available = true");
        if (userCheck.Rows.Count == 0)
            throw new KeyNotFoundException($"User with id {dto.AuthorId} not found.");

        // Inserta el comentario y regresa el id
        string sql = $@"
            INSERT INTO ticket_comments (ticket_id, user_id, comment, created_at, available)
            VALUES (
                {ticketId},
                {dto.AuthorId},
                '{dto.Comment.Replace("'", "''")}',
                NOW(),
                true
            ) RETURNING id;
        ";

        var dt = await Task.Run(() => _db.ExecDataTable(sql));
        return dt.Rows.Count > 0 ? Convert.ToInt32(dt.Rows[0]["id"]) : 0;
    }
}

}
