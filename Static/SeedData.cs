using E_Commerce_API.Models;
using E_Commerce_API.Permissions;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace E_Commerce_API.Static
{
    public class SeedData
    {

        public static async Task CreateAdmin(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

            string adminEmail = "admin@example.com";
            string adminPassword = "Admin123!";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                var user = new User
                {
                    UserName = "Admin",
                    Email = adminEmail,
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(user, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }
        }


        public static async Task CreateRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string[] roleNames = { "Admin", "User" };

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                    await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }



        public static async Task SeedPermissions(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var adminRole = await roleManager.FindByNameAsync("Admin");
            if (adminRole != null)
            {
                // قائمة الصلاحيات التي نريد إضافتها
                var permissions = new List<string> { Permission.Project.Create, Permission.Project.Delete };

                // جلب الصلاحيات الموجودة حالياً لتجنب التكرار
                var currentClaims = await roleManager.GetClaimsAsync(adminRole);

                foreach (var permission in permissions)
                {
                    if (!currentClaims.Any(c => c.Type == "permission" && c.Value == permission))
                    {
                        await roleManager.AddClaimAsync(adminRole, new Claim("permission", permission));
                    }
                }
            }
        }

    }
}
