// Services/Interfaces/IAuthService.cs
using JabilDevPortal.Api.DTOs.Auth;

namespace JabilDevPortal.Api.Services.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponse> LoginAsync(LoginRequest request);
        Task RegisterAsync(RegisterRequest request);
        Task<UserInfoDto> GetCurrentUserAsync(string userId);
    }
}
