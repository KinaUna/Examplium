# Adding Duende Identity Server (V6)

This document describes setting up the foundations for the Identity Server project.

More documentation for Duende Identity Server can be found here: https://docs.duendesoftware.com/identityserver/v6/overview/

<br/>

## Add a new project to the solution.

- Right click on the solution, select "Add", then click on "New project..."
- Select ASP.NET Core Web app.
- Type in the name of the project: "Examplium.IdentityServer", click next.
- Use the default settings (Framework: .NET 7.0, Authentication Type: None, Configure for HTTPS: checked, all others unchecked)

## Install NuGet Packages

### Add Duende Identity Server packages.

- Duende.IdentityServer
- Duende.IdentityServer.AspNetIdentity
- Duende.IdentityServer.EntityFramework
- Duende.IdentityServer.EntityFramework.Storage

### Add EntityFramework packages

- Microsoft.EntityFrameworkCore
- Microsoft.EntityFrameworkCore.Design
- Microsoft.EntityFrameworkCore.SqlServer

### Add Identity package

- Microsoft.AspNetCore.Identity.EntityFrameworkCore

<br/>

### Constants

If it doesn't exist, in the Examplium.Shared project add a new folder named "Constants".

Add a new class with the name "ExampliumAuthServerConstants.cs", and replace the content with this code:

```
namespace Examplium.Shared.Constants
{
    public static class ExampliumAuthServerConstants
    {
        public const string WebServerUrlDebug = "https://localhost:7128";

        public const string CoreApiName = "Examplium.CoreApi";
    }
}
```

You may need to change the port number in WebServerUrlDebug string. 

Look in the Examplium.IdentityServer/Properties/launchSettings.json file for information about the port(s) used in your project.

<br/>

## Data folder

Add a new folder at the top level, name it "Data".

<br/>

### ApplicationDbContext

##### This defines the database used by identity server.

<br/>

Add a new class in the "Data" folder, name it ApplicationDbContext.cs

In the ApplicationDbContext.cs file update the code so it looks like this:

```
using Examplium.Shared.Models.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Examplium.IdentityServer.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
```

<br/>


### IdentityServerConfiguration

##### This class is used for configuring what data Identity Server will use for users, what APIs are managed by IdentityServer, and how and what clients can access.

<br/>

Add a new class with the file name "IdentityServerConfiguration.cs" and add this content:

```
using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using Examplium.Shared.Constants;

namespace Examplium.IdentityServer.Data
{
    public static class IdentityServerConfiguration
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId()
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope(ExampliumAuthServerConstants.CoreApiName, "Examplium Core API"),
            };

        public static IEnumerable<Client> Clients(string coreApiSecret) =>
            new Client[]
            {
                new Client
                {
                    ClientId = "Examplium.WebServer.Debug",
                    ClientSecrets = { new Secret(coreApiSecret.Sha256()) },

                    AllowedGrantTypes = GrantTypes.Code,

                    // where to redirect to after login
                    RedirectUris = { ExampliumAuthServerConstants.WebServerUrlDebug + "/signin-oidc" },

                    // where to redirect to after logout
                    PostLogoutRedirectUris = { ExampliumAuthServerConstants.WebServerUrlDebug + "/signout-callback-oidc" },

                    AllowOfflineAccess = true,

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        ExampliumAuthServerConstants.CoreApiName,
                    }
                }
            };
    }
}
```

<br/>>

### IdentityServerInitialization

##### This class ensures that the configuration is stored in the database.

##### Note1: Initial migrations and updates to the database still needs to be done manually, see Initial Database Migrations below.

##### Note2: This is only ensures the initial configuration is applied, if you make changes IdentityServerConfiguration.cs after running the app the first time, they will not be applied automatically. Later we will update that to make sure changes are applied properly too.

<br/>

