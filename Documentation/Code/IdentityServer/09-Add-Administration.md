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
        <h1>Administration</h1>
    </div>
    
    <div class="row">
        <div><a asp-page="/Admin/ManageUsers/">Manage users</a></div>
        <div><a asp-page="/Admin/ResetConfiguration/">Reset configuration</a></div>

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

<br/>

### Add Reset Configuration page

<br/>

