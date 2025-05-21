using JabilDevPortal.Api.DTOs.Auth;
using JabilDevPortal.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _auth;
    public AuthController(IAuthService auth) => _auth = auth;

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest req)
        => Ok(await _auth.LoginAsync(req));

    [HttpPost("register")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Register(RegisterRequest req)
    {
        await _auth.RegisterAsync(req);
        return StatusCode(201);
    }
}
