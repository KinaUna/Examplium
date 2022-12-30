using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Examplium.IdentityServer.Pages.Account.ForgotPassword
{
    [AllowAnonymous]
    [SecurityHeaders]
    public class ResetCodeSentModel : PageModel
    {
        [BindProperty] public ForgotPasswordViewModel View { get; set; } = new ForgotPasswordViewModel();

        public IActionResult OnGet(string returnUrl)
        {
            View = new ForgotPasswordViewModel { ReturnUrl = returnUrl };

            return Page();
        }
    }
}
