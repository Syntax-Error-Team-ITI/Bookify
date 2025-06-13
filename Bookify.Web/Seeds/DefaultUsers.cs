namespace Bookify.Web.Seeds
{
    public static class DefaultUsers
    {
        public static async Task SeedAdminUserAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Ensure the Admin role exists
            if (!await roleManager.RoleExistsAsync(AppRoles.Admin))
            {
                await roleManager.CreateAsync(new IdentityRole(AppRoles.Admin));
            }
            
            // Create new admin user
            var admin = new ApplicationUser
            {
                UserName = "admin",
                Email = "admin@bookify.com",
                FullName = "Admin",
                EmailConfirmed = true
            };

            // Check if admin user already exists
            var adminUser = await userManager.FindByEmailAsync(admin.Email);
            if (adminUser != null)
                return;


            var result = await userManager.CreateAsync(admin, "P@ssword123");

            if (!result.Succeeded)
            {
                throw new Exception($"Admin user creation failed: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }

            // Add to Admin role
            result = await userManager.AddToRoleAsync(admin, AppRoles.Admin);

            if (!result.Succeeded)
            {
                throw new Exception($"Adding admin to role failed: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
        }
    }
}
