using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Examplium.IdentityServer.Pages.Account.DeleteAccount
{
    [AllowAnonymous]
    [SecurityHeaders]
    public class AccountDeletedModel : PageModel
    {
        [BindProperty] public string ReturnUrl { get; set; } = string.Empty;

        public void OnGet(string returnUrl)
        {
            ReturnUrl = returnUrl;
        }
    }
}