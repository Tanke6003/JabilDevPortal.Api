// DTOs/Auth/RegisterRequest.cs
namespace JabilDevPortal.Api.DTOs.Auth
{
    public class RegisterRequest
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Department { get; set; } = null!;
        public int    RoleId     { get; set; }
    }
}
