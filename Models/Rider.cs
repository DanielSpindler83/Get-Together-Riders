namespace Get_Together_Riders.Models
{
    public class Rider
    {
        public int RiderID { get; set; }
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
    }
}
