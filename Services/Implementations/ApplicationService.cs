
using JabilDevPortal.Api.Services.Interfaces;

using System.Data;

using BackEnd.Infrastructure.Plugins;

public class ApplicationService : IApplicationService
{
    private readonly PGSQLConnectionPlugin _db;

    public ApplicationService(PGSQLConnectionPlugin db)
    {
        _db = db;
    }

    public async Task<List<ApplicationReadDto>> GetAllAsync()
    {
        // Si no tienes campo Department, omite ese filtro
        var sql = "SELECT * FROM applications order by id";
    

        var dt = await Task.Run(() => _db.ExecDataTable(sql));
        return dt.Rows.Cast<DataRow>().Select(MapToReadDto).ToList();
    }

    public async Task<ApplicationReadDto> GetByIdAsync(int id)
    {
        var sql = $"SELECT * FROM applications WHERE id = {id}";
        var dt = await Task.Run(() => _db.ExecDataTable(sql));
        if (dt.Rows.Count == 0)
            throw new KeyNotFoundException($"Application with id {id} not found.");
        return MapToReadDto(dt.Rows[0]);
    }

    public async Task<int> CreateAsync(ApplicationCreateDto dto)
    {
        string sql = $@"
            INSERT INTO applications 
                (name, description, url, server, repository, owner_id, support_email, sme_email, db_server, created_at, available)
            VALUES (
                '{dto.Name.Replace("'", "''")}',
                {(dto.Description == null ? "NULL" : $"'{dto.Description.Replace("'", "''")}'")},
                {(dto.Url == null ? "NULL" : $"'{dto.Url.Replace("'", "''")}'")},
                {(dto.Server == null ? "NULL" : $"'{dto.Server.Replace("'", "''")}'")},
                {(dto.Repository == null ? "NULL" : $"'{dto.Repository.Replace("'", "''")}'")},
                {(dto.OwnerId.HasValue ? dto.OwnerId.Value.ToString() : "NULL")},
                '{dto.SupportEmail.Replace("'", "''")}',
                {(dto.SmeEmail == null ? "NULL" : $"'{dto.SmeEmail.Replace("'", "''")}'")},
                {(dto.DbServer == null ? "NULL" : $"'{dto.DbServer.Replace("'", "''")}'")},
                NOW(),
                true
            )
            RETURNING id;
        ";

        var dt = await Task.Run(() => _db.ExecDataTable(sql));
        return dt.Rows.Count > 0 ? Convert.ToInt32(dt.Rows[0]["id"]) : 0;
    }

    public async Task UpdateAsync(int id, ApplicationCreateDto dto)
    {
        string sql = $@"
            UPDATE applications SET
                name = '{dto.Name.Replace("'", "''")}',
                description = {(dto.Description == null ? "NULL" : $"'{dto.Description.Replace("'", "''")}'")},
                url = {(dto.Url == null ? "NULL" : $"'{dto.Url.Replace("'", "''")}'")},
                server = {(dto.Server == null ? "NULL" : $"'{dto.Server.Replace("'", "''")}'")},
                repository = {(dto.Repository == null ? "NULL" : $"'{dto.Repository.Replace("'", "''")}'")},
                owner_id = {(dto.OwnerId.HasValue ? dto.OwnerId.Value.ToString() : "NULL")},
                support_email = '{dto.SupportEmail.Replace("'", "''")}',
                sme_email = {(dto.SmeEmail == null ? "NULL" : $"'{dto.SmeEmail.Replace("'", "''")}'")},
                db_server = {(dto.DbServer == null ? "NULL" : $"'{dto.DbServer.Replace("'", "''")}'")},
                available = {(dto.Available ? "true" : "false")}
            WHERE id = {id};
        ";
        
            await Task.Run(() => _db.ExecDataTable(sql));
    }

    public async Task DeleteAsync(int id)
    {
        string sql = $"DELETE FROM applications WHERE id = {id}";
        await Task.Run(() => _db.ExecNonQuery(sql));
    }

    public async Task<List<ApplicationReadDto>> SearchAsync(string? query, string? department, int? ownerId)
    {
        var sql = "SELECT * FROM applications WHERE 1=1";
        if (!string.IsNullOrEmpty(query))
            sql += $" AND (name ILIKE '%{query.Replace("'", "''")}%'" +
                   $" OR description ILIKE '%{query.Replace("'", "''")}%')";
        if (ownerId.HasValue)
            sql += $" AND owner_id = {ownerId.Value}";

        // TODO: Si tienes el campo 'department' en la tabla, descomenta esto
        // if (!string.IsNullOrEmpty(department))
        //     sql += $" AND department = '{department.Replace("'", "''")}'";

        var dt = await Task.Run(() => _db.ExecDataTable(sql));
        return dt.Rows.Cast<DataRow>().Select(MapToReadDto).ToList();
    }

    private ApplicationReadDto MapToReadDto(DataRow row)
    {
        return new ApplicationReadDto
        {
            Id = Convert.ToInt32(row["id"]),
            Name = row["name"].ToString()!,
            Description = row["description"] == DBNull.Value ? null : row["description"].ToString(),
            Url = row["url"] == DBNull.Value ? null : row["url"].ToString(),
            Server = row["server"] == DBNull.Value ? null : row["server"].ToString(),
            Repository = row["repository"] == DBNull.Value ? null : row["repository"].ToString(),
            OwnerId = row["owner_id"] == DBNull.Value ? null : (int?)Convert.ToInt32(row["owner_id"]),
            SupportEmail = row["support_email"].ToString()!,
            SmeEmail = row["sme_email"] == DBNull.Value ? null : row["sme_email"].ToString(),
            DbServer = row["db_server"] == DBNull.Value ? null : row["db_server"].ToString(),
            CreatedAt = Convert.ToDateTime(row["created_at"]),
            Available = Convert.ToBoolean(row["available"])
        };
    }
}
