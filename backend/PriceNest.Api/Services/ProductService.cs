using Microsoft.EntityFrameworkCore;
using PriceNest.Api.Data;
using PriceNest.Api.Models;

namespace PriceNest.Api.Services;

public class ProductService
{
    private readonly AppDbContext _dbContext;

    public ProductService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        return await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task AddProductAsync(Product p)
    {
        _dbContext.Products.Add(p);
        await _dbContext.SaveChangesAsync();
    }
}
