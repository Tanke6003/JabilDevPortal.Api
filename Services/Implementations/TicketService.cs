using System.Data;
using BackEnd.Infrastructure.Plugins;
using JabilDevPortal.Api.DTOs.Ticket;
using JabilDevPortal.Api.Services.Interfaces;

public class TicketService : ITicketService
{
    private readonly PGSQLConnectionPlugin _db;
    public TicketService(PGSQLConnectionPlugin db) => _db = db;

    public async Task<int> CreateAsync(TicketCreateDto dto)
    {
        // Verificar que la aplicación exista y obtener al owner
        var appDt = _db.ExecDataTable($"SELECT * FROM applications WHERE id = {dto.ApplicationId} AND available = true");
        if (appDt.Rows.Count == 0)
            throw new KeyNotFoundException($"Application with id {dto.ApplicationId} not found.");

        var application = appDt.Rows[0];
        int? ownerId = application["owner_id"] == DBNull.Value ? (int?)null : Convert.ToInt32(application["owner_id"]);

        string sql = $@"
            INSERT INTO tickets
                (application_id, title, description, status, created_by_id, created_at, assigned_to_id, available)
            VALUES
                (
                    {dto.ApplicationId},
                    '{dto.Title.Replace("'", "''")}',
                    {(dto.Description == null ? "NULL" : $"'{dto.Description.Replace("'", "''")}'")},
                    'Abierto',
                    {dto.CreatedById},
                    NOW(),
                    {(ownerId.HasValue ? ownerId.Value.ToString() : "NULL")},
                    true
                ) RETURNING id;
        ";

        var dt = await Task.Run(() => _db.ExecDataTable(sql));
        return dt.Rows.Count > 0 ? Convert.ToInt32(dt.Rows[0]["id"]) : 0;
    }
    
    public async Task<List<TicketReadDto>> GetAllAsync(int? appId, string? status)
    {
        var sql = "SELECT * FROM tickets WHERE available = true";
        if (appId.HasValue)
            sql += $" AND application_id = {appId.Value}";
        if (!string.IsNullOrEmpty(status))
            sql += $" AND status = '{status.Replace("'", "''")}'";

        var dt = await Task.Run(() => _db.ExecDataTable(sql));
        return dt.Rows.Cast<DataRow>().Select(MapToReadDto).ToList();
    }

    public async Task<TicketReadDto> GetByIdAsync(int id)
    {
        var dt = await Task.Run(() => _db.ExecDataTable($"SELECT * FROM tickets WHERE id = {id} AND available = true"));
        if (dt.Rows.Count == 0)
            throw new KeyNotFoundException($"Ticket with id {id} not found.");
        return MapToReadDto(dt.Rows[0]);
    }

   public async Task<List<TicketReadDto>> GetMyTickets(int userId)
{
    // Construir la consulta SQL para traer los tickets creados o asignados a este usuario
    var sql = $@"
        SELECT * 
        FROM tickets 
        WHERE available = true 
        AND (created_by_id = {userId} OR assigned_to_id = {userId});
    ";

    // Ejecutar la consulta y mapear los resultados
    var dt = await Task.Run(() => _db.ExecDataTable(sql));
    return dt.Rows.Cast<DataRow>().Select(MapToReadDto).ToList();
}

    public async Task UpdateStatusAsync(int id, string status)
    {
        // Solo actualiza el campo status
        var checkDt = _db.ExecDataTable($"SELECT id FROM tickets WHERE id = {id} AND available = true");
        if (checkDt.Rows.Count == 0)
            throw new KeyNotFoundException($"Ticket with id {id} not found.");

        string sql = $@"
            UPDATE tickets SET
                status = '{status.Replace("'", "''")}',
                updated_at = NOW()
            WHERE id = {id};
        ";
        await Task.Run(() => _db.ExecDataTable(sql));
    }

    private TicketReadDto MapToReadDto(DataRow row)
    {
        return new TicketReadDto
        {
            Id = Convert.ToInt32(row["id"]),
            ApplicationId = Convert.ToInt32(row["application_id"]),
            Title = row["title"].ToString()!,
            Description = row["description"] == DBNull.Value ? null : row["description"].ToString(),
            Status = row["status"].ToString()!,
            CreatedById = Convert.ToInt32(row["created_by_id"]),
            CreatedDate = Convert.ToDateTime(row["created_at"]),
            AssignedToId = row["assigned_to_id"] == DBNull.Value ? (int?)null : Convert.ToInt32(row["assigned_to_id"])
        };
    }
}
