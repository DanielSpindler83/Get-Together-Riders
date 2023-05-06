using Get_Together_Riders.Models.Enums;

namespace Get_Together_Riders.Models
{
    public class RideEventEnrollment
    {
        public int RideEventEnrollmentID { get; set; }
        public int RiderID { get; set; }
        public Rider Rider { get; set; }
        public int RideEventID { get; set; }
        public RideEvent RideEvent { get; set; }
        public RiderState State { get; set; }
    }
}
