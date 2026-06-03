using PriceNest.Api.Models;
using PriceNest.Api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PriceNest.Api.DTOs;
using PriceNest.Api.Services;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;


[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _dbContext;
    private readonly IConfiguration _configuration;
    private readonly AuthService _authService;
    public AuthController(AppDbContext dbContext, IConfiguration configuration, AuthService authService)
    {
        _dbContext = dbContext;
        _configuration = configuration;
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<ActionResult> LoginUser(UserLoginDto user)
    {
        if (user == null || string.IsNullOrEmpty(user.Login) || string.IsNullOrEmpty(user.Password))
        {
            return BadRequest(new { message = "Login and password are required." });
        }

        string? token = await _authService.LoginUserAsync(user);

        if (token == null)
        {
            return Unauthorized(new { message = "Invalid login or password." });
        }

        var cookiesOptions = new CookieOptions
        {
            HttpOnly = true, // Kluczowe dla bezpieczeństwa! Skrypty JS (XSS) nie mają dostępu do tego ciastka.
            Secure = false,   // Wymagane, jeśli SameSite = None. Wymusza przesyłanie po HTTPS (lub localhost).
            SameSite = SameSiteMode.Lax, // Pozwala na przesyłanie ciastka cross-origin (z portu 5295 do 3000)
            Expires = DateTime.UtcNow.AddDays(7) // Ważność ciastka (np. 7 dni)
        };

        Response.Cookies.Append("token", token, cookiesOptions);

        return Ok(new { message = "Login successful." });
    }



    [HttpPost("register")]
    public async Task<ActionResult> RegisterUser(UserRegisterDto user)
    {
        if (user == null || string.IsNullOrEmpty(user.Login) || string.IsNullOrEmpty(user.Password) || string.IsNullOrEmpty(user.Email))
        {
            return BadRequest(new { message = "Login, password and email are required." });
        }

        var res = await _authService.RegisterUserAsync(user);
        if (!res)
        {
            return Conflict(new { message = "User with this login already exists." });
        }

        return Ok(new { message = "User created successfully." });
    }
}