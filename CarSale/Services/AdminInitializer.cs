using CarSale.Models;
using Microsoft.AspNetCore.Identity;

namespace CarSale.Services;

public class AdminInitializer
{
    public static async Task SeedAdminUser(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
        string adminEmail = "nice.mozg@gmail.com";
        string adminUserName = "7064441111";
        string password = "Legioner012788796";
        string adminPhoneNumber = "77064441111";
        bool confirmedPhoneNumber = true;
        bool confirmedEmail = true;

        var roles = new[] { "superAdmin", "admin", "user", "block" };
        foreach (var role in roles)
        {
            if (await roleManager.FindByNameAsync(role) is null)
                await roleManager.CreateAsync(new IdentityRole(role));
        }

        if (await userManager.FindByNameAsync(adminUserName) is null)
        {
            User superAdmin = new User { Email = adminEmail, UserName = adminUserName,
                PhoneNumberConfirmed = confirmedPhoneNumber,
                EmailConfirmed = confirmedEmail
            };
            IdentityResult result = await userManager.CreateAsync(superAdmin, password);
            if (result.Succeeded)
                await userManager.AddToRoleAsync(superAdmin, "superAdmin");
        }
    }
}