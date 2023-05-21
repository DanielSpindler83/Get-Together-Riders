using Get_Together_Riders.Models;
using Get_Together_Riders.Models.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Get_Together_Riders.Pages
{
    [Authorize]
    public class RideEventsModel : PageModel
    {
        private readonly IRideEventRepository _rideEventRepository;

        public IEnumerable<RideEvent> ActiveRideEvents { get; set; }
        public IEnumerable<RideEvent> PastRideEvents { get; set; }

        public IEnumerable<RideEvent> RideEvents { get; set; }

        public RideEventsModel(IRideEventRepository rideEventRepository)
        {
            _rideEventRepository = rideEventRepository;
        }

        public void OnGet()
        {
            var allRideEvents = _rideEventRepository.GetAllRideEvents();

            var currentDate = DateTime.Now;

            ActiveRideEvents = allRideEvents.Where(re => re.StartDate <= currentDate && re.EndDate >= currentDate);
            PastRideEvents = allRideEvents.Where(re => re.EndDate < currentDate);
        }
    }
}
