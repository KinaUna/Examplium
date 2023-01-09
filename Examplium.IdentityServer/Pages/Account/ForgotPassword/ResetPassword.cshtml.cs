using Examplium.Shared.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Examplium.IdentityServer.Pages.Account.ForgotPassword
{
    [AllowAnonymous]
    [SecurityHeaders]
    public class ResetPasswordModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ResetPasswordModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty] public ResetPasswordViewModel Input { get; set; } = new ResetPasswordViewModel();

        public IActionResult OnGet(string userId, string code, string returnUrl)
        {
            Input = new ResetPasswordViewModel
            {
                UserId= userId,
                ResetCode = code,
                ReturnUrl = returnUrl                
            };
            
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                if (Input.Button == "setpassword")
                {
                    if (!string.IsNullOrEmpty(Input.UserId) && !string.IsNullOrEmpty(Input.ResetCode))
                    {
                        ApplicationUser? user = await _userManager.FindByIdAsync(Input.UserId);
                        if (user != null && user.Email?.ToUpper() == Input.Email.ToUpper())
                        {
                            IdentityResult result = await _userManager.ResetPasswordAsync(user, Input.ResetCode, Input.Password);
                            if (result.Succeeded)
                            {
                                return RedirectToPage("/Account/ForgotPassword/PasswordSet", routeValues: new { returnUrl = Input.ReturnUrl,});
                            }
                        }
                    }
                }
            }

            return Page();
        }
    }
}
