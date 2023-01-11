using Duende.IdentityServer.Extensions;
using Examplium.Shared.Constants;
using Examplium.Shared.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Examplium.IdentityServer.Pages.Admin.ManageUsers
{
    [Authorize(Roles = ExampliumAuthServerConstants.AdminRole)]
    public class RemoveAdminRoleFromUserModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public RemoveAdminRoleFromUserModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty] public ManageUsersInputModel Input { get; set; } = new ManageUsersInputModel();

        public async Task<IActionResult> OnGet(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                ApplicationUser? user = await _userManager.FindByIdAsync(userId);
            
                if (user != null && !string.IsNullOrEmpty(user.Email)) 
                {
                    if (User.Identity.GetSubjectId() == userId)
                    {
                        Input.ErrorMessage = "Error: You cannot remove the admin role from your own account.";
                    }
                    Input.RemoveAdminRoleId = userId; 
                    Input.RemoveAdminRoleEmail = user.Email;
                }
            }
            
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (!string.IsNullOrEmpty(Input.RemoveAdminRoleId))
            {
                ApplicationUser? user = await _userManager.FindByIdAsync(Input.RemoveAdminRoleId);
                if (user != null && string.Equals(user.Email, Input.RemoveAdminRoleEmail, StringComparison.OrdinalIgnoreCase))
                {
                    await _userManager.RemoveFromRoleAsync(user, ExampliumAuthServerConstants.AdminRole);
                    return RedirectToPage("/Admin/ManageUsers/Index");
                }
            }

            Input.ErrorMessage = "Error: User not found.";
            return Page();
        }
    }
}
