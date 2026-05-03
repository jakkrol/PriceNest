using Microsoft.AspNetCore.Mvc;

namespace PriceNest.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly Data.AppDbContext _dbContext;

    public ProductController(Data.AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}

    
