using Get_Together_Riders.Models.Database;
using Get_Together_Riders.Models.Interfaces;

namespace Get_Together_Riders.Models.Repositories
{
    public class RideEventRepository : IRideEventRepository
    {
        private readonly GTRDbContext _gTRDbContext;

        public RideEventRepository(GTRDbContext gTRDbContext)
        {
            _gTRDbContext = gTRDbContext;
        }

        public RideEvent GetRideEventById(int rideEventId)
        {
            return _gTRDbContext.RideEvents.Find(rideEventId);
        }

        public void AddRideEvent(RideEvent rideEvent)
        {
            _gTRDbContext.RideEvents.Add(rideEvent);
            _gTRDbContext.SaveChanges();
        }

        public void UpdateRideEvent(RideEvent rideEvent)
        {
            _gTRDbContext.RideEvents.Update(rideEvent);
            _gTRDbContext.SaveChanges();
        }

        public void DeleteRideEvent(int rideEventId)
        {
            var rideEvent = _gTRDbContext.RideEvents.Find(rideEventId);
            if (rideEvent != null)
            {
                _gTRDbContext.RideEvents.Remove(rideEvent);
                _gTRDbContext.SaveChanges();
            }
        }

        public IEnumerable<RideEvent> GetAllRideEvents()
        {
            return _gTRDbContext.RideEvents.ToList();
        }
    }

}
