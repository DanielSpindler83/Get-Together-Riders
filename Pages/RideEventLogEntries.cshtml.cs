using Get_Together_Riders.Models;
using Get_Together_Riders.Models.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
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

        public IActionResult OnPostDelete(int rideEventLogEntryID)
        {
            var entryToDelete = _rideEventLogEntryRepository.GetRideEventLogEntryById(rideEventLogEntryID);
            if (entryToDelete != null)
            {
                _rideEventLogEntryRepository.DeleteRideEventLogEntry(rideEventLogEntryID);
                _rideEventLogEntryRepository.SaveChanges();
            }
            return RedirectToPage(); // Redirect back to the same page after deletion
        }
    }
}
