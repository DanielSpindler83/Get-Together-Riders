using Microsoft.AspNetCore.Identity;

namespace Get_Together_Riders.Models
{
    public class Rider
    {
        public int RiderID { get; set; }
        public string? IdentityUserId { get; set; } // link to IdentityUserId
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string? Bio { get; set; }
        public string? Email { get; set; } // should be set same as facebook email login when this is added
        public string? PhoneNumber { get; set; }
        public string? BikeModel { get; set; }
        public string? Location { get; set; }
        public string? EmergencyContactPerson { get; set; }
        public string? EmergencyContactNumber { get; set; }
        public string? GTRNumber { get; set; }
        public string? Status { get; set; }
        public bool IsActive { get; set; } = true;
        public List<RideEventEnrollment> RideEventEnrollments { get; set; } = new List<RideEventEnrollment>();


        public override string ToString()
        {
            return $"RiderID: {RiderID}\n" +
                   $"IdentityUserId: {IdentityUserId ?? "null"}\n" +
                   $"FirstName: {FirstName}\n" +
                   $"LastName: {LastName}\n" +
                   $"DisplayName: {DisplayName}\n" +
                   $"Bio: {Bio ?? "null"}\n" +
                   $"Email: {Email ?? "null"}\n" +
                   $"PhoneNumber: {PhoneNumber ?? "null"}\n" +
                   $"BikeModel: {BikeModel ?? "null"}\n" +
                   $"Location: {Location ?? "null"}\n" +
                   $"EmergencyContactPerson: {EmergencyContactPerson ?? "null"}\n" +
                   $"EmergencyContactNumber: {EmergencyContactNumber ?? "null"}\n" +
                   $"GTRNumber: {GTRNumber ?? "null"}\n" +
                   $"Status: {Status ?? "null"}\n" +
                   $"IsActive: {IsActive}\n" +
                   $"RideEventEnrollments: {GetRideEventEnrollmentDetails()}";
        }

        private string GetRideEventEnrollmentDetails()
        {
            if (RideEventEnrollments.Count > 0)
            {
                return string.Join(", ", RideEventEnrollments);
            }
            else
            {
                return "No ride event enrollments";
            }
        }
    }

}
