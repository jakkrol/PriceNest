using Microsoft.AspNetCore.Mvc;
using PriceNest.Api.Data;
using PriceNest.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
namespace PriceNest.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly AppDbContext _dbContext;

    public UserController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    // [HttpGet("error")]
    // public ActionResult TriggerError()
    // {
    //     throw new Exception("Test error");
    // }

    [HttpGet]
    public async Task<ActionResult<User>> GetUser(string login)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Login == login);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }



    // [HttpPost]
    // public async Task<ActionResult> CreateUser(User user)
    // {
    //     var exists = await _dbContext.Users.AnyAsync(u => u.Login == user.Login);
    //     if (exists)
    //     {
    //         return Conflict(new { message = "User with this login already exists." });
    //     }



    //     string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);
    //     user.Password = hashedPassword;
    //     //////////////////////////
    //     /// TODO: Add password hashing and validation
    //     //////////////////////////


    //     _dbContext.Users.Add(user);
    //     await _dbContext.SaveChangesAsync();
    //     return CreatedAtAction(nameof(GetUser), new { login = user.Login }, user);
    // }



}