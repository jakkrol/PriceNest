using Microsoft.EntityFrameworkCore;
using PriceNest.Api.Models;

namespace PriceNest.Api.Data;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Login)
            .IsUnique();
    }

    public DbSet<Product> Products {get; set;}
    public DbSet<PriceHistory> PriceHistories {get; set;}
    public DbSet<User> Users {get; set;}
    public DbSet<UserProduct> UserProducts {get; set;}
    
}