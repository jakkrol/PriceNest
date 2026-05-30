using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PriceNest.Api.Data;
using PriceNest.Api.DTOs;
using PriceNest.Api.Models;
using PriceNest.Api.Services;

namespace PriceNest.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class WatchlistController : ControllerBase
{
    private readonly WatchListService _watchListService;
    private readonly ScraperService _scraperService;

    public WatchlistController(WatchListService watchListService, ScraperService scraperService)
    {
        _watchListService = watchListService;
        _scraperService = scraperService;
    }


    // REMEMBER: Need to test if getting user by claims works fine
    [HttpPost]
    public async Task<ActionResult> AddToWatchList([FromBody] AddToWatchlistDto dto)
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
        if (userIdClaim == null)
        {
            return Unauthorized(new { message = "User ID claim is missing." });
        }

        int userId = int.Parse(userIdClaim.Value);

        await _watchListService.AddProductToWatchListAsync(userId, dto.ProductName, dto.currentUrl, dto.currentPrice, dto.TargetPrice);
        return Ok(new { message = "Product added to watchlist." });
    }




    // To complete those methods, also need to get user id from claims
    [HttpGet]
    public async Task<ActionResult<List<WatchlistItem>>> GetWatchlist()
    {
        return Ok(await _watchListService.GetWatchListAsync(1)); // To modify, get user id from claims
    }

    [HttpDelete("{productId}")]
    public async Task<ActionResult> RemoveFromWatchlist(int productId)
    {
        return Ok(); // To modify, implement method in service and call it here, also get user id from claims
    }

}