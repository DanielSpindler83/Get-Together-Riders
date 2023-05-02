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

        // how do i represent the list of users that are registered for this ride - could be confirmed or maybe (not exact terminology - refer DB group)
        // we would evenutally sync a list of users from the Facebook group
        // debating on if we just have two lists of riders? - ConfirmedRiders and TentativeRiders
        // Options, going, not going and interested
    }
}
