using Microsoft.AspNetCore.Mvc;
using PriceNest.Api.Models;
using PriceNest.Api.DTOs;
using Microsoft.EntityFrameworkCore;

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
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == id);
        if (product == null)
        {
            return NotFound();
        }
        return Ok(product);
    }


    [HttpPost]
    public async Task<ActionResult> AddProduct(Product p)
    {
        // To modify, maybe add DTO instead of pure object
        _dbContext.Add(p);
        await _dbContext.SaveChangesAsync();
        return Ok();
    }

}


