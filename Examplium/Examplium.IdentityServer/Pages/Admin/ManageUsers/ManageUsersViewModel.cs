using Examplium.Shared.Models.Identity;

namespace Examplium.IdentityServer.Pages.Admin.ManageUsers
{
    public class ManageUsersViewModel
    {
        public IList<ApplicationUser> Administrators { get; set; } = new List<ApplicationUser>();
    }
}
