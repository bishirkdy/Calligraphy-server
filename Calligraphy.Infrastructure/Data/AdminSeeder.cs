using Calligraphy.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Calligraphy.Infrastructure.Data;

public static class AdminSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        var adminExists = await context.Users
            .AnyAsync(u => u.Role == "Admin");

        if (adminExists)
        {
            return;
        }

        var admin = new User
        {
            Name = "Admin",
            Email = "admin@calligraphy.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("REMOVED_PASSWORD"),
            Role = "Admin"
        };

        context.Users.Add(admin);

        await context.SaveChangesAsync();
    }
}