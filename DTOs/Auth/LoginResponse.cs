// DTOs/Auth/LoginResponse.cs
namespace JabilDevPortal.Api.DTOs.Auth
{
    public class LoginResponse
    {
        public string Token    { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Role     { get; set; } = null!;
        public string Department { get; set; } = null!;
    }
}
