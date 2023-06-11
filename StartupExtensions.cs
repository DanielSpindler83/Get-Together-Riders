using Get_Together_Riders.Middleware;
using Get_Together_Riders.Models.Database;
using Get_Together_Riders.Models.Interfaces;
using Get_Together_Riders.Models.Repositories;
using IdentityModel;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;

namespace Get_Together_Riders
{
    public static class StartupExtensions
    {

        public static IServiceCollection RegisterServices(this IServiceCollection services, string appSecret, WebApplicationBuilder builder )
        {
            //// add DI services collection
            services.AddScoped<IRiderRepository, RiderRepository>(); //our custom DI services
            services.AddScoped<IRideEventRepository, RideEventRepository>(); //our custom DI services
            services.AddScoped<IRideEventLogEntryRepository, RideEventLogEntryRepository>(); //our custom DI services

            // setup our DB context and read in the connection string from app settings
            services.AddDbContext<GTRDbContext>(options => {
                options.UseSqlServer(
                    builder.Configuration["ConnectionStrings:GTRDbContextConnection"]);
            });


            // Facebook login stuff - https://www.youtube.com/watch?v=xzcDoUPy8Mk
            services.AddAuthentication(options =>
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
                options.AppId = "1862186037494265";
                options.AppSecret = appSecret;
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
            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<IdentityRole>() // https://www.youtube.com/watch?v=Y6DCP-yH-9Q - setting up roles
                .AddEntityFrameworkStores<GTRDbContext>();

            //if (builder.Environment.IsDevelopment())
            //{
            //    services.AddDatabaseDeveloperPageExceptionFilter();

            //    services.AddHttpLogging(logging =>
            //    {
            //        // https://bit.ly/aspnetcore6-httplogging 
            //        logging.LoggingFields = HttpLoggingFields.All;
            //        logging.MediaTypeOptions.AddText("application/javascript");
            //        logging.RequestBodyLogLimit = 4096;
            //        logging.ResponseBodyLogLimit = 4096;
            //    });

            //    // Loggging learning about trace files - added to play with trace files and logging
            //    // trace listeners use debug providers(not log level) to listen for log entries and if configured as below, write to file.
            //    // example path = C:\Users\User\AppData\Local
            //    var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            //    var tracePath = Path.Join(path, $"Log_GTR_{DateTime.Now.ToString("yyyMMdd-HHmm")}.log");
            //    Trace.Listeners.Add(new TextWriterTraceListener(System.IO.File.CreateText(tracePath)));
            //    Trace.AutoFlush = true;
            //}

            // Add authorization services
            // services.AddAuthorization(); // Do I need this ?

            // allow use of Razor pages (as opposed to MVC controllers with views)
            services.AddRazorPages(); 

            return services;
        }

        public static WebApplication RegisterMiddleware(this WebApplication app)
        {
            // custom global exception handling - https://github.com/StefanTheCode/GlobalErrorHandling
            // app.UseMiddleware<ExceptionMiddleware>(); // this is for API's really - dont think ill use it. UseExceptionHandler is more useful.

            // pretty error pages are good hmmm K
            app.UseExceptionHandler("/Error");

            //if (app.Environment.IsDevelopment())
            //{
            //    // set log level in appsettings.json
            //    app.UseHttpLogging(); // lets us log incoming and outgoing requests as they traverse the request pipeline

            //    app.UseDeveloperExceptionPage(); // this is for dev only

            //}

            //app.UseHttpsRedirection(); i dont think we need this right now
            app.UseStaticFiles(); // add support for static files

            //app.UseRouting(); i dont think we need this right now - we have routing via MapRazorPages();

            app.UseAuthentication(); // need this for facebook and asp.net core identity
            app.UseAuthorization();

            //app.MapRazorPages(); // enable razor page routing - pages folder
            app.MapRazorPages().RequireAuthorization(); // trying out forcing authenticaiton and authorization

            return app;
        }
    }
}
