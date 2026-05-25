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

    // Getting a product by id
    public async Task<Product?> GetProductByIdAsync(int id)
    {
        return await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == id);
    }

    // Adding a new product, returns false if product with same id already exists
    public async Task<bool> AddProductAsync(Product product)
    {
        var exists = await _dbContext.Products.AnyAsync(p => p.Url == product.Url);
        if (exists)
        {
            return false;
        }
        _dbContext.Products.Add(product);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    // Getting all products
    public async Task<List<Product>> GetAllProductsAsync()
    {
        return await _dbContext.Products.ToListAsync();
    }

    // Deleting a product by id
    public async Task<bool> DeleteProductAsync(int id)
    {
        var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == id);
        if (product == null)
        {
            return false;
        }
        _dbContext.Products.Remove(product);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    // Updating a product, returns false if product doesn't exist
    public async Task<bool> UpdateProductAsync(Product updatedProduct)
    {
        var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == updatedProduct.Id);
        if (product == null)
        {
            return false;
        }
        product.Name = updatedProduct.Name;
        product.Price = updatedProduct.Price;
        product.Url = updatedProduct.Url;
        product.LastUpdated = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync();
        return true;
    }
}
