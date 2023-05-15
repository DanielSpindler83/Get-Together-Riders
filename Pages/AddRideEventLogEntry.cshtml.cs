using Get_Together_Riders.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Get_Together_Riders.Pages
{
    public class AddRideEventLogEntryModel : PageModel
    {
        private readonly IRideEventLogEntryRepository _rideEventLogEntryRepository;
        private readonly IRiderRepository _riderRepository; 

        [BindProperty]
        public RideEventLogEntry RideEventLogEntry { get; set; }

        public int RiderID { get; set; }
        public int RideEventID { get; set; }

        public AddRideEventLogEntryModel(IRideEventLogEntryRepository rideEventLogEntryRepository, IRiderRepository riderRepository)
        {
            _rideEventLogEntryRepository = rideEventLogEntryRepository;
            _riderRepository = riderRepository;
        }

        public IActionResult OnGet(int rideEventId)
        {
            RideEventID = rideEventId; 

            // Get the identity ID of the logged-in user
            var identityUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Use the identity ID to fetch the Rider details
            var rider = _riderRepository.GetRiderByIdentityId(identityUserId);

            // setup the form ready for user to complete it
            // what do we need to show? Mirror from

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _rideEventLogEntryRepository.AddRideEventLogEntry(RideEventLogEntry);
            _rideEventLogEntryRepository.SaveChanges();

            return RedirectToPage("/RideEvents/Index");
        }
    }
}