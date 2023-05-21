using Get_Together_Riders.Models;
using Get_Together_Riders.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Get_Together_Riders.Pages
{
    public class AddRideEventLogEntryModel : PageModel
    {
        private readonly IRideEventLogEntryRepository _rideEventLogEntryRepository;
        private readonly IRideEventRepository _rideEventRepository;
        private readonly IRiderRepository _riderRepository; 

        [BindProperty]
        public RideEventLogEntry RideEventLogEntry { get; set; }

        public int RideEventID { get; set; }
        public Rider CurrentRider { get; private set; }


        public AddRideEventLogEntryModel(IRideEventLogEntryRepository rideEventLogEntryRepository, IRiderRepository riderRepository, IRideEventRepository rideEventRepository)
        {
            _rideEventLogEntryRepository = rideEventLogEntryRepository;
            _riderRepository = riderRepository;
            _rideEventRepository = rideEventRepository;
        }

        public IActionResult OnGet(int rideEventId)
        {
            RideEventID = rideEventId;

            // Get the identity ID of the logged-in user
            var identityUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Use the identity ID to fetch the Rider details
            CurrentRider = _riderRepository.GetRiderByIdentityId(identityUserId);

            // setup the form ready for user to complete it
            // what do we need to show? Mirror from

            return Page();
        }

        public IActionResult OnPost()
        {

            // Get the identity ID of the logged-in user
            var identityUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Use the identity ID to fetch the Rider details
            CurrentRider = _riderRepository.GetRiderByIdentityId(identityUserId);

            // another approach here is to use a ViewModel class just for the form fields
            ModelState.Remove("RideEventLogEntry.Rider");
            ModelState.Remove("RideEventLogEntry.RideEvent");

            if (!ModelState.IsValid)
            {
                return Page();
            }

            RideEventLogEntry.Rider = CurrentRider;
            RideEventLogEntry.RideEvent = _rideEventRepository.GetRideEventById(RideEventLogEntry.RideEventID);

            _rideEventLogEntryRepository.AddRideEventLogEntry(RideEventLogEntry);
            _rideEventLogEntryRepository.SaveChanges();

            return RedirectToPage("/RideEvents");
        }
    }
}