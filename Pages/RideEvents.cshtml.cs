using Get_Together_Riders.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Get_Together_Riders.Pages
{
    [Authorize]
    public class RideEventsModel : PageModel
    {
        private readonly IRideEventRepository _rideEventRepository;

        public IEnumerable<RideEvent> RideEvents { get; set; }

        public RideEventsModel(IRideEventRepository rideEventRepository)
        {
            _rideEventRepository = rideEventRepository;
        }

        public void OnGet()
        {
            RideEvents = _rideEventRepository.GetAllRideEvents();
        }
    }
}
