using CleanArchitectureBlog.Models;
using Microsoft.AspNetCore.Identity;

namespace CleanArchitectureBlog.Contexts.Seed
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            string[] roles = new string[] { "Admin", "NormalUser" };

            foreach (string role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
            var adminUser = new ApplicationUser
            {
                UserName = "ynsemrecoskun",
                Email = "emrecoskun@example.com",
                EmailConfirmed = true,
                FirstName = "Emre",
                LastName = "Coskun"
            };

            string adminPassword = "ynsemre1020";
            var user = await userManager.FindByEmailAsync(adminUser.Email);

            if (user == null)
            {
                var createAdmin = await userManager.CreateAsync(adminUser, adminPassword);
                if (createAdmin.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }
    }

}
