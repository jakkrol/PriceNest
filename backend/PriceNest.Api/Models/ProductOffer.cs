namespace PriceNest.Api.Models;

public class ProductOffer
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public string StoreName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Url { get; set; } = string.Empty;
    public DateTime LastUpdated { get; set; }
    public List<PriceHistory> PriceHistories { get; set; } = [];
}