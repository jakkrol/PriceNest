namespace PriceNest.Api.Services;

using System.Net.Http;
using System.Net.Http.Json;
using PriceNest.Api.Data;
public class ScraperService
{
    private readonly HttpClient _httpClient;
    private readonly AppDbContext _dbContext;
    public ScraperService(HttpClient httpClient, AppDbContext dbContext)
    {
        _httpClient = httpClient;
        _dbContext = dbContext;
    }

    public async Task<string> ScrapeData(string item)
    {
        var data = new { item = item };
        var res = await _httpClient.PostAsJsonAsync("api/scrape", data);
        res.EnsureSuccessStatusCode();
        return await res.Content.ReadAsStringAsync();
    }



    //testing method to scrape all products in the database
    public async Task<string> ScrapeFromDb()
    {
        var items = _dbContext.Products.Select(p => p.Name).ToArray();
        Console.WriteLine($"Scraping {items} items from the database...");
        var res = await _httpClient.PostAsJsonAsync("api/scrape-watchlist", new { products = items });
        var data = await res.Content.ReadAsStringAsync();
        return data;
    }
}