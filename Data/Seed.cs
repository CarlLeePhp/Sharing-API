using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Sharing_API.Models;
using Microsoft.AspNetCore.Identity;

namespace Sharing_API.Data
{
    public class Seed
    {
        public static async Task SeedUsers(UserManager<AppUser> userManager)
        {
            Console.WriteLine("Seed Users:");
            if (await userManager.Users.AnyAsync()) return;
            
            
            var userData = await System.IO.File.ReadAllTextAsync("Data/UserSeedData.json");
            var users = JsonSerializer.Deserialize<List<AppUser>>(userData);
            if (users == null) return;
            
            foreach (var user in users)
            {
                user.Email = user.Email.ToLower();
                await userManager.CreateAsync(user, "Pa$$w0rd");
            }
        }
        public static async Task SeedCategories(DataContext context)
        {
            if (await context.Categories.AnyAsync()) return;
            var categoryData = await System.IO.File.ReadAllTextAsync("Data/CategorySeedData.json");
            var categories = JsonSerializer.Deserialize<List<Category>>(categoryData);
            if (categories == null) return;

            foreach (var category in categories)
            {
                context.Categories.Add(category);
            }
            await context.SaveChangesAsync();
        }
    }
}
