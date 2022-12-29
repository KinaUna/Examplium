# Add Register/Sign up Pages

## Create Register folder and files

### Add folder

Add a new folder named "Register" here: Examplium.IdentityServer/Pages/Account/

### Add RegisterViewModel.cs
In the folder created add a new class file: RegisterViewModel.cs

Add this code to the file:
```
using System.ComponentModel.DataAnnotations;

namespace Examplium.IdentityServer.Pages.Account.Register
{
    public class RegisterViewModel
    {
        [Required] public string Email { get; set; }
        [Required] public string Password { get; set; }
        [Required] public string ConfirmPassword { get; set; }

        public string ReturnUrl { get; set; }
        public string Button { get; set; }
    }
}
```

### Add Index page

Add a new blank Razor page to the Register folder, name it "Index.cshtml".

Replace the content with this code:
```
@page
@model Examplium.IdentityServer.Pages.Account.Register.IndexModel
@{
}

<div class="login-page">
    <div class="lead">
        <h1>Register a new account</h1>
    </div>

    <partial name="_ValidationSummary" />

    <div class="row">

        <div class="col-sm-6">
            <div class="card">
                <div class="card-header">
                    <h2>Local Account</h2>
                </div>

                <div class="card-body">
                    <form asp-page="/Account/Register/Index">
                        <input type="hidden" asp-for="Input.ReturnUrl"/>

                        <div class="form-group">
                            <label asp-for="Input.Email"></label>
                            <input class="form-control" placeholder="Email" asp-for="Input.Email" autofocus>
                        </div>
                        <div class="form-group">
                            <label asp-for="Input.Password"></label>
                            <input type="password" class="form-control" placeholder="Password" asp-for="Input.Password" autocomplete="off">
                        </div>
                        <div class="form-group">
                            <label asp-for="Input.ConfirmPassword"></label>
                            <input type="password" class="form-control" placeholder="Confirm Password" asp-for="Input.ConfirmPassword" autocomplete="off">
                        </div>

                        <button class="btn btn-primary" name="Input.Button" value="register">Register</button>
                        <button class="btn btn-secondary" name="Input.Button" value="cancel">Cancel</button>
                    </form>
                </div>
            </div>
        </div>

    </div>
</div>
```

Replace the content of Index.cshtml.cs with this code:
```
using Examplium.Shared.Models.Identity;
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
```

### Add Registered page

Add a new blank Razor page to the Register folder, name it "Registered.cshtml".

Insert the follow code:
```
@page
@model Examplium.IdentityServer.Pages.Account.Register.RegisteredModel
@{
}
<div class="login-page">
    <div class="lead">
        <h1>Thank you for registering</h1>
    </div>
    <div class="row">
        <div>
            Thank you for registering a new account.
        </div>
        <div>
            An email has been sent to confirm your email address.
        </div>
        <div>
            If you do not see the email in your inbox within a few minutes, please check your spam/junk email folder. 
        </div>
        
    </div>
    <div class="row">
        <a asp-page="Login" asp-route-returnUrl="@Model.View.ReturnUrl">Return to login</a>
    </div>
</div>
```

Replace the contents of Registered.cshtml.cs with this:
```
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Examplium.IdentityServer.Pages.Account.Register
{
    [AllowAnonymous]
    [SecurityHeaders]
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
```

### Add link to register on the login page.

Add a link to the register page on the login page:
```
<a asp-page="/Account/Register/Index" asp-route-returnUrl="@Model.Input.ReturnUrl">Register as a new user</a>
```
