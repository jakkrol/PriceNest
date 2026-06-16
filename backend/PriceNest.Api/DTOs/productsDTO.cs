namespace PriceNest.Api.DTOs;

public record ProductRequestDTO(
    int Id
);

public record ProductOfferDto(
    string StoreName,
    decimal Price,
    string Url,
    DateTime LastUpdated
);

public record ProductResponseDto(
    int Id,
    string Name,
    List<ProductOfferDto> Offers
);