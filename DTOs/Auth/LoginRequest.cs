// DTOs/Auth/LoginRequest.cs
namespace JabilDevPortal.Api.DTOs.Auth
{
    public class LoginRequest
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
