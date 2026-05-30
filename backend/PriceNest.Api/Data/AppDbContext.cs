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

        modelBuilder.Entity<PriceHistory>()
            .HasOne(ph => ph.Product)
            .WithMany(p => p.PriceHistories)
            .HasForeignKey(ph => ph.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<WatchlistItem>()
            .HasOne(wi => wi.User)
            .WithMany(u => u.WatchlistItems)
            .HasForeignKey(wi => wi.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<WatchlistItem>()
            .HasIndex(wi => new { wi.UserId, wi.ProductId })
            .IsUnique();

        modelBuilder.Entity<WatchlistItem>()
            .HasOne(wi => wi.Product)
            .WithMany(p => p.WatchlistItems)
            .HasForeignKey(wi => wi.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        // modelBuilder.Entity<Product>()
        //     .HasMany(p => p.WatchlistItems)
        //     .WithOne(wi => wi.Product)
        //     .HasForeignKey(wi => wi.ProductId)
        //     .OnDelete(DeleteBehavior.Cascade);
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<PriceHistory> PriceHistories { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<WatchlistItem> WatchlistItems { get; set; }

}