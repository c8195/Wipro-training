using DoConnect.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DoConnect.API.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(
            ApplicationDbContext context,
            UserManager<User> userManager,
            RoleManager<Role> roleManager)
        {
            // Ensure database is created
            await context.Database.EnsureCreatedAsync();

            // Check if we need to seed data
            if (await context.Users.AnyAsync())
            {
                return; // DB has been seeded
            }

            // Ensure roles exist
            var roles = new[] { "Admin", "User" };
            foreach (var roleName in roles)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new Role(roleName));
                }
            }

            // Create admin user
            var adminUser = new User
            {
                FirstName = "Admin",
                LastName = "User",
                UserName = "admin",
                Email = "admin@doconnect.com",
                EmailConfirmed = true,
                CreatedAt = DateTime.UtcNow
            };

            var result = await userManager.CreateAsync(adminUser, "Admin123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }

            // Create sample regular user
            var sampleUser = new User
            {
                FirstName = "John",
                LastName = "Doe",
                UserName = "johndoe",
                Email = "john@example.com",
                EmailConfirmed = true,
                CreatedAt = DateTime.UtcNow
            };

            result = await userManager.CreateAsync(sampleUser, "User123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(sampleUser, "User");
            }

            await context.SaveChangesAsync();
        }
    }
}