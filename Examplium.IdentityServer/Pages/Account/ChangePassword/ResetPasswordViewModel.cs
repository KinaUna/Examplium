﻿using System.ComponentModel.DataAnnotations;

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
