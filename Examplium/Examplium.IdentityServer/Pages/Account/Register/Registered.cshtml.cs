using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Examplium.IdentityServer.Pages.Account.Register
{
    [SecurityHeaders]
    [AllowAnonymous]
    public class RegisteredModel : PageModel
    {
        
        [BindProperty] public RegisterViewModel View { get; set; } = new RegisterViewModel();

        public IActionResult OnGet(string returnUrl)
        {
            View = new RegisterViewModel { ReturnUrl = returnUrl};

            return Page();
        }
    }
}
