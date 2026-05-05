using Microsoft.AspNetCore.Mvc;
using PriceNest.Api.Data;
using PriceNest.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace PriceNest.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly AppDbContext _dbContext;

    public UserController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

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



    [HttpPost]
    public async Task<ActionResult> CreateUser(User user)
    {
        try
        {
            var exists = await _dbContext.Users.AnyAsync(u => u.Login == user.Login);
            if (exists)
            {
                return Conflict(new { message = "User with this login already exists." });
            }

            //////////////////////////
            /// TODO: Add password hashing and validation
            //////////////////////////
            
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUser), new { login = user.Login }, user);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Unexpected error occurred while creating user.", error = ex.Message});
        }
    }
    
}