using Examplium.IdentityServer.Services;
using Examplium.Shared.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Examplium.IdentityServer.Pages.Account.Register
{
    [AllowAnonymous]
    [SecurityHeaders]
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;

        public IndexModel( UserManager<ApplicationUser> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }


        [BindProperty] public RegisterViewModel Input { get; set; } = new RegisterViewModel();

        public IActionResult OnGet(string returnUrl)
        {
            Input = new RegisterViewModel
            {
                ReturnUrl = returnUrl
            };
            return Page();
        }

        public async Task<IActionResult> OnPost(string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (Input.Button == "register")
                {
                    var user = new ApplicationUser()
                    {
                        UserName = Input.Email,
                        Email = Input.Email,
                        EmailConfirmed = false
                    };

                    var createUserResult = await _userManager.CreateAsync(user, Input.Password);

                    if (createUserResult.Errors.Any())
                    {
                        return Page(); // Todo: Show error.
                    }

                    string code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    if(_emailSender.SendConfirmationEmailToUser(user, code))
                    {
                        return RedirectToPage(pageName: "Registered", routeValues: new { returnUrl });
                    }
                    
                    // Todo: Show error.
                    
                }
                
                return Redirect(returnUrl);
                
            }

            return Page();
        }
    }
}
