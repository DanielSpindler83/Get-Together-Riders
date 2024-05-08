using Get_Together_Riders.Models.Database;
using Get_Together_Riders.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Get_Together_Riders.Models.Repositories
{
    public class RideEventLogEntryRepository : IRideEventLogEntryRepository
    {
        private readonly GTRDbContext _gTRDbContext;

        public RideEventLogEntryRepository(GTRDbContext gTRDbContext)
        {
            _gTRDbContext = gTRDbContext;
        }

        public RideEventLogEntry GetRideEventLogEntryById(int id)
        {
            return _gTRDbContext.RideEventLogEntries.FirstOrDefault(r => r.RideEventLogEntryID == id);
        }

        public void AddRideEventLogEntry(RideEventLogEntry rideEventLogEntry)
        {
            _gTRDbContext.RideEventLogEntries.Add(rideEventLogEntry);
        }

        public void UpdateRideEventLogEntry(RideEventLogEntry rideEventLogEntry)
        {
            _gTRDbContext.RideEventLogEntries.Update(rideEventLogEntry);
        }

        public void DeleteRideEventLogEntry(int id)
        {
            var rideEventLogEntry = GetRideEventLogEntryById(id);
            if (rideEventLogEntry != null)
            {
                _gTRDbContext.RideEventLogEntries.Remove(rideEventLogEntry);
            }
        }

        public void SaveChanges()
        {
            _gTRDbContext.SaveChanges();
        }

        public IEnumerable<RideEventLogEntry> GetAllRideEventLogEntries()
        {
            // Note we also pull back the navigation properties Rider and RideEvent
            var eventLogEntries = _gTRDbContext.RideEventLogEntries
                                    .Include(entry => entry.Rider)
                                    .Include(entry => entry.RideEvent)
                                    .ToList();
 
            return eventLogEntries;
        }
    }


}
