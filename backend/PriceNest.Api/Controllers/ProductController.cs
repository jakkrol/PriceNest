using Microsoft.AspNetCore.Mvc;
using PriceNest.Api.Models;
using PriceNest.Api.DTOs;
using Microsoft.EntityFrameworkCore;
using PriceNest.Api.Services;

namespace PriceNest.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly Data.AppDbContext _dbContext;
    private readonly ScraperService _scraperService;
    public ProductController(Data.AppDbContext dbContext, ScraperService scraperService)
    {
        _dbContext = dbContext;
        _scraperService = scraperService;
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



    ///scraping endpoint, to be called by the scrapper service
    [HttpPost("scrape")]
    public async Task<ActionResult> ScrapeProduct(string item)
    {
        var scrapedJson = await _scraperService.ScrapeData(item);
        return Ok(scrapedJson);
    }

}


