using Get_Together_Riders.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using System.Security.Claims;
using IdentityModel;

namespace Get_Together_Riders
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // read in from local secrets.json - this file is NOT checked into GIT
            string AppSecret = builder.Configuration["AppSecret"];

            // read in our DB Connection string from appsettings.json
            var connectionString = builder.Configuration.GetConnectionString("GTRDbContextConnection") ?? throw new InvalidOperationException("Connection string 'GTRDbContextConnection' not found.");

            //// add DI services collection
            builder.Services.AddScoped<IRiderRepository, RiderRepository>(); //our custom DI services

            // setup our DB context and read in the connection string from app settings
            builder.Services.AddDbContext<GTRDbContext>(options => {
                options.UseSqlServer(
                    builder.Configuration["ConnectionStrings:GTRDbContextConnection"]);
            });


            // Facebook login stuff - https://www.youtube.com/watch?v=xzcDoUPy8Mk
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                options.LoginPath = "/account/facebook-login";
            })
            .AddFacebook(options =>
            {
                // https://www.thecodehubs.com/how-to-login-with-facebook-in-asp-net-core-identity/
                options.AppId = "3731952183691780";
                options.AppSecret = AppSecret;
                options.CallbackPath = "/signin-facebook"; // this is the default and is the Valid OAuth Redirect URIs set in Facebook App (Facebook Login --> Settings)
                options.Scope.Add("email");
                options.Scope.Add("public_profile");
                options.Fields.Add("name");
                options.Fields.Add("email");
                //options.AuthorizationEndpoint += "&prompt=login"; // force login even if user is logged in

                // https://stackoverflow.com/questions/45855660/how-to-retrieve-facebook-profile-picture-from-logged-in-user-with-asp-net-core-i
                options.Fields.Add("picture");
                options.Events = new OAuthEvents
                {
                    OnCreatingTicket = context =>
                    {
                        var identity = (ClaimsIdentity)context.Principal.Identity;
                        var profileImg = context.User.GetProperty("picture").GetProperty("data").GetProperty("url").ToString();
                        identity.AddClaim(new Claim(JwtClaimTypes.Picture, profileImg));
                        return Task.CompletedTask;
                    }
                };
            });

            ////
            
            // Order of this is IMPORTANT - add auth before adding the DefaultIdentity
            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<GTRDbContext>();

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            // Add services to the container.
            builder.Services.AddRazorPages(); // allow use of Razor pages (as opposed to MVC controllers with views)

            var app = builder.Build(); // webapp instance is built(instance is ready to be instantiated)

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage(); // this is for dev only

                // set log level in appsettings.json
                app.UseHttpLogging(); // lets us log incoming and outgoing requests as they traverse the request pipeline
            }

            //app.UseHttpsRedirection(); i dont think we need this right now
            app.UseStaticFiles(); // add support for static files

            //app.UseRouting(); i dont think we need this right now - we have routing via MapRazorPages();

            app.UseAuthentication(); // need this for facebook and asp.net core identity
            app.UseAuthorization(); 

            app.MapRazorPages(); // enable razor page routing - pages folder

            // run our DB seed - will only populate data if none exists in the DB
            DbInitializer.Seed(app);

            app.Run();
        }
    }
}