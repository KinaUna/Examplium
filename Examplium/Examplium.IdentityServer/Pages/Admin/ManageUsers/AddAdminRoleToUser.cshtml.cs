using Examplium.Shared.Constants;
using Examplium.Shared.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Examplium.IdentityServer.Pages.Admin.ManageUsers
{
    [Authorize(Roles = ExampliumAuthServerConstants.AdminRole)]
    public class AddAdminRoleToUserModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AddAdminRoleToUserModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty] public ManageUsersInputModel Input { get; set; } = new ManageUsersInputModel();

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            Console.WriteLine("Input.AddAdminRoleEmail: " + Input.AddAdminRoleEmail);
            if (!string.IsNullOrEmpty(Input.AddAdminRoleEmail))
            {
                ApplicationUser? user = await _userManager.FindByEmailAsync(Input.AddAdminRoleEmail);
                if (user != null)
                {
                    await _userManager.AddToRoleAsync(user, ExampliumAuthServerConstants.AdminRole);
                    return RedirectToPage("/Admin/ManageUsers/Index");
                }
            }

            Input.ErrorMessage = "Error: User not found.";
            return Page();
        }
    }
}
