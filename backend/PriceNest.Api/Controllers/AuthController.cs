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

        var token = await _authService.LoginUserAsync(user);

        if (token == null)
        {
            return Unauthorized(new { message = "Invalid login or password." });
        }

        var accessToken = token.Value.AccessToken;
        var refreshToken = token.Value.RefreshToken;

        var accessTokenOptions = new CookieOptions
        {
            HttpOnly = true, // Kluczowe dla bezpieczeństwa! Skrypty JS (XSS) nie mają dostępu do tego ciastka.
            Secure = false,   // Wymagane, jeśli SameSite = None. Wymusza przesyłanie po HTTPS (lub localhost).
            SameSite = SameSiteMode.Lax, // Pozwala na przesyłanie ciastka cross-origin (z portu 5295 do 3000)
            Expires = DateTime.UtcNow.AddMinutes(15), // Ważność ciastka (np. 7 dni)
            Path = "/"
        };

        var refreshTokenOptions = new CookieOptions
        {
            HttpOnly = true,   // Blokuje dostęp dla skryptów JS
            Secure = false,    // false dla localhost HTTP
            SameSite = SameSiteMode.Lax,
            Expires = DateTime.UtcNow.AddDays(7), // Ważność 7 dni, tak jak w bazie danych

            //Not setting path becouse frontend needs to access this cookie in order to not log out on refresh if access token is expired
            Path = "/"
        };

        Response.Cookies.Append("access_token", accessToken, accessTokenOptions);
        Response.Cookies.Append("refresh_token", refreshToken, refreshTokenOptions);


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


    [HttpPost("refresh")]
    public async Task<ActionResult> RefreshToken()
    {
        Console.WriteLine("Attempting to refresh token");
        if (!Request.Cookies.TryGetValue("refresh_token", out string? currentRefreshToken) || string.IsNullOrEmpty(currentRefreshToken))
        {
            return BadRequest(new { message = "Refresh token missing" });
        }

        var resToken = await _authService.RefreshTokensAsync(currentRefreshToken);

        if (resToken == null)
        {
            return Unauthorized(new { message = "Invalid or expired token" });
        }

        var accessTokenOptions = new CookieOptions
        {
            HttpOnly = true, // Kluczowe dla bezpieczeństwa! Skrypty JS (XSS) nie mają dostępu do tego ciastka.
            Secure = false,   // Wymagane, jeśli SameSite = None. Wymusza przesyłanie po HTTPS (lub localhost).
            SameSite = SameSiteMode.Lax, // Pozwala na przesyłanie ciastka cross-origin (z portu 5295 do 3000)
            Expires = DateTime.UtcNow.AddMinutes(15), // Ważność ciastka (np. 7 dni)
            Path = "/"
        };
        Response.Cookies.Append("access_token", resToken.Value.AccessToken, accessTokenOptions);
        if (!string.IsNullOrEmpty(resToken.Value.RefreshToken))
        {
            var refreshTokenOptions = new CookieOptions
            {
                HttpOnly = true,   // Blokuje dostęp dla skryptów JS
                Secure = false,    // false dla localhost HTTP
                SameSite = SameSiteMode.Lax,
                Expires = DateTime.UtcNow.AddDays(7), // Ważność 7 dni, tak jak w bazie danych

                //Not setting path becouse frontend needs to access this cookie in order to not log out on refresh if access token is expired
                Path = "/"!
            };
            Response.Cookies.Append("refresh_token", resToken.Value.RefreshToken, refreshTokenOptions);
        }

        return Ok(new { message = "Token refreshed" });
    }



}