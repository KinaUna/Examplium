# Add Confirmation Email to IdentityServer

### Create new "Services" folder in Examplium.IdentityServer root folder.

<br/>

### Add "IEmailSender.cs" interface file.
Code:
```
using Examplium.Shared.Models.Identity;

namespace Examplium.IdentityServer.Services
{
    public interface IEmailSender
    {
        bool SendEmail(string email, string subject, string message);
        bool SendConfirmationEmailToUser(ApplicationUser user, string code, string returnUrl);
    }
}
```

<br/>

### Add "EmailSender.cs" class file.
Code:
```
using System.Net.Mail;
using System.Net;
using Examplium.Shared.Constants;
using Examplium.Shared.Models.Identity;

namespace Examplium.IdentityServer.Services
{
    public class EmailSender: IEmailSender
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EmailSender(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public bool SendEmail(string email, string subject, string message)
        {
            var emailServerString = _configuration["EmailServer"] ?? throw new InvalidOperationException("Configuration string 'EmailServer' not found.");
            SmtpClient emailClient = new SmtpClient(emailServerString);
            emailClient.UseDefaultCredentials = false;
            var emailUserNameString = _configuration["EmailUserName"] ?? throw new InvalidOperationException("Configuration string 'EmailUserName' not found.");
            var emailPasswordString = _configuration["EmailPassword"] ?? throw new InvalidOperationException("Configuration string 'EmailPassword' not found.");
            emailClient.Credentials = new NetworkCredential(emailUserNameString, emailPasswordString);
            emailClient.EnableSsl = true;
            emailClient.Port = 587;

            MailMessage mailMessage = new MailMessage();
            
            var emailFromString = _configuration["EmailFrom"] ?? throw new InvalidOperationException("Configuration string 'EmailFrom' not found.");
            mailMessage.From = new MailAddress(emailFromString, "Support - " + ExampliumCoreConstants.ApplicationName);
            mailMessage.To.Add(email);
            mailMessage.Body = message;
            mailMessage.IsBodyHtml = true;
            mailMessage.Subject = subject;

            try
            {
                emailClient.SendMailAsync(mailMessage);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool SendConfirmationEmailToUser(ApplicationUser user, string code, string returnUrl)
        {
            if(!string.IsNullOrEmpty(user.Email) && !string.IsNullOrEmpty(code))
            {
                var currentContext = _httpContextAccessor.HttpContext;
                string encodedCode = Uri.EscapeDataString(code);
                string codeLink = $"{currentContext?.Request.Scheme}://{currentContext?.Request.Host}/Account/Register/ConfirmEmail?userId={user.Id}&code={encodedCode}&returnUrl={returnUrl}";

                string emailSubject = "Confirm your email";
                string emailText = $"Please confirm your {ExampliumCoreConstants.ApplicationName} account's email address by clicking this link: <a href='{codeLink}'>link</a>";
                return SendEmail(user.Email, emailSubject, emailText);
            }
            
            return false;
        }
    }
}
```

<br/>

### Update User Secrets.
Add these four lines, replace the values with your email account settings:
```
    "EmailServer": "mail.example.com",
    "EmailUserName": "user@example.com",
    "EmailPassword": "Password",
    "EmailFrom": "user@example.com"
```
<br/>

### Register dependency injection for EmailSender.

In Examplium.IdentityServer/Program.cs add these lines after `builder.Services.AddAuthentication();`:
```
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
```

<br/>

### Update Pages/Account/Register/Index.cshtml.cs

In the OnPost() action, after checking createUserResult for errors add these lines:
```
string code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
if(_emailSender.SendConfirmationEmailToUser(user, code, Input.ReturnUrl))
{
   return RedirectToPage(pageName: "Registered", routeValues: new { Input.ReturnUrl });
}
```
<br/>

### Add Pages/Account/Register/ConfirmEmail.cshtml Razor page.

Add a new blank Razor page in the Register folder, update the content with this code:
```
@page
@model Examplium.IdentityServer.Pages.Account.Register.ConfirmEmailModel
@{
}
<div class="login-page">
    <div class="lead">
        <h1>Email address confirmed</h1>
    </div>
    <div class="row">
        <div>
            Your email address has been confirmed and your account is now active.
        </div>
        <div>
            You can now log in to @Examplium.Shared.Constants.ExampliumCoreConstants.ApplicationName
        </div>
    </div>
    <div class="row">
        <a asp-page="/Account/Login/Index" asp-route-returnUrl="@Model.View.ReturnUrl">Go to login</a>
    </div>
</div>
```

Update the code behind file, ConfirmEmail.cshtml.cs with the following code:
```
using Examplium.Shared.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Examplium.IdentityServer.Pages.Account.Register
{
    [SecurityHeaders]
    [AllowAnonymous]
    public class ConfirmEmailModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ConfirmEmailModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty] public RegisterViewModel View { get; set; } = new RegisterViewModel();

        public async Task<IActionResult> OnGet(string userId, string code, string returnUrl)
        {
            View = new RegisterViewModel { ReturnUrl = returnUrl };

            if (!string.IsNullOrEmpty(userId) &&  !string.IsNullOrEmpty(code))
            {
                ApplicationUser? user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    throw new ApplicationException($"Unable to load user with ID '{userId}'.");

                }

                if (user.EmailConfirmed)
                {
                    return Page();
                }

                IdentityResult confirmEmailCodeResult = await _userManager.ConfirmEmailAsync(user, code);
                if (confirmEmailCodeResult.Succeeded)
                {
                    return Page();
                }
            }            
            
            return RedirectToPage(pageName: "/Account/Login/Index", routeValues: new { View.ReturnUrl });
        }
    }
}
```

<br/>
