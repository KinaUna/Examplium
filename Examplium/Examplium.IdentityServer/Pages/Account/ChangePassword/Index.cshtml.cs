using Examplium.IdentityServer.Services;
using Examplium.Shared.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Examplium.IdentityServer.Pages.Account.ChangePassword
{
    [Authorize]
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

        [BindProperty] public ChangePasswordViewModel Input { get; set; } = new ChangePasswordViewModel();

        public async Task<IActionResult> OnGet(string returnUrl)
        {
            ApplicationUser? user = await _userManager.GetUserAsync(User);
            if (user == null || string.IsNullOrEmpty(user.Email))
            {
                throw new ApplicationException("Unable to load current user.");
            }

            Input = new ChangePasswordViewModel
            {
                Email = user.Email,
                ReturnUrl = returnUrl
            };

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                if (Input.Button == "changepassword")
                {
                    ApplicationUser? user = await _userManager.GetUserAsync(User);
                    if (user == null)
                    {
                        throw new ApplicationException("Unable to load current user.");
                    }

                    string code = await _userManager.GeneratePasswordResetTokenAsync(user);
                    if (_emailSender.SendChangePasswordEmailToUser(user, code, Input.ReturnUrl))
                    {
                        return RedirectToPage("/Account/ChangePassword/ChangeCodeSent", routeValues: new { returnUrl = Input.ReturnUrl });
                    }

                }


                return Redirect(Input.ReturnUrl);

            }

            return Page();
        }
    }
}
