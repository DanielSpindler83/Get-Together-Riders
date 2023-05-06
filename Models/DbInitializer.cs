using Get_Together_Riders.Models.Enums;
using Microsoft.AspNetCore.Builder;

namespace Get_Together_Riders.Models
{
    public static class DbInitializer
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            GTRDbContext context = applicationBuilder.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<GTRDbContext>();


        }
    }
}