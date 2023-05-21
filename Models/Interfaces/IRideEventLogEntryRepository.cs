namespace Get_Together_Riders.Models.Interfaces
{
    public interface IRideEventLogEntryRepository
    {
        RideEventLogEntry GetRideEventLogEntryById(int id);
        void AddRideEventLogEntry(RideEventLogEntry rideEventLogEntry);
        void UpdateRideEventLogEntry(RideEventLogEntry rideEventLogEntry);
        void DeleteRideEventLogEntry(int id);
        void SaveChanges();
    }
}
