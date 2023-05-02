namespace Get_Together_Riders
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("GTRDbContextConnection") ?? throw new InvalidOperationException("Connection string 'BethanysPieShopDbContextConnection' not found.");

            // Add services to the container.
            builder.Services.AddRazorPages(); // allow use of Razor pages (as opposed to MVC controllers with views)

            var app = builder.Build(); // webapp instance is built(instance is ready to be instantiated)

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage(); // this is for dev only
            }

            //app.UseHttpsRedirection(); i dont think we need this right now
            app.UseStaticFiles(); // add support for static files

            //app.UseRouting(); i dont think we need this right now - we have routing via MapRazorPages();

            //app.UseAuthorization(); i dont think we need this right now

            app.MapRazorPages(); // enable razor page routing - pages folder

            app.Run();
        }
    }
}