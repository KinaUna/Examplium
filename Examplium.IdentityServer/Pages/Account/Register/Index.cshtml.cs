using Examplium.Shared.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Examplium.IdentityServer.Pages.Account.Register
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        

        public IndexModel( UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
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

                    var result = await _userManager.CreateAsync(user, Input.Password);

                    if (result.Errors.Any())
                    {
                        return Page(); // Todo: Show error.
                    }

                    // Todo: generate confirmation code and send it by email.

                    return RedirectToPage(pageName: "Registered", routeValues: new { returnUrl });
                }
                
                return Redirect(returnUrl);
                
            }

            return Page();
        }
    }
}
