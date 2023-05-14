namespace Get_Together_Riders.Models
{
    public interface IRideEventRepository
    {
        RideEvent GetRideEventById(int rideEventId);
        void AddRideEvent(RideEvent rideEvent);
        void UpdateRideEvent(RideEvent rideEvent);
        void DeleteRideEvent(int rideEventId);
        IEnumerable<RideEvent> GetAllRideEvents();
    }
}
