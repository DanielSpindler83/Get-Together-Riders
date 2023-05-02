using Microsoft.EntityFrameworkCore;

namespace Get_Together_Riders.Models
{
    public class GTRDbContext : DbContext
    {

        public GTRDbContext(DbContextOptions<GTRDbContext> options) : base(options)
        { 
        }

        // add DbSets here....
    }
}
