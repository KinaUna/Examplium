using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Examplium.IdentityServer.Pages.Diagnostics
{
    [SecurityHeaders]
    [Authorize]
    public class IndexModel : PageModel
    {
        public DiagnosticsViewModel? View { get; set; }

        public async Task<IActionResult> OnGet()
        {
            if(HttpContext.Connection.LocalIpAddress != null)
            {
                var localAddresses = new string[] { "127.0.0.1", "::1", HttpContext.Connection.LocalIpAddress.ToString() };
                if (HttpContext.Connection.RemoteIpAddress != null && !localAddresses.Contains(HttpContext.Connection.RemoteIpAddress.ToString()))
                {
                    return NotFound();
                }

                View = new DiagnosticsViewModel(await HttpContext.AuthenticateAsync());
            }
            

            return Page();
        }
    }
}
