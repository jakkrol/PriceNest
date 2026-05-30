using PriceNest.Api.Data;
using PriceNest.Api.DTOs;
using Microsoft.EntityFrameworkCore;

namespace PriceNest.Api.Services;

public class UserService
{
    private readonly AppDbContext _dbContext;

    public UserService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<UserResponseDto?> GetUserByIdAsync(int userId)
    {
        var user = await _dbContext.Users.FindAsync(userId);
        if (user == null) return null;

        return new UserResponseDto(user.Id, user.Login, user.Email);
    }


    public async Task<UserResponseDto?> GetUserByLoginAsync(string login)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Login == login);
        if (user == null) return null;

        return new UserResponseDto(user.Id, user.Login, user.Email);
    }


    public async Task<bool> ChangePasswordAsync(int userId, string newPassword)
    {
        var user = await _dbContext.Users.FindAsync(userId);
        if (user == null) return false;

        user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
        await _dbContext.SaveChangesAsync();
        return true;
    }


    public async Task<bool> DeleteUserAsync(int userId)
    {
        var user = await _dbContext.Users.FindAsync(userId);
        if (user == null) return false;

        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync();
        return true;
    }

}