using System.ComponentModel.DataAnnotations;

namespace Examplium.IdentityServer.Pages.Account.Register
{
    public class RegisterViewModel
    {
        [Required] public string Email { get; set; }
        [Required] public string Password { get; set; }
        [Required] public string ConfirmPassword { get; set; }

        public string ReturnUrl { get; set; }
        public string Button { get; set; }
    }
}
