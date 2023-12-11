using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

/// <summary> 
/// We use this class to ALWAYS get same data for our DB. If something goes wrong we need to drop (dotnet ef database drop),
/// and this will handle itself.
/// </summary>
public class Seed
{
    public static async Task SeedUsers(DataContext ctx)
    {
        if (await ctx.Users.AnyAsync()) return;

        var userData = await File.ReadAllTextAsync("Data/UserSeedData.json");
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var users = JsonSerializer.Deserialize<List<AppUser>>(userData, options);

        foreach (var user in users)
        {
            using var hmac = new HMACSHA512();

            user.UserName = user.UserName.ToLower();
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$w0rd"));
            user.PasswordSalt = hmac.Key;

            ctx.Users.Add(user);
        }

        await ctx.SaveChangesAsync();
    }
}
