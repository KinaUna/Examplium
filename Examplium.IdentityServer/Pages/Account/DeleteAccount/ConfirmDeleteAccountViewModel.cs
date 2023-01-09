namespace Examplium.IdentityServer.Pages.Account.DeleteAccount
{
    public class ConfirmDeleteAccountViewModel
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string ResetCode { get; set; } = string.Empty;
        public string ReturnUrl { get; set; } = string.Empty;
        public string Button { get; set; } = string.Empty;
    }
}
