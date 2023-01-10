using Examplium.Shared.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Examplium.IdentityServer.Pages.Account.Register
{
    [SecurityHeaders]
    [AllowAnonymous]
    public class ConfirmEmailModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ConfirmEmailModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty] public RegisterViewModel View { get; set; } = new RegisterViewModel();

        public async Task<IActionResult> OnGet(string userId, string code, string returnUrl)
        {
            View = new RegisterViewModel { ReturnUrl = returnUrl };

            if (!string.IsNullOrEmpty(userId) &&  !string.IsNullOrEmpty(code))
            {
                ApplicationUser? user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    throw new ApplicationException($"Unable to load user with ID '{userId}'.");

                }

                if (user.EmailConfirmed)
                {
                    return Page();
                }

                IdentityResult confirmEmailCodeResult = await _userManager.ConfirmEmailAsync(user, code);
                if (confirmEmailCodeResult.Succeeded)
                {
                    return Page();
                }
            }            
            
            return RedirectToPage(pageName: "/Account/Login/Index", routeValues: new { View.ReturnUrl });
        }
    }
}