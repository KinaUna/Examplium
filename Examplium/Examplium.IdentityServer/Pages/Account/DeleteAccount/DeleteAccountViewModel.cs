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
