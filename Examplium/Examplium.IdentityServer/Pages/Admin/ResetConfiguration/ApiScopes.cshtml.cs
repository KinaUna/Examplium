using Examplium.IdentityServer.Services;
using Examplium.Shared.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Examplium.IdentityServer.Pages.Admin.ResetConfiguration
{
    [Authorize(Roles = ExampliumAuthServerConstants.AdminRole)]
    public class ApiScopesModel : PageModel
    {
        private readonly IDatabaseInitializer _databaseInitializer;

        public ApiScopesModel(IDatabaseInitializer databaseInitializer)
        {
            _databaseInitializer = databaseInitializer;
        }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            _databaseInitializer.ResetApiScopes();

            return RedirectToPage("/Admin/ResetConfiguration/Index", new { reset = "Api Scopes" });
        }
    }
}
