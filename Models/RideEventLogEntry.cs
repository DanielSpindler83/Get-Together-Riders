namespace Get_Together_Riders.Models
{
    public class RideEventLogEntry
    {
        public int RideEventLogEntryID { get; set; }

        public int RiderID { get; set; }
        public Rider Rider { get; set; }

        public int RideEventID { get; set; }
        public RideEvent RideEvent { get; set; }

        public string SuburbLeftFrom { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public int KmsToCheckIn { get; set; }
        public int TotalKms { get; set; }
    }
}
