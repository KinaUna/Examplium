using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Examplium.IdentityServer.Pages.Account.ChangePassword
{
    [Authorize]
    [SecurityHeaders]
    public class ChangeCodeSentModel : PageModel
    {
        [BindProperty] public ChangePasswordViewModel View { get; set; } = new ChangePasswordViewModel();

        public IActionResult OnGet(string returnUrl)
        {
            View = new ChangePasswordViewModel { ReturnUrl = returnUrl };

            return Page();
        }
    }
}
