using Microsoft.AspNetCore.SignalR;

namespace PriceNest.Api.Models;

public class PriceHistory
{
    public int Id { get; set; }

    public int ProductOfferId { get; set; }
    public ProductOffer ProductOffer { get; set; } = null!;

    public decimal Price { get; set; }
    public DateTime CheckedAt { get; set; }
}