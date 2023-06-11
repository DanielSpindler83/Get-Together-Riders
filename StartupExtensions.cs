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


            services.AddAuthentication().AddMicrosoftAccount(options =>
            {
                options.ClientId = "df01c830-2308-4412-a3e2-e873dafa3981";
                options.ClientSecret = appSecret;
            });

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

            app.UseRouting(); // not sure if needed or not

            app.UseAuthentication(); // need this for facebook and asp.net core identity
            app.UseAuthorization();

            //app.MapRazorPages(); // enable razor page routing - pages folder
            app.MapRazorPages().RequireAuthorization(); // trying out forcing authenticaiton and authorization

            return app;
        }
    }
}
