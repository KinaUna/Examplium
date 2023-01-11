using Examplium.Shared.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Examplium.IdentityServer.Pages.Admin
{
    [Authorize(Roles = ExampliumAuthServerConstants.AdminRole)]
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
