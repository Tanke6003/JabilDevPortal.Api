
using System.Data;
using BackEnd.Infrastructure.Plugins;

public class RoleService : IRoleService
{
    private readonly PGSQLConnectionPlugin _db;

    public RoleService(PGSQLConnectionPlugin db)
    {
        _db = db;
    }

    public List<RoleDto> GetAll()
    {
        var result = new List<RoleDto>();
        string sql = "SELECT * FROM roles";
        var dt = _db.ExecDataTable(sql);

        foreach (DataRow row in dt.Rows)
        {
            result.Add(new RoleDto
            {
                Id = Convert.ToInt32(row["id"]),
                Name = row["name"].ToString()!,
                Description = row["description"] == DBNull.Value ? null : row["description"].ToString(),
                Available = Convert.ToBoolean(row["available"])
            });
        }
        return result;
    }

    public RoleDto? GetById(int id)
    {
        string sql = $"SELECT * FROM roles WHERE id = {id}";
        var dt = _db.ExecDataTable(sql);
        if (dt.Rows.Count == 0) return null;

        var row = dt.Rows[0];
        return new RoleDto
        {
            Id = Convert.ToInt32(row["id"]),
            Name = row["name"].ToString()!,
            Description = row["description"] == DBNull.Value ? null : row["description"].ToString(),
            Available = Convert.ToBoolean(row["available"])
        };
    }

    public void Create(RoleCreateUpdateDto dto)
    {
        string sql = $@"
            INSERT INTO roles (name, description, available)
            VALUES (
                '{dto.Name.Replace("'", "''")}',
                {(dto.Description == null ? "NULL" : $"'{dto.Description.Replace("'", "''")}'")},
                {dto.Available.ToString().ToLower()}
            );
        ";
        _db.ExecNonQuery(sql);
    }

    public void Update(int id, RoleCreateUpdateDto dto)
    {
        string sql = $@"
            UPDATE roles SET
                name = '{dto.Name.Replace("'", "''")}',
                description = {(dto.Description == null ? "NULL" : $"'{dto.Description.Replace("'", "''")}'")},
                available = {dto.Available.ToString().ToLower()}
            WHERE id = {id};
        ";
        _db.ExecNonQuery(sql);
    }

    public void Delete(int id)
    {
        string sql = $"DELETE FROM roles WHERE id = {id}";
        _db.ExecNonQuery(sql);
    }
}