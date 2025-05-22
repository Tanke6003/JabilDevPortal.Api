using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BackEnd.Infrastructure.Plugins;
using JabilDevPortal.Api.DTOs.Auth;
using JabilDevPortal.Api.Helpers;
using JabilDevPortal.Api.Services.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using BCryptNet = BCrypt.Net.BCrypt;

namespace JabilDevPortal.Api.Services.Implementations
{
    public class AuthService : IAuthService
{
    private readonly PGSQLConnectionPlugin _db;
    private readonly JwtSettings _jwt;

    public AuthService(PGSQLConnectionPlugin db, IOptions<JwtSettings> jwtOpt)
    {
        _db = db;
        _jwt = jwtOpt.Value;
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        string sql = "SELECT * FROM users WHERE email = @email AND available = true";
        var dt = _db.ExecDataTable(sql.Replace("@email", $"'{request.Email.Replace("'", "''")}'"));
        if (dt.Rows.Count == 0)
            throw new UnauthorizedAccessException("Usuario o contraseña incorrectos");

        var row = dt.Rows[0];
        var hashedPassword = row["password"].ToString();
        if (string.IsNullOrEmpty(hashedPassword) || !BCryptNet.Verify(request.Password, hashedPassword))
            throw new UnauthorizedAccessException("Usuario o contrase    incorrectos");

        var userId = Convert.ToInt32(row["id"]);
        var userName = row["name"].ToString();
        var roleId = row["role_id"]?.ToString() ?? "0";

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, userName),
            new Claim(JwtRegisteredClaimNames.Typ, roleId)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwt.Issuer,
            audience: _jwt.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwt.ExpiryMinutes),
            signingCredentials: creds
        );

        return new LoginResponse
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Username = userName,
            Role = roleId
        };
    }

    public async Task RegisterAsync(RegisterRequest request)
    {
        // Validar si el usuario/email existe
        string checkSql = $"SELECT 1 FROM users WHERE email = '{request.Email.Replace("'", "''")}'";
        var dt = _db.ExecDataTable(checkSql);
        if (dt.Rows.Count > 0)
            throw new InvalidOperationException($"El usuario '{request.Email}' ya existe.");

        var passwordHash = BCryptNet.HashPassword(request.Password);

        string sql = $@"
            INSERT INTO users (name, email, password, role_id, created_at, available)
            VALUES (
                '{request.FullName.Replace("'", "''")}',
                '{request.Email.Replace("'", "''")}',
                '{passwordHash}',
                {3},
                now(),
                true
            );
        ";

        _db.ExecDataTable(sql);
    }

    public async Task<UserInfoDto> GetCurrentUserAsync(string userId)
    {
        if (!int.TryParse(userId, out var id))
            throw new ArgumentException("Token inválido");

        string sql = $"SELECT * FROM users WHERE id = {id} AND available = true";
        var dt = _db.ExecDataTable(sql);

        if (dt.Rows.Count == 0)
            throw new KeyNotFoundException("Usuario no encontrado");

        var row = dt.Rows[0];

        return new UserInfoDto
        {
            Id = Convert.ToInt32(row["id"]),
            Username = row["email"].ToString(),
            FullName = row["name"].ToString(),
            Email = row["email"].ToString(),
            RoleId = row["role_id"] == DBNull.Value ? null : (int?)Convert.ToInt32(row["role_id"])
        };
    }
}

}
