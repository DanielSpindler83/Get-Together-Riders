using Get_Together_Riders.Models;
using Get_Together_Riders.Models.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Get_Together_Riders.Pages
{
    [Authorize]
    public class RideEventLogEntriesModel : PageModel
    {
        private readonly IRideEventLogEntryRepository _rideEventLogEntryRepository; 

        public RideEventLogEntriesModel(IRideEventLogEntryRepository rideEventLogEntryRepository)
        {
            _rideEventLogEntryRepository = rideEventLogEntryRepository;
        }

        public IEnumerable<RideEventLogEntry> RideEventLogEntries { get; set; }

        public void OnGet(int? rideEventID)
        {
            RideEventLogEntries = _rideEventLogEntryRepository.GetAllRideEventLogEntries();

            if (rideEventID.HasValue)
            {
                RideEventLogEntries = RideEventLogEntries.Where(entry => entry.RideEventID == rideEventID.Value);
            }

        }
    }
}
