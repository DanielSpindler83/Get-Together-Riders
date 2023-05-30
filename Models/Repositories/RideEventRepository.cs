using Get_Together_Riders.Models.Database;
using Get_Together_Riders.Models.Interfaces;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Diagnostics;

namespace Get_Together_Riders.Models.Repositories
{
    public class RideEventRepository : IRideEventRepository
    {
        private readonly GTRDbContext _gTRDbContext;
        private readonly ILogger<RideEventRepository> _logger;

        public RideEventRepository(GTRDbContext gTRDbContext, ILogger<RideEventRepository> logger)
        {
            _gTRDbContext = gTRDbContext;
            _logger = logger;
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
            // Logging test
            var timer = new Stopwatch();
            timer.Start();
            var events = _gTRDbContext.RideEvents.ToList();
            timer.Stop();

            _logger.LogDebug("Querying for all Ride Events finished in {milliseconds} milliseconds", timer.ElapsedMilliseconds);

            return events;
        }
    }

}
