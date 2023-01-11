using Examplium.Shared.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Examplium.IdentityServer.Pages.Admin.ResetConfiguration
{
    [Authorize(Roles = ExampliumAuthServerConstants.AdminRole)]
    public class IndexModel : PageModel
    {
        [BindProperty] public string Reset { get; set; } = string.Empty;
        public void OnGet(string? reset)
        {
            Reset = reset ?? string.Empty;
        }
    }
}
