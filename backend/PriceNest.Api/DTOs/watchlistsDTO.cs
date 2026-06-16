namespace PriceNest.Api.DTOs;

public record AddToWatchlistDto(
    string ProductName,
    string StoreName,
    string currentUrl,
    decimal currentPrice,
    decimal TargetPrice
);


public record WatchlistResponseDto(
    int ProductId,
    string ProductName,
    decimal TargetPrice,
    string PreferredStores,
    List<ProductOfferDto> Offers 
);