using Microsoft.AspNetCore.Identity;

namespace Exercici5API.Tools
{
    public class Seeder
    {
        public static async Task CreateInitialRoles(IServiceProvider services)
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            string[] roles = { "Admin", "Sales", "Client" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
    }
}
