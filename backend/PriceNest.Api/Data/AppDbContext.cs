using Microsoft.EntityFrameworkCore;
using PriceNest.Api.Models;

namespace PriceNest.Api.Data;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }

    public DbSet<Product> Products {get; set;}
}