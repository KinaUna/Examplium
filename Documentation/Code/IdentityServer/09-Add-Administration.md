# Add Identity Server Administration Features

<br/>

### Initial administrator

Add user name and password for the account creation to User Secrets:

Right click the Examplium.IdentityServer project, select "Manage User Secrets..".

Add these lines:
```
"AdminEmail": "user@example.com",
"AdminInitialPassword": "&IVKBGF2ghB#wrd@uL^KgKL#"
```

Update the Admin value with your email.

You can update the AdminInitalPassword value too, or use the password reset function to change it when running the app later.

### Refactor IdentityServerInitialization.cs

Adding a user and roles will require access to UserManager and RoleManager, so we will move the initialization to a service where these can be accessed via dependency injection. The reset methods will also be needed for the management pages, so if we move it to a service we can reuse it later.

In the Examplium.IdentityServer/Services folder add a new interface file named "IDatabaseInitializer.cs" and a class file named "DatabaseInitializer.cs"

Put this code in IDatabaseInitializer.cs:
```
namespace Examplium.IdentityServer.Services
{
    public interface IDatabaseInitializer
    {
        void InitializeDatabase();
        void AddDefaultClients();
        void ResetClients();
        void AddDefaultIdentityResources();
        void ResetIdentityResources();
        void AddDefaultApiScopes();
        void ResetApiScopes();
    }
}
```

<br/>

And this code for DatabaseInitializer.cs:
```
using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Entities;
using Duende.IdentityServer.EntityFramework.Mappers;
using Examplium.IdentityServer.Data;
using Examplium.Shared.Constants;
using Examplium.Shared.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Examplium.IdentityServer.Services
{
    public class DatabaseInitializer: IDatabaseInitializer
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly PersistedGrantDbContext _persistedGrantDbContext;
        private readonly ConfigurationDbContext _configurationDbContext;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DatabaseInitializer(IConfiguration configuration, ConfigurationDbContext configurationDbContext, PersistedGrantDbContext persistedGrantDbContext, ApplicationDbContext applicationDbContext,
            UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _configuration = configuration;
            _configurationDbContext = configurationDbContext;
            _applicationDbContext = applicationDbContext;
            _persistedGrantDbContext = persistedGrantDbContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void InitializeDatabase()
        {
            MigrateDatabases();

            AddDefaultClients();

            AddDefaultIdentityResources();

            AddDefaultApiScopes();

            AddDefaultRoles();

            AddDefaultAdminUser();
        }

        private void MigrateDatabases()
        {
            _applicationDbContext.Database.Migrate();
            _configurationDbContext.Database.Migrate();
            _persistedGrantDbContext.Database.Migrate();
        }

        public void AddDefaultClients()
        {
            var coreApiSecretString = _configuration["CoreApiSecret"] ?? throw new InvalidOperationException("Configuration string 'CoreApiSecret' not found.");
            if (!_configurationDbContext.Clients.Any())
            {
                foreach (var client in IdentityServerConfiguration.Clients(coreApiSecretString))
                {
                    _configurationDbContext.Clients.Add(client.ToEntity());
                }

                _configurationDbContext.SaveChanges();
            }
        }

        public void ResetClients()
        {
            List<Client> existingClients = _configurationDbContext.Clients.ToList();
            foreach (Client client in existingClients)
            {
                _configurationDbContext.Clients.Remove(client);
            }

            _configurationDbContext.SaveChanges();

            AddDefaultClients();
        }

        public void AddDefaultIdentityResources()
        {
            if (!_configurationDbContext.IdentityResources.Any())
            {
                foreach (var identityResource in IdentityServerConfiguration.IdentityResources)
                {
                    _configurationDbContext.IdentityResources.Add(identityResource.ToEntity());
                }

                _configurationDbContext.SaveChanges();
            }
        }

        public void ResetIdentityResources()
        {
            List<IdentityResource> existingIdentityResources = _configurationDbContext.IdentityResources.ToList();
            foreach (IdentityResource identityResource in existingIdentityResources)
            {
                _configurationDbContext.IdentityResources.Remove(identityResource);
            }

            _configurationDbContext.SaveChanges();

            AddDefaultIdentityResources();
        }

        public void AddDefaultApiScopes()
        {
            if (!_configurationDbContext.ApiScopes.Any())
            {
                foreach (var apiScope in IdentityServerConfiguration.ApiScopes)
                {
                    _configurationDbContext.ApiScopes.Add(apiScope.ToEntity());
                }

                _configurationDbContext.SaveChanges();
            }
        }

        public void ResetApiScopes()
        {
            List<ApiScope> existingApiScopes = _configurationDbContext.ApiScopes.ToList();
            foreach (ApiScope apiScopeToDelete in existingApiScopes)
            {
                _configurationDbContext.ApiScopes.Remove(apiScopeToDelete);
            }

            _configurationDbContext.SaveChanges();

            AddDefaultApiScopes();
        }

        private void AddDefaultRoles()
        {
            if (_roleManager.FindByNameAsync(ExampliumAuthServerConstants.AdminRole).Result == null)
            {
                _roleManager.CreateAsync(new IdentityRole(ExampliumAuthServerConstants.AdminRole)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(ExampliumAuthServerConstants.DefaultRole)).GetAwaiter().GetResult();
            }
        }

        private void AddDefaultAdminUser()
        {
            var emailString = _configuration["AdminEmail"] ?? throw new InvalidOperationException("Configuration string 'AdminEmail' not found.");

            if(_userManager.FindByEmailAsync(emailString).GetAwaiter().GetResult() == null)
            {
                var passwordString = _configuration["AdminInitialPassword"] ?? throw new InvalidOperationException("Configuration string 'AdminInitialPassword' not found.");

                ApplicationUser adminUser = new ApplicationUser()
                {
                    Email = emailString,
                    UserName = emailString,
                    EmailConfirmed = true
                };

                _userManager.CreateAsync(adminUser, passwordString).GetAwaiter().GetResult();

                _userManager.AddToRoleAsync(adminUser, ExampliumAuthServerConstants.AdminRole).GetAwaiter().GetResult();
            }
            
        }
    }
}
```

