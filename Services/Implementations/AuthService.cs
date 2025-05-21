using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JabilDevPortal.Api.Data;
using JabilDevPortal.Api.Data.Entities;
using JabilDevPortal.Api.DTOs.Auth;
using JabilDevPortal.Api.Helpers;
using JabilDevPortal.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using BCryptNet = BCrypt.Net.BCrypt;

namespace JabilDevPortal.Api.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _db;
        private readonly JwtSettings          _jwt;

        public AuthService(ApplicationDbContext db, IOptions<JwtSettings> jwtOpt)
        {
            _db  = db;
            _jwt = jwtOpt.Value;
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            var user = await _db.Users.SingleOrDefaultAsync(u => u.Username == request.Username);
            if (user == null || !BCryptNet.Verify(request.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Usuario o contraseña incorrectos");

            // Claims
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,   user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
                new Claim(ClaimTypes.Role,                user.Role),
                new Claim("department",                   user.Department)
            };

            var key   = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer:         _jwt.Issuer,
                audience:       _jwt.Audience,
                claims:         claims,
                expires:        DateTime.UtcNow.AddMinutes(_jwt.ExpiryMinutes),
                signingCredentials: creds
            );

            return new LoginResponse
            {
                Token      = new JwtSecurityTokenHandler().WriteToken(token),
                Username   = user.Username,
                Role       = user.Role,
                Department = user.Department
            };
        }

        public async Task RegisterAsync(RegisterRequest request)
        {
            if (await _db.Users.AnyAsync(u => u.Username == request.Username))
                throw new InvalidOperationException($"El usuario '{request.Username}' ya existe.");

            var user = new User
            {
                Username     = request.Username,
                PasswordHash = BCryptNet.HashPassword(request.Password),
                FullName     = request.FullName,
                Email        = request.Email,
                Department   = request.Department,
                Role         = request.Role
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();
        }

        public async Task<UserInfoDto> GetCurrentUserAsync(string userId)
        {
            if (!int.TryParse(userId, out var id))
                throw new ArgumentException("Token inválido");

            var user = await _db.Users.SingleOrDefaultAsync(u => u.Id == id);
            if (user == null)
                throw new KeyNotFoundException("Usuario no encontrado");

            return new UserInfoDto
            {
                Id         = user.Id,
                Username   = user.Username,
                FullName   = user.FullName,
                Email      = user.Email,
                Department = user.Department,
                Role       = user.Role
            };
        }
    }
}
