using System.Data;
using BackEnd.Infrastructure.Plugins;

public class UserService : IUserService
{
    private readonly PGSQLConnectionPlugin _db;
    public UserService(PGSQLConnectionPlugin db) => _db = db;

    public async Task<List<UserReadDto>> GetAllAsync()
    {
        string sql = @"
            SELECT u.id, u.name, u.email, u.role_id, u.created_at, u.available, r.name AS role_name
            FROM users u
            LEFT JOIN roles r ON u.role_id = r.id
            ORDER BY u.id;
        ";
        var dt = await Task.Run(() => _db.ExecDataTable(sql));
        return dt.Rows.Cast<DataRow>().Select(MapToReadDto).ToList();
    }

    public async Task<UserReadDto?> GetByIdAsync(int id)
    {
        string sql = @"
            SELECT u.id, u.name, u.email, u.role_id, u.created_at, u.available, r.name AS role_name
            FROM users u
            LEFT JOIN roles r ON u.role_id = r.id
            WHERE u.id = " + id + @";
        ";
        var dt = await Task.Run(() => _db.ExecDataTable(sql));
        if (dt.Rows.Count == 0)
            return null;
        return MapToReadDto(dt.Rows[0]);
    }

    public async Task<List<UserReadDto>> GetByRoleAsync(int roleId)
    {
        string sql = @"
            SELECT u.id, u.name, u.email, u.role_id, u.created_at, u.available, r.name AS role_name
            FROM users u
            LEFT JOIN roles r ON u.role_id = r.id
            WHERE u.role_id = " + roleId + @"
            ORDER BY u.id;
        ";
        var dt = await Task.Run(() => _db.ExecDataTable(sql));
        return dt.Rows.Cast<DataRow>().Select(MapToReadDto).ToList();
    }

    public async Task UpdateAsync(int id, UserEditDto dto)
    {
        // Valida existencia del usuario
        var exists = _db.ExecDataTable($"SELECT 1 FROM users WHERE id = {id}");
        if (exists.Rows.Count == 0)
            throw new KeyNotFoundException($"User with id {id} not found.");

        string sql = $@"
            UPDATE users SET
                name = '{dto.Name.Replace("'", "''")}',
                email = '{dto.Email.Replace("'", "''")}',
                role_id = {(dto.RoleId.HasValue ? dto.RoleId.Value.ToString() : "NULL")},
                available = {dto.Available.ToString().ToLower()}
            WHERE id = {id};
        ";
        await Task.Run(() => _db.ExecNonQuery(sql));
    }

    private UserReadDto MapToReadDto(DataRow row)
    {
        return new UserReadDto
        {
            Id = Convert.ToInt32(row["id"]),
            Name = row["name"].ToString()!,
            Email = row["email"].ToString()!,
            RoleId = row["role_id"] == DBNull.Value ? null : (int?)Convert.ToInt32(row["role_id"]),
            RoleName = row["role_name"] == DBNull.Value ? null : row["role_name"].ToString(),
            CreatedAt = Convert.ToDateTime(row["created_at"]),
            Available = Convert.ToBoolean(row["available"])
        };
    }
}
