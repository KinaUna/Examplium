using Examplium.Shared.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Examplium.IdentityServer.Services;

namespace Examplium.IdentityServer.Pages.Admin.ResetConfiguration
{
    [Authorize(Roles = ExampliumAuthServerConstants.AdminRole)]
    public class ClientsModel : PageModel
    {
        private readonly IDatabaseInitializer _databaseInitializer;

        public ClientsModel(IDatabaseInitializer databaseInitializer)
        {
            _databaseInitializer = databaseInitializer;
        }
        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            _databaseInitializer.ResetClients();

            return RedirectToPage("/Admin/ResetConfiguration/Index", new {reset = "Clients"});
        }
    }
}