<br/>

Add these two lines to Examplium.Shared/Constants/ExampliumAuthServerConstants.cs:
```
public const string DefaultRole = "Default";
public const string AdminRole = "Admin";
```

<br/>

Change the content of Examplium.IdentityServer/Data/IdentityServerInitialization.cs to this:
```
using Examplium.IdentityServer.Services;

namespace Examplium.IdentityServer.Data
{
    public static class IdentityServerInitialization
    {

        public static void InitializeDatabase(WebApplication app)
        {
            using var serviceScope = app.Services.GetService<IServiceScopeFactory>()?.CreateScope();
            
            var databaseInitializer = serviceScope?.ServiceProvider.GetRequiredService<IDatabaseInitializer>();
            
            databaseInitializer?.InitializeDatabase();
        }
    }
}
```
<br/>

### Update Program.cs

Just above "var app = builder.Build();" add this line:
```
builder.Services.AddScoped<IDatabaseInitializer, DatabaseInitializer>();
```
Update the IdentityServerInitialization.InitializeDatabase() line to this:
```
IdentityServerInitialization.InitializeDatabase(app);
```

<br/>

### Add Admin Index page

In Examplium.IdentityServer/Pages/ create a new folder named "Admin".

Add a blank razor page with the name "Index.cshtml" in the newly created folder.
Put this code in the file:
```
@page
@model Examplium.IdentityServer.Pages.Admin.IndexModel
@{
}
<div class="login-page">
    <div class="lead">
        <h3>Administration</h3>
    </div>
    
    <div class="row">
        <div class="mt-2 mb-2">
            <a asp-page="/Admin/ManageUsers/Index">Manage users</a>
        </div>
        <div class="mt-2 mb-2">
            <a asp-page="/Admin/ResetConfiguration/Index">Reset configuration</a>
        </div>

    </div>
</div>
```

