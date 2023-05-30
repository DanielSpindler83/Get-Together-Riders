using Get_Together_Riders.Models.Enums;
using Get_Together_Riders.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Get_Together_Riders.Models.Interfaces;

namespace Get_Together_Riders.Pages
{
    [Authorize(Roles = "Admin")]
    public class AddRideEventModel : PageModel
    {
        private readonly IRideEventRepository _rideEventRepository;
        private readonly ILogger<AddRideEventModel> _logger;

        public AddRideEventModel(IRideEventRepository rideEventRepository, ILogger<AddRideEventModel> logger)
        {
            _rideEventRepository = rideEventRepository;
            _logger = logger;
            RideEvent = new RideEvent(); // Initialize RideEvent property
        }

        [BindProperty]
        public RideEvent RideEvent { get; set; }

        public List<RideEventCategory> RideEventCategories { get; set; }

        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            RideEventCategories = Enum.GetValues(typeof(RideEventCategory)).Cast<RideEventCategory>().ToList();
            _logger.LogInformation("AddRideEvent OnGetAsync.");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                _rideEventRepository.AddRideEvent(RideEvent);
                return RedirectToPage("/Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding ride event");
                ErrorMessage = "An error occurred while adding the ride event";
                return Page();
            }
        }
    }

}
