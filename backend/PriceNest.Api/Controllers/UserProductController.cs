using Microsoft.AspNetCore.Mvc;
using PriceNest.Api.Data;

namespace PriceNest.Api.Controllers;

[ApiController]
[Route("api/[controller]")]

public class UserProductController : ControllerBase
{
    private readonly AppDbContext _dbContext;

    public UserProductController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }


}