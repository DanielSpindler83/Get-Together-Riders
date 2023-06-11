using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Get_Together_Riders.Pages
{
    [AllowAnonymous]
    public class ContactAdminModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
