namespace PriceNest.Api.DTOs;

public record AddToWatchlistDto(
    string ProductName,
    string currentUrl,
    decimal currentPrice,
    decimal TargetPrice
);