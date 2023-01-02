# Add Change Password Feature

Note: This feature depends on the email service created in [Add Confirmation Email](https://github.com/KinaUna/Examplium/blob/master/Documentation/Code/IdentityServer/03-Add-Confirmation-Email.md)

### Update IEmailSender.cs and EmailSender.cs

In Examplium.IdentityServer/Services/IEmailSender.cs add this line:
```
bool SendChangePasswordEmailToUser(ApplicationUser user, string code, string returnUrl);
```

Examplium.IdentityServer/Services/EmailSender.cs add this code after the `SendConfirmationToUser()` method:
```
public bool SendChangePasswordEmailToUser(ApplicationUser user, string code, string returnUrl)
{
    if (!string.IsNullOrEmpty(user.Email) && !string.IsNullOrEmpty(code))
    {
        var currentContext = _httpContextAccessor.HttpContext;
        string encodedCode = Uri.EscapeDataString(code);
        string codeLink = $"{currentContext?.Request.Scheme}://{currentContext?.Request.Host}/Account/ChangePassword/ResetPassword?userId={user.Id}&code={encodedCode}&returnUrl={returnUrl}";
        string emailSubject = "Change password";
        string emailText = $"Please click the following link to change your {ExampliumCoreConstants.ApplicationName} account's password: <a href='{codeLink}'>link</a>";
        return SendEmail(user.Email, emailSubject, emailText);
    }

    return false;
}
```

<br/>

### Add new ChangePassword folder

Add a new folder named "ChangePassword" in Examplium.IdentityServer/Pages/Account/

<br/>

### Add ChangePasswordViewModel.cs

Replace the content with this code:
```
using System.ComponentModel.DataAnnotations;

namespace Examplium.IdentityServer.Pages.Account.ChangePassword
{
    public class ChangePasswordViewModel
    {
        [Required] public string Email { get; set; } = string.Empty;
        [Required] public string Password { get; set; } = string.Empty;
        [Required] public string ConfirmPassword { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string ResetCode { get; set; } = string.Empty;
        public string Button { get; set; } = string.Empty;
        public string ReturnUrl { get; set; } = string.Empty;
    }
}
```

<br/>

### Add Index.cshtml

Add an empty Razor page, name it "Index.cshtml".

Replace the content with this:
```
@page
@model Examplium.IdentityServer.Pages.Account.ChangePassword.IndexModel
@{
}
<div class="login-page">
    <div class="lead">
        <h1>Forgot Password</h1>
    </div>

    <partial name="_ValidationSummary" />

    <div class="row">

        <div class="col-sm-6">
            <div class="card">
                <div class="card-header">
                    <h2>Reset password</h2>
                </div>

                <div class="card-body">
                    <form asp-page="/Account/ChangePassword/Index">
                        <input type="hidden" asp-for="Input.ReturnUrl" />

                        <div class="form-group">
                            <label asp-for="Input.Email"></label>
                            <input class="form-control" placeholder="Email" asp-for="Input.Email" readonly>
                        </div>

                        <button class="btn btn-primary" name="Input.Button" value="passwordreset">Change my password</button>
                        <button class="btn btn-secondary" name="Input.Button" value="cancel">Cancel</button>
                    </form>
                </div>
                <div class="card-footer">
                    <div>
                        When you click "Change my password" an email will be sent to your email address with further instructions.
                    </div>
                </div>
            </div>
        </div>

    </div>
</div>
```
Change the code behind file Index.cshtml.cs to this:
```
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
```
<br/>

### Add ChangeCodeSent page

Add a new blank Razor page named "ChangeCodeSent.cshtml" to the ChangePassword folder.

Change the content to this:
```
@page
@model Examplium.IdentityServer.Pages.Account.ChangePassword.ChangeCodeSentModel
@{
}

<div class="login-page">
    <div class="lead">
        <h1>Password change link sent</h1>
    </div>
    <div class="row">
        <div>
            Please check your email for further instructions on how to change your password.
        </div>
    </div>
    <div class="row">
        <a href="@Model.View.ReturnUrl">Return</a>
    </div>
</div>
```

Change the content of the codebehind page "ChangeCodeSent.cshtml to this:
```
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Examplium.IdentityServer.Pages.Account.ChangePassword
{
    [Authorize]
    [SecurityHeaders]
    public class ChangeCodeSentModel : PageModel
    {
        [BindProperty] public ChangePasswordViewModel View { get; set; } = new ChangePasswordViewModel();

        public IActionResult OnGet(string returnUrl)
        {
            View = new ChangePasswordViewModel { ReturnUrl = returnUrl };

            return Page();
        }
    }
}
```

<br/>

### Add ResetPasswordViewModel.cs
Add a new class file named "ResetPasswordViewModel.cs" in the folder Examplium.IdentityServer/Pages/Account/ChangePassword/

Replace the content with this code:
```
using System.ComponentModel.DataAnnotations;

namespace Examplium.IdentityServer.Pages.Account.ChangePassword
{
    public class ResetPasswordViewModel
    {
        [Required] public string Email { get; set; } = string.Empty;
        [Required] public string Password { get; set; } = string.Empty;
        [Required] public string ConfirmPassword { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string ResetCode { get; set; } = string.Empty;
        public string Button { get; set; } = string.Empty;
        public string ReturnUrl { get; set; } = string.Empty;
        
    }
}
```

<br/>

### Add ResetPassword page

Add a new blank Razor page named "ResetPassword.cshtml" to the ChangePassword folder.

Replace the content with this:
```
@page
@model Examplium.IdentityServer.Pages.Account.ChangePassword.ResetPasswordModel
@{
}
<div class="login-page">
    <div class="lead">
        <h1>Reset password</h1>
    </div>

    <partial name="_ValidationSummary" />

    <div class="row">

        <div class="col-sm-6">
            <div class="card">
                <div class="card-header">
                    <h2>@Examplium.Shared.Constants.ExampliumCoreConstants.OrganizationName Account</h2>
                </div>

                <div class="card-body">
                    <form asp-page="/Account/ResetPassword/ResetPassword">
                        <input type="hidden" asp-for="Input.ReturnUrl" />
                        <input type="hidden" asp-for="Input.ResetCode" />
                        <input type="hidden" asp-for="Input.UserId" />

                        <div class="form-group">
                            <label asp-for="Input.Email"></label>
                            <input class="form-control" placeholder="Email" asp-for="Input.Email" readonly>
                        </div>
                        <div class="form-group">
                            <label asp-for="Input.Password"></label>
                            <input type="password" class="form-control" placeholder="Password" asp-for="Input.Password" autocomplete="off" autofocus>
                        </div>
                        <div class="form-group">
                            <label asp-for="Input.ConfirmPassword"></label>
                            <input type="password" class="form-control" placeholder="Confirm Password" asp-for="Input.ConfirmPassword" autocomplete="off">
                        </div>

                        <button class="btn btn-primary" name="Input.Button" value="setpassword">Set password</button>
                        <button class="btn btn-secondary" name="Input.Button" value="cancel">Cancel</button>
                    </form>
                </div>
            </div>
        </div>

    </div>
</div>
```
Replace the code behind file "ResetPassword.cshtml.cs" content with this code:
```
using Examplium.Shared.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Examplium.IdentityServer.Pages.Account.ChangePassword
{
    [Authorize]
    [SecurityHeaders]
    public class ResetPasswordModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ResetPasswordModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty] public ResetPasswordViewModel Input { get; set; } = new ResetPasswordViewModel();

        public async Task<IActionResult> OnGet(string userId, string code, string returnUrl)
        {
            ApplicationUser? user = await _userManager.GetUserAsync(User);
            if (user == null || string.IsNullOrEmpty(user.Email))
            {
                throw new ApplicationException("Unable to load current user.");
            }

            Input = new ResetPasswordViewModel
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
                                return RedirectToPage("/Account/ChangePassword/PasswordSet", routeValues: new { returnUrl = Input.ReturnUrl, });
                            }
                        }
                    }
                }
            }

            return Page();
        }
    }
}
```
<br/>

### Add PasswordSet page

Add a new blank Razor page to the ChangePassword folder, name it "PasswordSet.cshtml" and replace the content with this:
```
@page
@model Examplium.IdentityServer.Pages.Account.ChangePassword.PasswordSetModel
@{
}

<div class="login-page">
    <div class="lead">
        <h1>Password set</h1>
    </div>
    <div class="row">
        <div>
            Your password has been set.
        </div>
    </div>
    <div class="row">
        <a href="@Model.View.ReturnUrl">Return</a>
    </div>
</div>
```

Replace the content of the code behind file, "PasswordSet.cshtml.cs" with the following code:
```
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Examplium.IdentityServer.Pages.Account.ChangePassword
{
    public class PasswordSetModel : PageModel
    {
        [BindProperty] public ChangePasswordViewModel View { get; set; } = new ChangePasswordViewModel();

        public IActionResult OnGet(string returnUrl)
        {
            View = new ChangePasswordViewModel
            {
                ReturnUrl = returnUrl
            };

            return Page();
        }
    }
}
```

<br/>

### Update \_Layout.cshtml 

Add the following lines to the Examplium.IdentityServer/Pages/Shared/\_Layout.cshtml file, after the last `<li class="nav-item">` element:
```
@if (User.Identity != null && User.Identity.IsAuthenticated)
{
    <li class="nav-item">
        @{
            string returnUrl = string.IsNullOrEmpty(Context.Request.Path) ? "~/" : Context.Request.GetEncodedUrl();
        }
        <a class="nav-link text-dark" asp-area="" asp-page="/Account/ChangePassword/Index" asp-route-returnUrl="@returnUrl">Change Password</a>
    </li>
}
```

Add this using statement at the top:
```
@using Microsoft.AspNetCore.Http.Extensions;
```

<br/>
