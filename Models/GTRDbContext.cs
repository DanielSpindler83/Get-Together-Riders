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

        public DbSet<RideEventLogEntry> RideEventLogEntrys { get; set; }


        //// allows nullable foreign key 
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Rider>()
        //        .HasOne(r => r.IdentityUser)
        //        .WithMany()
        //        .HasForeignKey(r => r.IdentityUserId)
        //        .OnDelete(DeleteBehavior.Restrict) // DeleteBehavior.Restrict is used to specify that when a related IdentityUser is deleted, the deletion is restricted and does not cascade to the Rider entity.
        //        .HasConstraintName("FK_Riders_AspNetUsers_IdentityUserId");
        //}
    }
}
