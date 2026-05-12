using PriceNest.Api.Models;
using PriceNest.Api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PriceNest.Api.DTOs;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _dbContext;
    private readonly IConfiguration _configuration;
    public AuthController(AppDbContext dbContext, IConfiguration configuration)
    {
        _dbContext = dbContext;
        _configuration = configuration;
    }



    // Add JWT token generation and validation here in the future for better authentication management

    [HttpPost("login")]
    public async Task<ActionResult> LoginUser(UserLoginDto user)
    {
        var existingUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Login == user.Login);
        if (existingUser == null || !BCrypt.Net.BCrypt.Verify(user.Password, existingUser.Password))
        {
            return Unauthorized(new { message = "Invalid login or password." });
        }


        return Ok(new { message = "Login successful." });
    }

    [HttpPost("register")]
    public async Task<ActionResult> RegisterUser(UserRegisterDto user)
    {
        var exists = await _dbContext.Users.AnyAsync(u => u.Login == user.Login);
        if (exists)
        {
            return Conflict(new { message = "User with this login already exists." });
        }

        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);
        var newUser = new User
        {
            Login = user.Login,
            Password = hashedPassword,
            Email = user.Email
        };

        _dbContext.Users.Add(newUser);
        await _dbContext.SaveChangesAsync();
        return Ok(new { message = "User created successfully." });
    }
}