Update the code behind file with the Authorize attribute like this:
```
using Examplium.Shared.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Examplium.IdentityServer.Pages.Admin
{
    [Authorize(Roles = ExampliumAuthServerConstants.AdminRole)]
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}

```

<br/>

### Add Manage Users page

Add a new folder in the Admin folder, name it "ManageUsers".

Add a new class in this folder, name it "ManageUsersViewModel.cs" and update the code:
```
using Examplium.Shared.Models.Identity;

namespace Examplium.IdentityServer.Pages.Admin.ManageUsers
{
    public class ManageUsersViewModel
    {
        public IList<ApplicationUser> Administrators { get; set; } = new List<ApplicationUser>();
    }
}
```

Add another class, name it "ManageUsersInputModel.cs" with this content:
```
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
```

Add a new blank razor page with the name "Index.cshtml".
Code:
```
@page
@model Examplium.IdentityServer.Pages.Admin.ManageUsers.IndexModel
@{
}
<div class="login-page">
    <div class="lead">
        <h3>Manage Users</h3>
    </div>

    <div class="row">
        <div class="mt-2 mb-2">
            <h5>Administrators</h5>
            <table class="table">
                <thead>
                <tr>
                    <th>Email</th>
                    <th>Action</th>
                </tr>
                </thead>
                <tbody>
                @foreach (var applicationUser in Model.View.Administrators)
                {
                    <tr>
                        <td>@applicationUser.Email</td>
                        <td>
                            <a asp-page="/Admin/ManageUsers/RemoveAdminRoleFromUser" asp-route-userId="@applicationUser.Id">Remove administrator role</a>
                        </td>
                    </tr>
                }
                </tbody>
            </table>
            
        </div>
        <div class="mt-2 mb-2">
            <h5>Add administrator</h5>
            <div class="card">

                <div class="card-header">
                    Add admin role to a user
                </div>
                <div class="card-body">
                    <a asp-page="/Admin/ManageUsers/AddAdminRoleToUser">Add administrator role</a>
                </div>
            </div>
        </div>

        <div class="mt-2 mb-2">
            <a asp-page="/Admin/Index">Return to administration</a>
        </div>

    </div>
</div>
```

Update the code behind content to this:

```
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
```

Add another blank razor page with the name "AddAdminRoleToUser.cshtml", update the content to this:
```
@page
@model Examplium.IdentityServer.Pages.Admin.ManageUsers.AddAdminRoleToUserModel
@{
}
<div class="login-page">
    <div class="lead">
        <h3>Manage Users - Add admin role to user</h3>
    </div>
    @if (string.IsNullOrEmpty(Model.Input.ErrorMessage))
    {
        <div class="row">
            <div class="card m-0 p-0">
                <div class="card-header">
                    <h5>Enter the email address of the user</h5>
                </div>
                <div class="card-body p-5">
                    <form asp-page="/Admin/ManageUsers/AddAdminRoleToUser">
                        <div class="form-group">
                            <input class="form-control" placeholder="Email" asp-for="Input.AddAdminRoleEmail"/>
                        </div>
                        <button type="submit" class="btn btn-primary mt-2">Add administrator role</button>
                    </form>
                </div>
                <div class="card-footer">
                    <div class="mt-2 mb-2">
                        <a asp-page="/Admin/ManageUsers/Index">Return to manager users page</a>
                    </div>
                    <div class="mt-2 mb-2">
                        <a asp-page="/Admin/Index">Return to administration page</a>
                    </div>
                </div>
            </div>
        </div>
    }
    else
    {
        <div class="row">
            <div class="mt-2 mb-2">
                <h4>@Model.Input.ErrorMessage</h4>
            </div>
            <div class="mt-2 mb-2">
                <a asp-page="/Admin/ManageUsers/Index">Return to manager users page</a>
            </div>
            <div class="mt-2 mb-2">
                <a asp-page="/Admin/Index">Return to administration page</a>
            </div>
        </div>
    }
    
</div>
```

