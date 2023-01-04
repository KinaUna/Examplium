# Add Delete Account Feature to Identity Server

<br/>

Note: This feature depends on the email service created in [Add Confirmation Email](https://github.com/KinaUna/Examplium/blob/master/Documentation/Code/IdentityServer/03-Add-Confirmation-Email.md)

<br/>

### Update IEmailSender.cs and EmailSender.cs

In Examplium.IdentityServer/Services/IEmailSender.cs add this line:
```
bool SendDeleteAccountEmailToUser(ApplicationUser user, string code, string returnUrl);
```

Examplium.IdentityServer/Services/EmailSender.cs add this code after the `SendChangePasswordEmailToUser()` method:
```
public bool SendDeleteAccountEmailToUser(ApplicationUser user, string code, string returnUrl)
{
    if (!string.IsNullOrEmpty(user.Email) && !string.IsNullOrEmpty(code))
    {
        var currentContext = _httpContextAccessor.HttpContext;
        string encodedCode = Uri.EscapeDataString(code);
        string codeLink = $"{currentContext?.Request.Scheme}://{currentContext?.Request.Host}/Account/DeleteAccount/ConfirmDelete?userId={user.Id}&code={encodedCode}&returnUrl={returnUrl}";

        string emailSubject = "Delete Account";
        string emailText = $"Please click the following link to delete your {ExampliumCoreConstants.OrganizationName} account: <a href='{codeLink}'>link</a>";
        return SendEmail(user.Email, emailSubject, emailText);
    }
    return false;
}
```

<br/>

### Add DeleteAccount folder

Add a new folder named "DeleteAccount" in Examplium.IdentityServer/Pages/Account/

<br/>

### Add DeleteAccountViewModel.cs

Add a new class file named "DeleteAccountViewModel.cs" in the DeleteAccount folder.

Replace the content with this code:
```
using System.ComponentModel.DataAnnotations;

namespace Examplium.IdentityServer.Pages.Account.DeleteAccount
{
    public class DeleteAccountViewModel
    {
        [Required] public string Email { get; set; } = string.Empty;
        [Required] public string Password { get; set; } = string.Empty;
        
        public string ReturnUrl { get; set; } = string.Empty;
        public string Button { get; set; } = string.Empty;
    }
}
```

<br/>

### Add Index Razor Page

Add an empty Razor page, name it "Index.cshtml".

Replace the content with this:
```
@page
@model Examplium.IdentityServer.Pages.Account.DeleteAccount.IndexModel
@{
}
<div class="login-page">
    <div class="lead">
        <h1>Delete Account</h1>
    </div>

    <partial name="_ValidationSummary" />

    <div class="row">

        <div class="col-sm-6">
            <div class="card">
                <div class="card-header">
                    <h2>Delete @Examplium.Shared.Constants.ExampliumCoreConstants.OrganizationName Account</h2>
                </div>

                <div class="card-body">
                    <div>Enter your email and password below and click "Delete my account" to confirm you want to delete your account.</div>
                    <form asp-page="/Account/DeleteAccount/Index">
                        <input type="hidden" asp-for="Input.ReturnUrl" />

                        <div class="form-group">
                            <label asp-for="Input.Email"></label>
                            <input class="form-control" placeholder="Email" asp-for="Input.Email">
                        </div>
                        <div class="form-group">
                            <label asp-for="Input.Password"></label>
                            <input type="password" class="form-control" placeholder="Password" asp-for="Input.Password" autocomplete="off">
                        </div>
                        <button class="btn btn-primary" name="Input.Button" value="deleteaccount">Delete my account</button>
                        <button class="btn btn-secondary" name="Input.Button" value="cancel">Cancel</button>
                    </form>
                </div>
                <div class="card-footer">
                    <div>
                        When you click "Delete my account" an email will be sent to your email address with further instructions.
                    </div>
                </div>
            </div>
        </div>

    </div>
</div>
```

Change the code behind file Index.cshtml.cs to this:
```
uusing Duende.IdentityServer.Extensions;
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
```

<br/>

## Add DeleteAccountEmailSent Razor Page

Add a blank Razor page in the DeleteAccount folder, give the name "DeleteAccountEmailSent.cshtml

Replace the content with this:

```
@page
@model Examplium.IdentityServer.Pages.Account.DeleteAccount.DeleteAccountEmailSentModel
@{
}
<div class="login-page">
    <div class="lead">
        <h1>Delete account email sent</h1>
    </div>
    <div class="row">
        <div>
            Please check your email for further instructions.
        </div>
    </div>
    <div class="row">
        <a href="@Model.View.ReturnUrl">Return</a>
    </div>
</div>
```
Change the code behind file's (DeleteAccountEmailSent.cshtml.cs) content to this code:

```
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Examplium.IdentityServer.Pages.Account.DeleteAccount
{
    [Authorize]
    [SecurityHeaders]
    public class DeleteAccountEmailSentModel : PageModel
    {
        [BindProperty] public DeleteAccountViewModel View { get; set; } = new DeleteAccountViewModel();

        public IActionResult OnGet(string returnUrl)
        {
            View = new DeleteAccountViewModel { ReturnUrl = returnUrl };

            return Page();
        }
    }
}
```

<br/>

### Add ConfirmDeleteAccountViewModel.cs

Add a new class file named "ConfirmDeleteAccountViewModel.cs" in the DeleteAccount folder.

Replace the content with this code:
```
namespace Examplium.IdentityServer.Pages.Account.DeleteAccount
{
    public class ConfirmDeleteAccountViewModel
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string ResetCode { get; set; } = string.Empty;
        public string ReturnUrl { get; set; } = string.Empty;
        public string Button { get; set; } = string.Empty;
    }
}
```

<br/>

### Add ConfirmDeleteAccount Razor Page

Add an empty Razor page in the DeleteAccount folder and name it "ConfirmDelete.cshtml", update the content with the following:
```
@page
@model Examplium.IdentityServer.Pages.Account.DeleteAccount.ConfirmDeleteModel
@{
}
<div class="login-page">
    <div class="lead">
        <h1>Delete Account</h1>
    </div>

    <partial name="_ValidationSummary" />

    <div class="row">

        <div class="col-sm-6">
            <div class="card">
                <div class="card-header">
                    <h2>Delete @Examplium.Shared.Constants.ExampliumCoreConstants.OrganizationName Account</h2>
                </div>

                <div class="card-body">
                    <form asp-page="/Account/DeleteAccount/ConfirmDelete">
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
                        <button class="btn btn-primary" name="Input.Button" value="deleteaccount">Delete my account</button>
                        <button class="btn btn-secondary" name="Input.Button" value="cancel">Cancel</button>
                    </form>
                </div>
            </div>
        </div>

    </div>
</div>
```
<br/>

Change the content of the code behind file ConfirmDelete.cshtml.cs to this:
```
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
```

<br/>

### Add AccountDeleted Razor Page

Add an empty Razor page in the DeleteAccount folder and name it "AccountDeleted.cshtml", update the content with the following:
```
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Examplium.IdentityServer.Pages.Account.DeleteAccount
{
    [AllowAnonymous]
    [SecurityHeaders]
    public class AccountDeletedModel : PageModel
    {
        [BindProperty] public string ReturnUrl { get; set; } = string.Empty;

        public void OnGet()
        {

        }
    }
}
```

Change the content of the code behind file AccountDeleted.cshtml.cs to this:
```
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Examplium.IdentityServer.Pages.Account.DeleteAccount
{
    [AllowAnonymous]
    [SecurityHeaders]
    public class AccountDeletedModel : PageModel
    {
        [BindProperty] public string ReturnUrl { get; set; } = string.Empty;

        public void OnGet(string returnUrl)
        {
            ReturnUrl = returnUrl;
        }
    }
}
```

<br/>

### Add link to DeleteAccount in the menu

Update the code block after the Privacy nav-item in Examplium.IdentityServer/Pages/Shared/\_Layout.cshtml with this code:
```
@if (User.Identity != null && User.Identity.IsAuthenticated)
{
    <li class="nav-item">
        @{
            string returnUrl = string.IsNullOrEmpty(Context.Request.Path) ? "~/" : Context.Request.GetEncodedUrl();
        }
        <a class="nav-link text-dark" asp-area="" asp-page="/Account/ChangePassword/Index" asp-route-returnUrl="@returnUrl">Change Password</a>
    </li>
    <li class="nav-item">
        <a class="nav-link text-dark" asp-area="" asp-page="/Account/DeleteAccount/Index" asp-route-returnUrl="@returnUrl">Delete My Account</a>
    </li>
}
```

<br/>
