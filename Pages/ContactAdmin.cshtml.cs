using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Get_Together_Riders.Pages
{
    public class ContactAdminModel : PageModel
    {
        [AllowAnonymous]
        public void OnGet()
        {
        }
    }
}
