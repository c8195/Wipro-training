using Microsoft.AspNetCore.Identity;
using DoConnect.API.Models;
using DoConnect.API.Data;

namespace DoConnect.API.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var services = scope.ServiceProvider;

            try
            {
                var userManager = services.GetRequiredService<UserManager<User>>();
                var roleManager = services.GetRequiredService<RoleManager<Role>>();
                var context = services.GetRequiredService<ApplicationDbContext>();

                // Ensure roles exist
                foreach (var roleName in new[] { "Admin", "User" })
                {
                    if (!await roleManager.RoleExistsAsync(roleName))
                    {
                        await roleManager.CreateAsync(new Role { Name = roleName });
                    }
                }

                // Create admin user
                var adminEmail = "admin@doconnect.com";
                var adminUser = await userManager.FindByEmailAsync(adminEmail);

                if (adminUser == null)
                {
                    adminUser = new User
                    {
                        UserName = adminEmail,
                        Email = adminEmail,
                        FirstName = "Admin",
                        LastName = "User",
                        EmailConfirmed = true,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };

                    var result = await userManager.CreateAsync(adminUser, "Admin@123");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(adminUser, "Admin");
                        
                        // Create user profile
                        var profile = new UserProfile
                        {
                            UserId = adminUser.Id,
                            JoinedAt = DateTime.UtcNow
                        };
                        context.UserProfiles.Add(profile);
                        await context.SaveChangesAsync();
                    }
                }

                // Create test user
                var userEmail = "user@doconnect.com";
                var testUser = await userManager.FindByEmailAsync(userEmail);

                if (testUser == null)
                {
                    testUser = new User
                    {
                        UserName = userEmail,
                        Email = userEmail,
                        FirstName = "Test",
                        LastName = "User",
                        EmailConfirmed = true,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };

                    var result = await userManager.CreateAsync(testUser, "User@123");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(testUser, "User");
                        
                        // Create user profile
                        var profile = new UserProfile
                        {
                            UserId = testUser.Id,
                            JoinedAt = DateTime.UtcNow
                        };
                        context.UserProfiles.Add(profile);
                        await context.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred while seeding the database");
            }
        }
    }
}