Replace the content of the code behind file, "AddAdminRoleToUser.cshtml.cs" with this:
```
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
```

Add a blank razor page again, name it "RemoveAdminRoleFromUser.cshtml", and replace the file content with this:
```
@page
@model Examplium.IdentityServer.Pages.Admin.ManageUsers.RemoveAdminRoleFromUserModel
@{
}
<div class="login-page">
    <div class="lead">
        <h3>Manage Users - Remove admin role from user</h3>
    </div>

    <div class="row">
        @if (string.IsNullOrEmpty(Model.Input.ErrorMessage))
        {
            <div class="row">
                <div class="card m-0 p-0">
                    <div class="card-header">
                        <h5>Remove Admin role from this user?</h5>
                    </div>
                    <div class="card-body p-5">
                        <form asp-page="/Admin/ManageUsers/RemoveAdminRoleFromUser">
                            <input type="hidden" asp-for="Input.RemoveAdminRoleEmail"/>
                            <input type="hidden" asp-for="Input.RemoveAdminRoleId"/>
                            <div class="form-group">
                                <h5>Email: @Model.Input.RemoveAdminRoleEmail</h5>
                            </div>
                            <button type="submit" class="btn btn-primary mt-2">Yes - Remove administrator role</button>
                        </form>
                    </div>
                    <div class="card-footer">
                        <div class="mt-2 mb-2">
                            <a asp-page="/Admin/ManageUsers/Index">Return to manager users page</a>
                        </div>
                        <div class="mt-2 mb-2">
                            <a asp-page="/Admin/Index">Return to administration page</a>
                        </div>
                    </div>
                </div>
            </div>
        }
        else
        {
            <div class="row">
                <div class="mt-2 mb-2">
                    <h4>@Model.Input.ErrorMessage</h4>
                </div>
                <div class="mt-2 mb-2">
                    <a asp-page="/Admin/ManageUsers/Index">Return to manager users page</a>
                </div>
                <div class="mt-2 mb-2">
                    <a asp-page="/Admin/Index">Return to administration page</a>
                </div>
            </div>
        }
    </div>
</div>
```

Then replace the code behind file content with the following:
```
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
```
<br/>

### Add Reset Configuration page

Add a new folder in Examplium.IdentityServer/Pages/Admin/, name it "ResetConfiguration".

Add an empty razor page with the name "Index.cshtml".

Update the content with this:
```
@page
@model Examplium.IdentityServer.Pages.Admin.ResetConfiguration.IndexModel
@{
}
<div class="login-page">
    <div class="lead">
        <h3>Reset Identity Server Configuration</h3>
    </div>
    @if (!string.IsNullOrEmpty(Model.Reset))
    {
        <div class="bg-danger m-3 p-3 text-white">
            @Model.Reset have been reset.
        </div>
    }
    <div class="row">
        <div class="mt-2 mb-2">
            <h5>Clients</h5>
            <a asp-page="/Admin/ResetConfiguration/Clients">Reset clients</a>
        </div>
        <div class="mt-2 mb-2">
            <h5>Identity Resources</h5>
            <a asp-page="/Admin/ResetConfiguration/IdentityResources">Reset identity resources</a>
        </div>
        <div class="mt-2 mb-2">
            <h5>Api Scopes</h5>
            <a asp-page="/Admin/ResetConfiguration/ApiScopes">Reset api scopes</a>
        </div>
        <div class="mt-2 mb-2">
            <h5>All configuration settings</h5>
            <a asp-page="/Admin/ResetConfiguration/All">Reset all</a>
        </div>

        <div class="mt-2 mb-2">
            <a asp-page="/Admin/Index">Return to administration</a>
        </div>

    </div>
</div>
```

