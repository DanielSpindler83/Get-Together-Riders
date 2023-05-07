using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Get_Together_Riders.Models
{
    public class GTRDbContext : IdentityDbContext
    {

        public GTRDbContext(DbContextOptions<GTRDbContext> options) : base(options)
        { 
        }

        // add DbSets here....
        public DbSet<Rider> Riders { get; set; }
        public DbSet<RideEvent> RideEvents { get; set; }
        public DbSet<RideEventEnrollment> RideEventEnrollments { get; set; }
    }
}
