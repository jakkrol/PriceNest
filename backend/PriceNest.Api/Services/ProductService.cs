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

    // Getting a product by name
    public async Task<Product?> GetProductByNameAsync(string name)
    {
        return await _dbContext.Products.FirstOrDefaultAsync(p => p.Name == name);
    }

    // Adding a new product, returns false if product with same id already exists
    public async Task<bool> AddProductAsync(Product product)
    {
        var exists = await _dbContext.Products.AnyAsync(p => p.Name == product.Name);
        if (exists)
        {
            return false;
        }
        _dbContext.Products.Add(product);
        AddHistoryEntry(product.Id, product.Price);
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

        if (product.Price != updatedProduct.Price)
        {
            AddHistoryEntry(product.Id, updatedProduct.Price);
        }

        //product.Name = updatedProduct.Name;
        product.Price = updatedProduct.Price;
        product.Url = updatedProduct.Url;
        product.LastUpdated = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync();
        return true;
    }


    // Method to add a price history entry for a product
    public void AddHistoryEntry(int productId, decimal price)
    {
        var entry = new PriceHistory
        {
            ProductId = productId,
            Price = price,
            CheckedAt = DateTime.UtcNow
        };
        _dbContext.PriceHistories.Add(entry);
    }

    // Method to get price history for a product
    public async Task<List<PriceHistory>> GetPriceHistoryAsync(int productId)
    {
        return await _dbContext.PriceHistories
            .Where(ph => ph.ProductId == productId)
            .OrderByDescending(ph => ph.CheckedAt)
            .ToListAsync();
    }
}
