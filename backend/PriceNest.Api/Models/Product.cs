namespace PriceNest.Api.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Url { get; set; } = string.Empty;
    public DateTime LastUpdated { get; set; }
    public List<UserProduct> UserProducts { get; set; } = [];
}