using Duende.IdentityServer.Events;
using Examplium.IdentityServer.Pages.Account.Login;
using Examplium.Shared.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Examplium.IdentityServer.Pages.Account.DeleteAccount
{
    [Authorize]
    [SecurityHeaders]
    public class ConfirmDeleteModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public ConfirmDeleteModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [BindProperty] public ConfirmDeleteAccountViewModel Input { get; set; } = new ConfirmDeleteAccountViewModel();

        public async Task<IActionResult> OnGet(string userId, string code, string returnUrl)
        {
            ApplicationUser? user = await _userManager.GetUserAsync(User);
            if (user == null || string.IsNullOrEmpty(user.Email))
            {
                throw new ApplicationException("Unable to load current user.");
            }

            Input = new ConfirmDeleteAccountViewModel
            {
                Email = user.Email,
                UserId = userId,
                ResetCode = code,
                ReturnUrl = returnUrl
            };
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                if (Input.Button == "deleteaccount")
                {
                    if (!string.IsNullOrEmpty(Input.UserId) && !string.IsNullOrEmpty(Input.ResetCode))
                    {
                        ApplicationUser? user = await _userManager.FindByIdAsync(Input.UserId);
                        if (user != null && user.Email?.ToUpper() == Input.Email.ToUpper())
                        {
                            if(await _userManager.CheckPasswordAsync(user, Input.Password))
                            {
                                IdentityResult result = await _userManager.ConfirmEmailAsync(user, Input.ResetCode);
                                if (result.Succeeded)
                                {
                                    await _userManager.DeleteAsync(user);

                                    await _signInManager.SignOutAsync();

                                    return RedirectToPage("/Account/DeleteAccount/AccountDeleted", routeValues: new { returnUrl = Input.ReturnUrl, });
                                }
                            }

                        }
                    }

                    ModelState.AddModelError(string.Empty, LoginOptions.InvalidCredentialsErrorMessage);
                }
                else
                {
                    return Redirect(Input.ReturnUrl);
                }
            }

            return Page();
        }
    }
}