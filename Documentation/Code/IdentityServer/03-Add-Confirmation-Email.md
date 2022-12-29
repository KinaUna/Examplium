# Add Confirmation Email to IdentityServer

### Create new "Services" folder in Examplium.IdentityServer root folder.

<br/>

### Add "IEmailSender.cs" interface file.
Code:
```
namespace Examplium.IdentityServer.Services
{
    public interface IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message);
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

namespace Examplium.IdentityServer.Services
{
    public class EmailSender: IEmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task SendEmailAsync(string email, string subject, string message)
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
                return Task.CompletedTask;
            }
            catch
            {
                return Task.FromResult(0);
            }
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

In Examplium.IdentityServer/Program.cs add this line after `builder.Services.AddAuthentication();`:
```
builder.Services.AddTransient<IEmailSender, EmailSender>();
```

<br/>

Todo: Create code, create link with code, update Register code to send the email with link.
