using Microsoft.AspNetCore.Identity;

namespace CarpetCleaningSystem.Infrastructure.Persistence.Seed
{
    public static class IdentitySeed
    {
        public static async Task SeedAsync(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            // 1) Roles
            string[] roles = { "Admin", "Employee" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // 2) Admin user (DEV)
            var adminEmail = "admin@carpetcleaning.local";
            var adminPassword = "Admin123!";

            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }

            var employeeEmail = "employee@carpetcleaning.local";
            var employeePassword = "Employee123!";

            var employeeUser = await userManager.FindByEmailAsync(employeeEmail);

            if (employeeUser == null)
            {
                employeeUser = new IdentityUser
                {
                    UserName = employeeEmail,
                    Email = employeeEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(employeeUser, employeePassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(employeeUser, "Employee");
                }
            }

        }
    }
}

