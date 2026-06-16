namespace PriceNest.Api.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public List<ProductOffer> ProductOffers { get; set; } = [];
    public List<WatchlistItem> WatchlistItems { get; set; } = [];
}