Replace the content of the code behind file with this:
```
using Examplium.Shared.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Examplium.IdentityServer.Pages.Admin.ResetConfiguration
{
    [Authorize(Roles = ExampliumAuthServerConstants.AdminRole)]
    public class IndexModel : PageModel
    {
        [BindProperty] public string Reset { get; set; } = string.Empty;
        public void OnGet(string? reset)
        {
            Reset = reset ?? string.Empty;
        }
    }
}
```

Add a new empty razor page with the name "Clients.cshtml", update the code:
```
@page
@model Examplium.IdentityServer.Pages.Admin.ResetConfiguration.ClientsModel
@{
}
<div class="login-page">
    <div class="lead">
        <h3>Reset Identity Server Clients</h3>
    </div>

    <div class="row">
        <div class="card m-0 p-0">
            <div class="card-header">
                <h5>Are you sure you want to reset clients?</h5>
            </div>
            <div class="card-body p-5">
                <form asp-page="/Admin/ResetConfiguration/Clients">
                    <button type="submit" class="btn btn-primary mt-2">Reset clients</button>
                </form>
            </div>
            <div class="card-footer">
                <div class="mt-2 mb-2">
                    <a asp-page="/Admin/ResetConfiguration/Index">Return to reset configuration page</a>
                </div>
                <div class="mt-2 mb-2">
                    <a asp-page="/Admin/Index">Return to administration page</a>
                </div>
            </div>
        </div>
    </div>
</div>
```

Replace the content of the code behind file Clients.cshtml.cs with this:
```
using Examplium.Shared.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Examplium.IdentityServer.Services;

namespace Examplium.IdentityServer.Pages.Admin.ResetConfiguration
{
    [Authorize(Roles = ExampliumAuthServerConstants.AdminRole)]
    public class ClientsModel : PageModel
    {
        private readonly IDatabaseInitializer _databaseInitializer;

        public ClientsModel(IDatabaseInitializer databaseInitializer)
        {
            _databaseInitializer = databaseInitializer;
        }
        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            _databaseInitializer.ResetClients();

            return RedirectToPage("/Admin/ResetConfiguration/Index", new {reset = "Clients"});
        }
    }
}
```

Add a new empty razor page with the name "IdentityResources.cshtml", update the code:
```
@page
@model Examplium.IdentityServer.Pages.Admin.ResetConfiguration.IdentityResourcesModel
@{
}
<div class="login-page">
    <div class="lead">
        <h3>Reset Identity Server Identity Resources</h3>
    </div>

    <div class="row">
        <div class="card m-0 p-0">
            <div class="card-header">
                <h5>Are you sure you want to reset identity resources?</h5>
            </div>
            <div class="card-body p-5">
                <form asp-page="/Admin/ResetConfiguration/IdentityResources">
                    <button type="submit" class="btn btn-primary mt-2">Reset identity resources</button>
                </form>
            </div>
            <div class="card-footer">
                <div class="mt-2 mb-2">
                    <a asp-page="/Admin/ResetConfiguration/Index">Return to reset configuration page</a>
                </div>
                <div class="mt-2 mb-2">
                    <a asp-page="/Admin/Index">Return to administration page</a>
                </div>
            </div>
        </div>
    </div>
</div>
```

Replace the content of the code behind file with this:
```
using Examplium.IdentityServer.Services;
using Examplium.Shared.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Examplium.IdentityServer.Pages.Admin.ResetConfiguration
{
    [Authorize(Roles = ExampliumAuthServerConstants.AdminRole)]
    public class IdentityResourcesModel : PageModel
    {
        private readonly IDatabaseInitializer _databaseInitializer;

        public IdentityResourcesModel(IDatabaseInitializer databaseInitializer)
        {
            _databaseInitializer = databaseInitializer;
        }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            _databaseInitializer.ResetIdentityResources();

            return RedirectToPage("/Admin/ResetConfiguration/Index", new { reset = "Identity Resources" });
        }
    }
}
```

