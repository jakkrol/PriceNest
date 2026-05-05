using Microsoft.AspNetCore.Mvc;
using PriceNest.Api.Models;

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

    [HttpGet]
    public async Task<ActionResult<Product>> GetProduct()
    {
        var product = new Product { Name = "test" };
        return Ok(product);
    }

    
}

    
