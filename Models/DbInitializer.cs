using Get_Together_Riders.Models.Enums;
using Microsoft.AspNetCore.Builder;

namespace Get_Together_Riders.Models
{
    public static class DbInitializer
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            GTRDbContext context = applicationBuilder.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<GTRDbContext>();

            if (!context.Riders.Any())
            {
                context.Riders.AddRange(
                new Rider { FirstName = "John", LastName = "Doe", DisplayName = "Johnny", Bio = "Some bio here", BikeModel = "Yamaha R1", Location = "New York", EmergencyContactPerson = "Jane", EmergencyContactNumber = "555-1234", GTRNumber = "123", Email = "john@example.com", PhoneNumber = "555-5678" },
                new Rider { FirstName = "Jane", LastName = "Doe", DisplayName = "Janey", Bio = "Some bio here", BikeModel = "Kawasaki Ninja", Location = "Los Angeles", EmergencyContactPerson = "John", EmergencyContactNumber = "555-4321", GTRNumber = "456", Email = "jane@example.com", PhoneNumber = "555-8765" },
                new Rider { FirstName = "Bob", LastName = "Smith", DisplayName = "Bobby", Bio = "Some bio here", BikeModel = "Honda CBR", Location = "Chicago", EmergencyContactPerson = "Susan", EmergencyContactNumber = "555-1111", GTRNumber = "789", Email = "bob@example.com", PhoneNumber = "555-2222" },
                new Rider { FirstName = "Susan", LastName = "Johnson", DisplayName = "Sue", Bio = "Some bio here", BikeModel = "Suzuki GSX", Location = "San Francisco", EmergencyContactPerson = "Bob", EmergencyContactNumber = "555-3333", GTRNumber = "246", Email = "susan@example.com", PhoneNumber = "555-4444" },
                new Rider { FirstName = "Mike", LastName = "Davis", DisplayName = "Mikey", Bio = "Some bio here", BikeModel = "Ducati Panigale", Location = "Miami", EmergencyContactPerson = "Karen", EmergencyContactNumber = "555-7777", GTRNumber = "135", Email = "mike@example.com", PhoneNumber = "555-8888" },
                new Rider { FirstName = "Karen", LastName = "Williams", DisplayName = "Kare", Bio = "Some bio here", BikeModel = "Triumph Daytona", Location = "Seattle", EmergencyContactPerson = "Mike", EmergencyContactNumber = "555-5555", GTRNumber = "802", Email = "karen@example.com", PhoneNumber = "555-6666" }
                );
            }



            if (!context.RideEvents.Any())
            {
                context.RideEvents.AddRange(
                new RideEvent { EventName = "Summer Ride", EventCategory = RideEventCategory.GTR8, StartDate = new DateTime(2022, 6, 1), EndDate = new DateTime(2022, 6, 7) },
                new RideEvent { EventName = "Charity Ride", EventCategory = RideEventCategory.GTR10, StartDate = new DateTime(2022, 7, 1), EndDate = new DateTime(2022, 7, 3) },
                new RideEvent { EventName = "Mountain Ride", EventCategory = RideEventCategory.GTR12, StartDate = new DateTime(2022, 8, 1), EndDate = new DateTime(2022, 8, 5) }
                );
            }

            context.SaveChanges();

            if (!context.RideEventEnrollments.Any())
            {
                context.RideEventEnrollments.AddRange(
                new RideEventEnrollment { RiderID = 1, RideEventID = 1, State = RiderState.Going },
                new RideEventEnrollment { RiderID = 2, RideEventID = 1, State = RiderState.Going },
                new RideEventEnrollment { RiderID = 3, RideEventID = 1, State = RiderState.Interested },
                new RideEventEnrollment { RiderID = 4, RideEventID = 2, State = RiderState.Going },
                new RideEventEnrollment { RiderID = 5, RideEventID = 2, State = RiderState.Interested },
                new RideEventEnrollment { RiderID = 1, RideEventID = 3, State = RiderState.Interested },
                new RideEventEnrollment { RiderID = 2, RideEventID = 3, State = RiderState.Going },
                new RideEventEnrollment { RiderID = 3, RideEventID = 3, State = RiderState.Interested },
                new RideEventEnrollment { RiderID = 5, RideEventID = 3, State = RiderState.Going }
                );
            }


            context.SaveChanges();
        } // end of Seed Method

    } //  end of DbInitializer Class

} // end of namespace