Add a new empty razor page with the name "ApiScopes.cshtml", update the code:
```
@page
@model Examplium.IdentityServer.Pages.Admin.ResetConfiguration.ApiScopesModel
@{
}
<div class="login-page">
    <div class="lead">
        <h3>Reset Identity Server Api Scopes</h3>
    </div>

    <div class="row">
        <div class="card m-0 p-0">
            <div class="card-header">
                <h5>Are you sure you want to reset api scopes?</h5>
            </div>
            <div class="card-body p-5">
                <form asp-page="/Admin/ResetConfiguration/ApiScopes">
                    <button type="submit" class="btn btn-primary mt-2">Reset api scopes</button>
                </form>
            </div>
            <div class="card-footer">
                <div class="mt-2 mb-2">
                    <a asp-page="/Admin/ResetConfiguration/Index">Return to reset configuration page</a>
                </div>
                <div class="mt-2 mb-2">
                    <a asp-page="/Admin/Index">Return to administration page</a>
                </div>
            </div>
        </div>
    </div>
</div>
```

Replace the content of the code behind file with this:
```
using Examplium.IdentityServer.Services;
using Examplium.Shared.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Examplium.IdentityServer.Pages.Admin.ResetConfiguration
{
    [Authorize(Roles = ExampliumAuthServerConstants.AdminRole)]
    public class ApiScopesModel : PageModel
    {
        private readonly IDatabaseInitializer _databaseInitializer;

        public ApiScopesModel(IDatabaseInitializer databaseInitializer)
        {
            _databaseInitializer = databaseInitializer;
        }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            _databaseInitializer.ResetApiScopes();

            return RedirectToPage("/Admin/ResetConfiguration/Index", new { reset = "Api Scopes" });
        }
    }
}
```

Add a new empty razor page with the name "All.cshtml", update the code:
```
@page
@model Examplium.IdentityServer.Pages.Admin.ResetConfiguration.AllModel
@{
}
<div class="login-page">
    <div class="lead">
        <h3>Reset All Identity Server Configurations </h3>
    </div>

    <div class="row">
        <div class="card m-0 p-0">
            <div class="card-header">
                <h5>Are you sure you want to reset all configurations?</h5>
            </div>
            <div class="card-body p-5">
                <form asp-page="/Admin/ResetConfiguration/All">
                    <button type="submit" class="btn btn-primary mt-2">Reset all</button>
                </form>
            </div>
            <div class="card-footer">
                <div class="mt-2 mb-2">
                    <a asp-page="/Admin/ResetConfiguration/Index">Return to reset configuration page</a>
                </div>
                <div class="mt-2 mb-2">
                    <a asp-page="/Admin/Index">Return to administration page</a>
                </div>
            </div>
        </div>
    </div>
</div>
```

Replace the content of the code behind file with this:
```
using Examplium.IdentityServer.Services;
using Examplium.Shared.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Examplium.IdentityServer.Pages.Admin.ResetConfiguration
{
    [Authorize(Roles = ExampliumAuthServerConstants.AdminRole)]
    public class AllModel : PageModel
    {
        private readonly IDatabaseInitializer _databaseInitializer;

        public AllModel(IDatabaseInitializer databaseInitializer)
        {
            _databaseInitializer = databaseInitializer;
        }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            _databaseInitializer.ResetClients();
            _databaseInitializer.ResetIdentityResources();
            _databaseInitializer.ResetApiScopes();

            return RedirectToPage("/Admin/ResetConfiguration/Index", new { reset = "All configurations" });
        }
    }
}
```
<br/>

### Update \_Layout.cshtml with link to Admin/Index page

Open Examplium.IdentityServer/Pages/Shared/\_Layout.cshtml

Just above the last `</ul>` closing tag add the following code:
```
@if (User.IsInRole(ExampliumAuthServerConstants.AdminRole))
{
    <li class="nav-item">
        <a class="nav-link text-dark" asp-area="" asp-page="/Admin/Index">Administration</a>
    </li>
}
```

<br/>
