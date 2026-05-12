namespace PriceNest.Api.DTOs;

public record UserRegisterDto(
    string Login,
    string Password,
    string Email
);