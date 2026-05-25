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
    private readonly ScraperService _scraperService;
    private readonly ProductService _productService;
    public ProductController(ScraperService scraperService, ProductService productService)
    {
        _scraperService = scraperService;
        _productService = productService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await _productService.GetProductByIdAsync(id);
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
        var result = await _productService.AddProductAsync(p);
        if (!result)
        {
            return BadRequest();
        }
        return Ok();
    }



    [HttpPost("scrape")]
    public async Task<ActionResult> ScrapeProduct(string item)
    {
        var scrapedJson = await _scraperService.ScrapeData(item);
        return Ok(scrapedJson);
    }

}


