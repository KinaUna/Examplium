using System.ComponentModel.DataAnnotations;

namespace Examplium.IdentityServer.Pages.Account.ForgotPassword
{
    public class ForgotPasswordViewModel
    {
        [Required] public string Email { get; set; } = string.Empty;
        
        public string Button { get; set;} = string.Empty;
        public string ReturnUrl { get; set; } = string.Empty;
        
    }
}
