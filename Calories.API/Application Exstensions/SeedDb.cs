using Calories.Entities.Models;
using Microsoft.AspNetCore.Identity;

namespace Calories.API.Application_Exstensions
{
    public static class SeedDb
    {
        public static async Task SeedRolesAndUsersAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

            string[] roles = { "User", "User Manager", "Admin" };

            // Creating roles
            foreach(var role in roles)
            {
                if(!await roleManager.RoleExistsAsync(role))
                {
                    var result = await roleManager.CreateAsync(new IdentityRole(role));

                    if (result.Succeeded)
                    {
                        logger.LogInformation($"Role {role} created successfully.");
                    }else
                    {
                        logger.LogError($"Failed to create {role} role.");
                    }
                }

                // Creating test users
                var userEmail = $"{role.ToLower().Replace(" ", "")}@test.com";
                var user = await userManager.FindByEmailAsync(userEmail);

                if(user == null) 
                {
                    user = new User { UserName = $"{role.ToLower().Replace(" ", "")}", Email = userEmail, ExpectedCaloriesPerDay = 2000 };
                    var result = await userManager.CreateAsync(user, "Pa$$w0rd");

                    if (!result.Succeeded)
                    {
                        logger.LogError($"Failed to create user {userEmail}.");
                    }
                    else
                    {
                        var addToRoleResult = await userManager.AddToRoleAsync(user, role);

                        if (!addToRoleResult.Succeeded)
                        {
                            logger.LogError($"Failed to add user {user} to role {role}.");
                        }
                    }
                }
                
            }

        }
    }
}
