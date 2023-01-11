using Examplium.Shared.Constants;
using Examplium.Shared.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Examplium.IdentityServer.Pages.Admin.ManageUsers
{
    [Authorize(Roles = ExampliumAuthServerConstants.AdminRole)]
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        
        public IndexModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty] public ManageUsersViewModel View { get; set; } = new ManageUsersViewModel();
        
        public async Task<IActionResult> OnGet()
        {
            View.Administrators = await _userManager.GetUsersInRoleAsync(ExampliumAuthServerConstants.AdminRole);

            return Page();

        }
    }
}