Add a new class with the file name "IdentityServerInitialization.cs" and add this content:
```
using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Mappers;
using Microsoft.EntityFrameworkCore;

namespace Examplium.IdentityServer.Data
{
    public static class IdentityServerInitialization
    {
        public static void InitializeDatabase(WebApplication app, string coreApiSecretString)
        {
            using var serviceScope = app.Services.GetService<IServiceScopeFactory>()?.CreateScope();
            serviceScope?.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();
            serviceScope?.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database.Migrate();
            
            var context = serviceScope?.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
            context?.Database.Migrate();
            if (context != null && !context.Clients.Any())
            {
                foreach (var client in IdentityServerConfiguration.Clients(coreApiSecretString))
                {
                    context.Clients.Add(client.ToEntity());
                }

                context.SaveChanges();
            }

            if (context != null && !context.IdentityResources.Any())
            {
                foreach (var resource in IdentityServerConfiguration.IdentityResources)
                {
                    context.IdentityResources.Add(resource.ToEntity());
                }

                context.SaveChanges();
            }

            if (context != null && !context.ApiScopes.Any())
            {
                foreach (var resource in IdentityServerConfiguration.ApiScopes)
                {
                    context.ApiScopes.Add(resource.ToEntity());
                }

                context.SaveChanges();
            }
        }
    }
}
```

## Add database connectionstring and secret code for API

Right click the project name "Examplium.IdentityServer", select "Manage User Secrets".

Update the file with this content:
```
{
    "DefaultDatabaseConnection": "Server=(localdb)\\mssqllocaldb;Database=Examplium.IdentityServer;Trusted_Connection=True;MultipleActiveResultSets=true",
    "CoreApiSecret": "[Enter your own code here]"
}
```

<br/>

## Update Progam.cs

### Database setup

Add the following code to Program.cs, after `var builder = WebApplication.CreateBuilder(args);`:
```
var migrationsAssembly = typeof(Program).Assembly.GetName().Name;
var connectionString = builder.Configuration["DefaultDatabaseConnection"] ?? throw new InvalidOperationException("Configuration string 'DefaultDatabaseConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString, sql =>
    {
        sql.MigrationsAssembly(migrationsAssembly);
        sql.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
    }));
```

<br/>

### Add identity configuration

Then add this code 
```
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
```

<br/>

### Add and configure Identity Server, with caching

Next, add this code:
```
builder.Services.AddDistributedMemoryCache();

var coreApiSecretString = builder.Configuration["CoreApiSecret"] ?? throw new InvalidOperationException("Configuration string 'CoreApiSecret' not found.");

builder.Services.AddIdentityServer(options =>
    {
        options.Events.RaiseErrorEvents = true;
        options.Events.RaiseInformationEvents = true;
        options.Events.RaiseFailureEvents = true;
        options.Events.RaiseSuccessEvents = true;
        options.EmitStaticAudienceClaim = true;
    })
    .AddConfigurationStore(options =>
    {
        options.ConfigureDbContext = optionsBuilder =>
            optionsBuilder.UseSqlServer(connectionString,
                sql =>
                {
                    sql.MigrationsAssembly(migrationsAssembly);
                    sql.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                });
    })
    .AddOperationalStore(options =>
    {
        options.ConfigureDbContext = optionsBuilder =>
            optionsBuilder.UseSqlServer(connectionString,
                sql =>
                {
                    sql.MigrationsAssembly(migrationsAssembly);
                    sql.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                });
    })
    .AddAspNetIdentity<ApplicationUser>()
    .AddInMemoryCaching()
    .AddConfigurationStoreCache()
    .AddProfileService<ProfileService<ApplicationUser>>();
```

### Enable authentication

Insert this line before the `var app = builder.Build();` line:
```
builder.Services.AddAuthentication();
```

<br/>

### Run database initialization code

Below `var app = builder.Build();` add this line:
```
IdentityServerInitialization.InitializeDatabase(app, coreApiSecretString);
```

<br/>

### Use Identity Server

