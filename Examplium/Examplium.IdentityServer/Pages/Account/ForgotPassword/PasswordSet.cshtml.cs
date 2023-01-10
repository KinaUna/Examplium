using Examplium.IdentityServer.Pages.Account.ForgotPassword;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Examplium.IdentityServer.Pages.Account.Register
{
    [AllowAnonymous]
    [SecurityHeaders]
    public class PasswordSetModel : PageModel
    {
        [BindProperty] public ForgotPasswordViewModel View { get; set; } = new ForgotPasswordViewModel();

        public IActionResult OnGet(string returnUrl)
        {
            View = new ForgotPasswordViewModel
            {
                ReturnUrl = returnUrl
            };

            return Page();
        }
    }
}
