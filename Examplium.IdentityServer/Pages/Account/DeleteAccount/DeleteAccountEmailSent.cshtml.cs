using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Examplium.IdentityServer.Pages.Account.DeleteAccount
{
    [Authorize]
    [SecurityHeaders]
    public class DeleteAccountEmailSentModel : PageModel
    {
        [BindProperty] public DeleteAccountViewModel View { get; set; } = new DeleteAccountViewModel();

        public IActionResult OnGet(string returnUrl)
        {
            View = new DeleteAccountViewModel { ReturnUrl = returnUrl };

            return Page();
        }
    }
}