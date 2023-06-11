using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Get_Together_Riders.Models.Database;
using System.Diagnostics;

namespace Get_Together_Riders
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            string MicrosoftLoginAppSecret = Environment.GetEnvironmentVariable("GTRMicrosoftLoginAppSecret") ?? throw new InvalidOperationException("Environment variable 'GTRMicrosoftLoginAppSecret' not found.");
            _ = builder.Configuration.GetConnectionString("GTRDbContextConnection") ?? throw new InvalidOperationException("Connection string 'GTRDbContextConnection' not found.");

            builder.Services.RegisterServices(MicrosoftLoginAppSecret, builder);

            var app = builder.Build();

            app.RegisterMiddleware();

            // run our DB seed - will only populate data if none exists in the DB
            DbInitializer.Seed(app);

            //// - Setting up Roles
            // https://www.youtube.com/watch?v=Y6DCP-yH-9Q - setting up roles
            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var roles = new[] { "Admin", "Rider" };
                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                        await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // assign the new admin role to my account
            using (var scope = app.Services.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                string email = "danielspindler83@gmail.com"; 

                if(await userManager.FindByEmailAsync(email) != null)
                {
                    var user = await userManager.FindByEmailAsync(email);
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }

            // [Authorize(Roles = "Admin")] - block we use for restricting to a role
            // @if (User.IsInRole("Admin")) {} - use in a razor page
            ////

            app.Run();
        }
    }
}