Between `app.UseRouting();` and `app.UseAuthorization();` insert this line:
```
app.UseIdentityServer();
```
Then append `.RequireAuthorization()` to `app.MapRazorPages()`.

<br/>

## Initial database migrations

Before running the application the database needs to be created and the configuration needs to be applied.

In the "Package Manager Console" run these commands:

### ApplicationDbContext
```
add-migration InitialIdentityServerApplicationDbMigration -Project Examplium.IdentityServer -Context ApplicationDbContext
```

```
update-database -Project Examplium.IdentityServer -Context ApplicationDbContext
```

### PersistedGrantDbContext

```
add-migration InitialIdentityServerPersistedGrantDbMigration -Project Examplium.IdentityServer -Context PersistedGrantDbContext
```

```
update-database -Project Examplium.IdentityServer -Context PersistedGrantDbContext
```

### ConfigurationDbContext
```
add-migration InitialIdentityServerConfigurationDbMigration -Project Examplium.IdentityServer -Context ConfigurationDbContext
```

```
update-database -Project Examplium.IdentityServer -Context ConfigurationDbContext
```
<br/>

## Add pages

### Copy page content from Duende Quickstarts

Go to https://github.com/DuendeSoftware/Samples/tree/main/IdentityServer/v6/Quickstarts/2_InteractiveAspNetCore/src/IdentityServer/Pages and copy all the contents, except TestUsers.cs, to the Examplium.IdentityServer/Pages folder.

Update all the name spaces where needed in the copied files.

### Copy CSS and JavaScript

Copy the two files found here https://github.com/DuendeSoftware/Samples/tree/main/IdentityServer/v6/Quickstarts/2_InteractiveAspNetCore/src/IdentityServer/wwwroot/js to Examplium.IdentityServer/wwwroot/js/

Copy the content of https://github.com/DuendeSoftware/Samples/blob/main/IdentityServer/v6/Quickstarts/2_InteractiveAspNetCore/src/IdentityServer/wwwroot/css/site.css to the site.css file in this folder Examplium.IdentityServer/wwwroot/css/

<br/>

## Next steps

### Add sign up/register pages.
The Duende Quickstart project does not have any pages for creating user accounts, they will need to be added next.

### Add confirmation email
To ensure users own the email address they use for registering a new account, an email with a code should be sent to the users which they can click to confirm that the email belongs to them.

- Add code link generation.
- Add email service
- Add Confirmation page

### Add change password and password reset/forgot password features.

### Add change email address feature.

### Add delete account feature

### Add External logins (Google/Apple/Microsoft/Facebook/etc).

### Add 2FA (Two-Factor Authentication)

### Configure clients and APIs to use the Identity Server

- Add Backend for Frontend (BFF) to Examplium.Server and Examplium.Client: https://docs.duendesoftware.com/identityserver/v6/bff/ and https://github.com/DuendeSoftware/Samples/tree/main/IdentityServer/v6/BFF/BlazorWasm
- For mobile, microservices, and other clients the APIs will need to be configured too: https://docs.duendesoftware.com/identityserver/v6/apis/ 

### Add GDPR/cookie consent features.

Ideally there should be no cookies to worry about, but if 3rd party cookies will be added (i.e. for Google/Facebook/Apple/Microsoft/etc logins) we need to show the users information about what cookies are used and what for, and they should be given a choice whether to allow them or not.
 
### Update pages and CSS to have a consistent design in the entire solution/product.

### Add language/localization features.

### Other

This implementation was intended for authentication and authorization only, the user information is very limited as the user profile, roles, and other claims will be handled by the other APIs/Services. You could add detailed user profiles, role management, and more here, which could give clients more information directly from the user claims. 

This limitation of user data may add some overhead for the Examplium.Server, and other future services and clients, to manage and validate user related data, but it will also reduce the dependency on Identity Server, so if you find another solution more appropriate it will be easier to reaplce it.

