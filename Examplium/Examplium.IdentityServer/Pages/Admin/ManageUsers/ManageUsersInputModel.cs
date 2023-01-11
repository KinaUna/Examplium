namespace Examplium.IdentityServer.Pages.Admin.ManageUsers
{
    public class ManageUsersInputModel
    {
        public string RemoveAdminRoleId { get; set; } = string.Empty;
        public string RemoveAdminRoleEmail { get; set; } = string.Empty;
        public string AddAdminRoleEmail { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
    }
}
