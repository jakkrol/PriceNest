using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PriceNest.Api.Data;
using PriceNest.Api.DTOs;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using PriceNest.Api.Models;

namespace PriceNest.Api.Services;

public class AuthService
{
    public AppDbContext _dbContext;
    private readonly IConfiguration _configuration;
    public AuthService(AppDbContext dbContext, IConfiguration configuration)
    {
        _dbContext = dbContext;
        _configuration = configuration;
    }

    public async Task<(string AccessToken, string RefreshToken)?> LoginUserAsync(UserLoginDto user)
    {
        var existingUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Login == user.Login);

        if (existingUser == null || !BCrypt.Net.BCrypt.Verify(user.Password, existingUser.Password))
        {
            return null;
        }

        ////////////////////////////
        // var jwtSettings = _configuration.GetSection("JwtSettings");
        // var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));
        // var claims = new[]
        // {
        //     new Claim("UserId", existingUser.Id.ToString()),
        //     new Claim(JwtRegisteredClaimNames.Sub, user.Login),
        //     new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        // };

        // var token = new JwtSecurityToken(
        //     issuer: jwtSettings["Issuer"],
        //     audience: jwtSettings["Audience"],
        //     claims: claims,
        //     expires: DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["ExpiryMinutes"]!)),
        //     signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        // );
        // var accessTokenString = new JwtSecurityTokenHandler().WriteToken(token);
        /////////////////////////

        var accessTokenString = GenerateAccessToken(existingUser);
        var refreshTokenString = Guid.NewGuid().ToString();
        //zapisz refresh token w db
        existingUser.RefreshToken = refreshTokenString;
        existingUser.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        await _dbContext.SaveChangesAsync();
        ///
        
        return (accessTokenString, refreshTokenString);
    }


    public async Task<bool> RegisterUserAsync(UserRegisterDto user)
    {
        var exists = await _dbContext.Users.AnyAsync(u => u.Login == user.Login);
        if (exists)
        {
            return false;
        }

        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);
        var newUser = new User
        {
            Login = user.Login,
            Password = hashedPassword,
            Email = user.Email
        };

        _dbContext.Users.Add(newUser);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<(string AccessToken, string? RefreshToken)?> RefreshTokensAsync(string currentRefreshToken)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.RefreshToken == currentRefreshToken);
        if(user == null || !user.RefreshTokenExpiryTime.HasValue || user.RefreshTokenExpiryTime.Value <= DateTime.UtcNow)
        {
            return null;
        }

        var accessToken = GenerateAccessToken(user);

        string? newRefreshToken = null;
        var remainingTime = user.RefreshTokenExpiryTime.Value - DateTime.UtcNow;

        if (remainingTime.TotalDays < 2)
        {
            newRefreshToken = Guid.NewGuid().ToString();
            
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            await _dbContext.SaveChangesAsync(); 
        }
        return (accessToken, newRefreshToken);
    }



    public string GenerateAccessToken(User user)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));
        var claims = new[]
        {
            new Claim("UserId", user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Sub, user.Login),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["ExpiryMinutes"]!)),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );
        var accessTokenString = new JwtSecurityTokenHandler().WriteToken(token);
        return accessTokenString;
    }
}