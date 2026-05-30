using Microsoft.EntityFrameworkCore;

namespace PriceNest.Api.Models;

public class User
{
    public int Id { get; set; }
    public string Login { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public List<WatchlistItem> WatchlistItems { get; set; } = [];
}