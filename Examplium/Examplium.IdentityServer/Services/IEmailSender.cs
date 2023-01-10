using Examplium.Shared.Models.Identity;

namespace Examplium.IdentityServer.Services
{
    public interface IEmailSender
    {
        bool SendEmail(string email, string subject, string message);
        bool SendConfirmationEmailToUser(ApplicationUser user, string code, string returnUrl);
        bool SendPasswordResetEmailToUser(ApplicationUser user, string code, string returnUrl);
        bool SendChangePasswordEmailToUser(ApplicationUser user, string code, string returnUrl);
        bool SendDeleteAccountEmailToUser(ApplicationUser user, string code, string returnUrl);
    }
}
