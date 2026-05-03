using Microsoft.AspNetCore.Mvc;
using PriceNest.Api.Data;

namespace PriceNest.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PriceHistoryController
{
    private readonly AppDbContext _dbContext;

    public PriceHistoryController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    
}