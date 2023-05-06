using Get_Together_Riders.Models.Enums;

namespace Get_Together_Riders.Models
{
    public class RideEvent
    {
        public int RideEventID { get; set; }
        public string EventName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public RideEventCategory EventCategory { get; set; } 
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<RideEventEnrollment> RideEventEnrollments { get; set; } = new List<RideEventEnrollment>();
    }
}
