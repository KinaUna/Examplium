using Examplium.IdentityServer.Services;
using Examplium.Shared.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Examplium.IdentityServer.Pages.Admin.ResetConfiguration
{
    [Authorize(Roles = ExampliumAuthServerConstants.AdminRole)]
    public class AllModel : PageModel
    {
        private readonly IDatabaseInitializer _databaseInitializer;

        public AllModel(IDatabaseInitializer databaseInitializer)
        {
            _databaseInitializer = databaseInitializer;
        }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            _databaseInitializer.ResetClients();
            _databaseInitializer.ResetIdentityResources();
            _databaseInitializer.ResetApiScopes();

            return RedirectToPage("/Admin/ResetConfiguration/Index", new { reset = "All configurations" });
        }
    }
}
