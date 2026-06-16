using Microsoft.EntityFrameworkCore;
using PriceNest.Api.Data;
using PriceNest.Api.DTOs;
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
    public async Task<ProductResponseDto?> GetProductByIdWithOffersAsync(int id)
    {
        var product = await _dbContext.Products.Include(p => p.ProductOffers).FirstOrDefaultAsync(p => p.Id == id);
        if(product == null)
        {
            return null;
        }

        return new ProductResponseDto(
        product.Id,
        product.Name,
        product.ProductOffers.Select(po => new ProductOfferDto(
            po.StoreName,
            po.Price,
            po.Url,
            po.LastUpdated
        )).ToList()
    );
    }


    //TODO - COMPLETE THOSE METHODS WITH DTO'S

    // Getting a product by name
    public async Task<Product?> GetProductByNameAsync(string name)
    {
        return await _dbContext.Products.FirstOrDefaultAsync(p => p.Name == name);
    }

    // Getting all products
    public async Task<List<Product>> GetAllProductsAsync()
    {
        return await _dbContext.Products.ToListAsync();
    }

    // Adding a new product, returns false if product with same id already exists
    public async Task<bool> AddProductAsync(Product product)
    {
        _dbContext.Products.Add(product);
        await _dbContext.SaveChangesAsync();
        return true;
        // var exists = await _dbContext.Products.AnyAsync(p => p.Name == product.Name);
        // if (exists)
        // {
        //     return false;
        // }
        // _dbContext.Products.Add(product);

        // //await _dbContext.SaveChangesAsync(); 


        // AddHistoryEntry(product, product.Price);
        
   
        // await _dbContext.SaveChangesAsync();
        // return true;
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

    // // Updating a product, returns false if product doesn't exist
    // public async Task<bool> UpdateProductAsync(Product updatedProduct)
    // {
    //     var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == updatedProduct.Id);
    //     if (product == null)
    //     {
    //         return false;
    //     }

    //     if (product.Price != updatedProduct.Price)
    //     {
    //         AddHistoryEntry(product, updatedProduct.Price);
    //     }

    //     //product.Name = updatedProduct.Name;
    //     product.Price = updatedProduct.Price;
    //     product.Url = updatedProduct.Url;
    //     product.LastUpdated = DateTime.UtcNow;
    //     await _dbContext.SaveChangesAsync();
    //     return true;
    // }dsa

public async Task SaveOrUpdateOfferAsync(int productId, string storeName, string url, decimal price)
    {
        var existingOffer = await _dbContext.ProductOffers
            .FirstOrDefaultAsync(po => po.ProductId == productId && po.StoreName == storeName);

        if (existingOffer != null)
        {
            if (existingOffer.Price != price || existingOffer.Url != url)
            {
                AddHistoryEntry(existingOffer.Id, existingOffer.Price);
                
                existingOffer.Price = price;
                existingOffer.Url = url;
                existingOffer.LastUpdated = DateTime.UtcNow;
            }
        }
        else
        {
            var newOffer = new ProductOffer
            {
                ProductId = productId,
                StoreName = storeName,
                Url = url,
                Price = price,
                LastUpdated = DateTime.UtcNow
            };
            _dbContext.ProductOffers.Add(newOffer);
        }

        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            // Zabezpieczenie anty-wyścigowe: jeśli scraper puścił dwa zapytania dla tego samego sklepu naraz,
            // indeks UNIQUE na ProductId + StoreName rzuci błąd. Łapiemy go i ignorujemy duplikat.
        }
    }

    // Method to add a price history entry for a product
    public void AddHistoryEntry(int productOfferId, decimal price)
    {
        var entry = new PriceHistory
        {
            ProductOfferId = productOfferId,
            Price = price,
            CheckedAt = DateTime.UtcNow
        };
        _dbContext.PriceHistories.Add(entry);
    }

    // Method to get price history for a product
    public async Task<List<PriceHistory>> GetPriceHistoryByProductAsync(int productId)
    {
        return await _dbContext.PriceHistories
            .Include(ph => ph.ProductOffer)
            .Where(ph => ph.ProductOffer.ProductId == productId)
            .OrderByDescending(ph => ph.CheckedAt)
            .ToListAsync();
    }
}
