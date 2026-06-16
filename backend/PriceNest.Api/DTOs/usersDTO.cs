namespace PriceNest.Api.DTOs;

public record UserResponseDto(
    int Id,
    string Login, 
    string Email
);

public record UserRegisterDto(
    string Login,
    string Password,
    string Email
);

public record UserLoginDto(
    string Login, 
    string Password
);