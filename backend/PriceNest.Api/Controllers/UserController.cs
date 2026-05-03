using Microsoft.AspNetCore.Mvc;
using PriceNest.Api.Data;

namespace PriceNest.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController
{
    private readonly AppDbContext _dbContext;

    public UserController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    
}