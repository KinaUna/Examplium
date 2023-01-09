using Examplium.IdentityServer.Services;
using Examplium.Shared.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Examplium.IdentityServer.Pages.Account.ForgotPassword
{
    [AllowAnonymous]
    [SecurityHeaders]
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;

        public IndexModel(UserManager<ApplicationUser> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }

        [BindProperty] public ForgotPasswordViewModel Input { get; set; } = new ForgotPasswordViewModel();

        public IActionResult OnGet(string returnUrl)
        {
            Input = new ForgotPasswordViewModel
            {
                ReturnUrl = returnUrl
            };

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                if (Input.Button == "passwordreset")
                {
                    ApplicationUser? user = await _userManager.FindByEmailAsync(Input.Email);
                    if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                    {
                        // Don't reveal that the user does not exist or is not confirmed
                        return RedirectToPage("/Account/ForgotPassword/ResetCodeSent", routeValues: new {returnUrl = Input.ReturnUrl});
                    }
                    
                    string code = await _userManager.GeneratePasswordResetTokenAsync(user);
                    if(_emailSender.SendPasswordResetEmailToUser(user, code, Input.ReturnUrl))
                    {
                        return RedirectToPage("/Account/ForgotPassword/ResetCodeSent", routeValues: new { returnUrl = Input.ReturnUrl });
                    }

                }
                               

                return Redirect(Input.ReturnUrl);

            }

            return Page();
        }
    }
}
