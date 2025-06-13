using Exercici5API.Models;
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
        public static async Task CreateInitialUsers(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<User>>();

            string password = "Recu2025!!";
            var admin = new User
            {
                Email = "admin@gmail.com",
                UserName = "admin@gmail.com"
            };

            var sales = new User
            {
                Email = "sales@gmail.com"
            };
            if (await userManager.FindByEmailAsync(admin.Email) == null)
            {
                var result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                }
                Console.WriteLine("User creat");
            }
            if (await userManager.FindByEmailAsync(sales.Email) == null)
            {
                var result = await userManager.CreateAsync(sales, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(sales, "Sales");
                }
                Console.WriteLine("User creat");
            }
        }
    }
}
