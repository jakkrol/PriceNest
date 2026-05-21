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

    // public string GenerateJwtToken(UserLoginDto user)
    // {
    //     var jwtSettings = _configuration.GetSection("JwtSettings");
    //     var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));
    //     var claims = new[]
    //     {
    //         new Claim(JwtRegisteredClaimNames.Sub, user.Login),
    //         new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
    //     };

    //     var token = new JwtSecurityToken(
    //         issuer: jwtSettings["Issuer"],
    //         audience: jwtSettings["Audience"],
    //         claims: claims,
    //         expires: DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["ExpiryMinutes"]!)),
    //         signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
    //     );
    //     return new JwtSecurityTokenHandler().WriteToken(token);
    // }

    // Add JWT token generation and validation here in the future for better authentication management

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

        return Ok(new { message = "Login successful.", token = token });
    }



    [HttpPost("register")]
    public async Task<ActionResult> RegisterUser(UserRegisterDto user)
    {
        if (user == null || string.IsNullOrEmpty(user.Login) || string.IsNullOrEmpty(user.Password) || string.IsNullOrEmpty(user.Email))
        {
            return BadRequest(new { message = "Login, password and email are required." });
        }
        // var exists = await _dbContext.Users.AnyAsync(u => u.Login == user.Login);
        // if (exists)
        // {
        //     return Conflict(new { message = "User with this login already exists." });
        // }

        // string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);
        // var newUser = new User
        // {
        //     Login = user.Login,
        //     Password = hashedPassword,
        //     Email = user.Email
        // };

        // _dbContext.Users.Add(newUser);
        // await _dbContext.SaveChangesAsync();
        return Ok(new { message = "User created successfully." });
    }
}