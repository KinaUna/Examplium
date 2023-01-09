using Duende.IdentityServer.Extensions;
using Examplium.IdentityServer.Services;
using Examplium.Shared.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Examplium.IdentityServer.Pages.Account.DeleteAccount
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

        [BindProperty] public DeleteAccountViewModel Input { get; set; } = new DeleteAccountViewModel();

        public IActionResult OnGet(string returnUrl)
        {
            Input = new DeleteAccountViewModel
            {
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
                    ApplicationUser? user = await _userManager.FindByEmailAsync(Input.Email);
                    if (user == null || User.GetSubjectId() != user.Id)
                    {
                        throw new ApplicationException("Unable to load current user.");
                    }

                    string code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    if (_emailSender.SendDeleteAccountEmailToUser(user, code, Input.ReturnUrl))
                    {
                        return RedirectToPage("/Account/DeleteAccount/DeleteAccountEmailSent", routeValues: new { returnUrl = Input.ReturnUrl });
                    }

                }
                
                return Redirect(Input.ReturnUrl);

            }

            return Page();
        }

    }    
}