﻿using Examplium.Shared.Models.Identity;

namespace Examplium.IdentityServer.Services
{
    public interface IEmailSender
    {
        bool SendEmail(string email, string subject, string message);
        bool SendConfirmationEmailToUser(ApplicationUser user, string code);
    }
}
