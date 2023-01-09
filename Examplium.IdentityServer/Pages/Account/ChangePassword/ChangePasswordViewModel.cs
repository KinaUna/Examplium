using System.ComponentModel.DataAnnotations;

namespace Examplium.IdentityServer.Pages.Account.ChangePassword
{
    public class ChangePasswordViewModel
    {
        [Required] public string Email { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string ResetCode { get; set; } = string.Empty;
        public string Button { get; set; } = string.Empty;
        public string ReturnUrl { get; set; } = string.Empty;
    }
}
