using System.Net.Mail;
using System.Net;
using Examplium.Shared.Constants;
using Examplium.Shared.Models.Identity;
using System.Text.Encodings.Web;

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

        public bool SendConfirmationEmailToUser(ApplicationUser user, string code)
        {
            if(!string.IsNullOrEmpty(user.Email) && !string.IsNullOrEmpty(code))
            {
                var currentContext = _httpContextAccessor.HttpContext;
                string encodedCode = Uri.EscapeDataString(code);
                string codeLink = $"{currentContext?.Request.Scheme}://{currentContext?.Request.Host}/Account/Register/ConfirmEmail?userId={user.Id}&code={encodedCode}";

                string emailSubject = "Confirm your email";
                string emailText = $"Please confirm your {ExampliumCoreConstants.ApplicationName} account's email address by clicking this link: <a href='{codeLink}'>link</a>";
                return SendEmail(user.Email, emailSubject, emailText);
            }
            
            return false;
        }
    }
}
