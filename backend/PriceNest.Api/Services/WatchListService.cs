using Microsoft.EntityFrameworkCore;
using PriceNest.Api.Data;
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
    public async Task<List<WatchlistItem>> GetWatchListAsync(int userId)
    {
        return await _dbContext.WatchlistItems
            .Include(wi => wi.Product)
            .Where(wi => wi.UserId == userId)
            .ToListAsync();
    }

    // Method to get all users watching a specific product
    public async Task<List<WatchlistItem>> GetUsersWatchingProductAsync(int productId)
    {
        return await _dbContext.WatchlistItems
            .Include(wi => wi.User)
            .Where(wi => wi.ProductId == productId)
            .ToListAsync();
    }

    public async Task AddProductToWatchListAsync(int userId, string productName, string currentUrl, decimal currentPrice, decimal targetPrice)
    {
        var existingEntry = await _productService.GetProductByNameAsync(productName);
        int productId;
        if (existingEntry == null)
        {
            var newProduct = new Product
            {
                Name = productName,
                Url = currentUrl,
                Price = currentPrice,
                LastUpdated = DateTime.UtcNow
            };

            await _productService.AddProductAsync(newProduct);
            productId = newProduct.Id;
        }
        else
        {
            productId = existingEntry.Id;

            if (currentPrice < existingEntry.Price)
            {
                existingEntry.Price = currentPrice;
                existingEntry.Url = currentUrl;
                await _productService.UpdateProductAsync(existingEntry);
            }
        }

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
                TargetPrice = targetPrice
            };
            _dbContext.WatchlistItems.Add(watchlistItem);
        }

        await _dbContext.SaveChangesAsync();
    }

    // Method to remove a product from the user's watch list, if no other user is watching it, also remove the product from the database
    public async Task RemoveFromWatchListAsync(int userId, int productId)
    {
        var entry = await _dbContext.WatchlistItems
            .FirstOrDefaultAsync(wi => wi.UserId == userId && wi.ProductId == productId);


        if (entry != null)
        {
            _dbContext.WatchlistItems.Remove(entry);
            await _dbContext.SaveChangesAsync();

            var isProductWatched = await _dbContext.WatchlistItems.AnyAsync(wi => wi.ProductId == productId);
            if (isProductWatched == false)
            {
                var product = await _dbContext.Products.FindAsync(productId);
                if (product != null)
                {
                    _dbContext.Products.Remove(product);
                    await _dbContext.SaveChangesAsync();
                }
            }
        }


    }




}