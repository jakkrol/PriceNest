using Microsoft.EntityFrameworkCore;
using PriceNest.Api.Data;
using PriceNest.Api.DTOs;
using PriceNest.Api.Models;

namespace PriceNest.Api.Services;

public class WatchListService
{
    private readonly AppDbContext _dbContext;
    private readonly ProductService _productService;
    public WatchListService(AppDbContext dbContext, ProductService productService)
    {
        _dbContext = dbContext;
        _productService = productService;
    }

    // Method to get the watch list for a specific user
    public async Task<List<WatchlistResponseDto>> GetWatchListAsync(int userId)
    {
        var items = await _dbContext.WatchlistItems
            .Include(wi => wi.Product)
            .ThenInclude(p => p.ProductOffers)
            .Where(wi => wi.UserId == userId)
            .ToListAsync();

        return items.Select(wi => new WatchlistResponseDto(
                wi.ProductId,
                wi.Product.Name,
                wi.TargetPrice,
                wi.PreferredStores,
                wi.Product.ProductOffers.Select(po => new ProductOfferDto(
                    po.StoreName,
                    po.Price,
                    po.Url,
                    po.LastUpdated
                )).ToList()
            )).ToList();
    }



    // TODO - COMPLETE WITH DTO'S

    // Method to get all users watching a specific product
    public async Task<List<WatchlistItem>> GetUsersWatchingProductAsync(int productId)
    {
        return await _dbContext.WatchlistItems
            .Include(wi => wi.User)
            .Where(wi => wi.ProductId == productId)
            .ToListAsync();
    }

    //Analyze this method to check if it needs transaction and such logic
    public async Task AddProductToWatchListAsync(int userId, string productName, string storeName, string currentUrl, decimal currentPrice, decimal targetPrice)
    {
        var existingProduct = await _productService.GetProductByNameAsync(productName);
        int productId;

        if (existingProduct == null)
        {
            var newProduct = new Product { Name = productName };

            try
            {
                await _productService.AddProductAsync(newProduct);
                productId = newProduct.Id;
            }
            catch (DbUpdateException)
            {
                var raceProduct = await _productService.GetProductByNameAsync(productName);
                productId = raceProduct!.Id;
            }
        }
        else
        {
            productId = existingProduct.Id;
        }
        await _productService.SaveOrUpdateOfferAsync(productId, storeName, currentUrl, currentPrice);

        await AddToWatchListAsync(userId, productId, targetPrice);
    }

    // Method to add or update a product in the user's watch list
    public async Task AddToWatchListAsync(int userId, int productId, decimal targetPrice)
    {
        var existingEntry = await _dbContext.WatchlistItems
            .FirstOrDefaultAsync(wi => wi.UserId == userId && wi.ProductId == productId);

        if (existingEntry != null)
        {
            existingEntry.TargetPrice = targetPrice;
        }
        else
        {
            var watchlistItem = new WatchlistItem
            {
                UserId = userId,
                ProductId = productId,
                TargetPrice = targetPrice,
                PreferredStores = ""
            };
            _dbContext.WatchlistItems.Add(watchlistItem);
        }


        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {

        }
    }



    // Method to remove a product from the user's watch list, if no other user is watching it, also remove the product from the database
    public async Task<bool> RemoveFromWatchListAsync(int userId, int productId)
    {
        var entry = await _dbContext.WatchlistItems
             .FirstOrDefaultAsync(wi => wi.UserId == userId && wi.ProductId == productId);

        if (entry == null) return false;

        _dbContext.WatchlistItems.Remove(entry);
        await _dbContext.SaveChangesAsync();

        var isAnyOtherUserWatching = await _dbContext.WatchlistItems.AnyAsync(wi => wi.ProductId == productId);
        if (!isAnyOtherUserWatching)
        {
            var product = await _dbContext.Products.FindAsync(productId);
            if (product != null)
            {
                _dbContext.Products.Remove(product);
                await _dbContext.SaveChangesAsync();
            }
        }

        return true;
    }


}