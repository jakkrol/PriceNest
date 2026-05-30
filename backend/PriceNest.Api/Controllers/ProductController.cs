using Microsoft.AspNetCore.Mvc;
using PriceNest.Api.Models;
using PriceNest.Api.DTOs;
using Microsoft.EntityFrameworkCore;
using PriceNest.Api.Services;
using Microsoft.AspNetCore.Authorization;

namespace PriceNest.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
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

    [HttpGet("{id}/history")]
    public async Task<ActionResult<List<PriceHistory>>> GetPriceHistory(int id)
    {
        var history = await _productService.GetPriceHistoryAsync(id);
        if (history == null || history.Count == 0)
        {
            return NotFound();
        }
        return Ok(history);
    }

    //--- NOTE: There shouldn't be endpoint like this here, because products are added by adding url to watchlist, so it needs to be realised in WatchListController ----

    // [HttpPost]
    // public async Task<ActionResult> AddProduct(Product p)
    // {
    //     // To modify, maybe add DTO instead of pure object
    //     var result = await _productService.AddProductAsync(p);
    //     if (!result)
    //     {
    //         return BadRequest();
    //     }
    //     return Ok();
    // }



    [HttpPost("scrape")]
    public async Task<ActionResult> ScrapeProduct(string item)
    {
        var scrapedJson = await _scraperService.ScrapeData(item);
        return Ok(scrapedJson);
    }

}


