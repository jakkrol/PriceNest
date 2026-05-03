namespace PriceNest.Api.Models;

public class UserProduct
{
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    
    public decimal TargetPrice { get; set; } 
}