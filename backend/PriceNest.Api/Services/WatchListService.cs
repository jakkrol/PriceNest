using Microsoft.EntityFrameworkCore;
using PriceNest.Api.Data;
using PriceNest.Api.Models;

namespace PriceNest.Api.Services;

public class WatchListService
{
    private readonly AppDbContext _dbContext;

    public WatchListService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    // Method to get the watch list for a specific user
    public async Task<List<UserProduct>> GetWatchListAsync(int userId)
    {
        return await _dbContext.UserProducts
            .Include(up => up.Product)
            .Where(up => up.UserId == userId)
            .ToListAsync();
    }

    // Method to get all users watching a specific product
    public async Task<List<UserProduct>> GetUsersWatchingProductAsync(int productId)
    {
        return await _dbContext.UserProducts
            .Include(up => up.User)
            .Where(up => up.ProductId == productId)
            .ToListAsync();
    }

    // Method to add or update a product in the user's watch list
    public async Task AddToWatchListAsync(int userId, int productId, decimal targetPrice)
    {
        var existingEntry = await _dbContext.UserProducts
            .FirstOrDefaultAsync(up => up.UserId == userId && up.ProductId == productId);

        if (existingEntry != null)
        {
            existingEntry.TargetPrice = targetPrice;
        }
        else
        {
            var userProduct = new UserProduct
            {
                UserId = userId,
                ProductId = productId,
                TargetPrice = targetPrice
            };
            _dbContext.UserProducts.Add(userProduct);
        }

        await _dbContext.SaveChangesAsync();
    }

    // Method to remove a product from the user's watch list
    // TODO: Add functionality to remove entry about product if no one is watching it anymore
    public async Task RemoveFromWatchListAsync(int userId, int productId)
    {
        var entry = await _dbContext.UserProducts
            .FirstOrDefaultAsync(up => up.UserId == userId && up.ProductId == productId);

        if (entry != null)
        {
            _dbContext.UserProducts.Remove(entry);
            await _dbContext.SaveChangesAsync();
        }
    }
}