# Update Examplium.Server Configuration

The official Duende documentation for Blazor WASM configuration can be found here: https://docs.duendesoftware.com/identityserver/v6/quickstarts/7_blazor/

<br/>

### Add nuget packages

Add these NuGet packages to the Examplium.Server project:
- Microsoft.AspNetCore.Authentication.OpenIdConnect
- Duende.BFF

<br/>

### Update Program.cs

Open the Examplium.Server/Program.cs file.

Before `var app = builder.Build();` add the BFF (Backend For Frontend) services like this:
```
builder.Services.AddBff();
```

Next, add authentication, for now we will just configure it for debugging:
```
var coreApiSecretString = builder.Configuration["CoreApiSecret"] ?? throw new InvalidOperationException("Configuration string 'CoreApiSecret' not found.");

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = "cookie";
        options.DefaultChallengeScheme = "oidc";
        options.DefaultSignOutScheme = "oidc";
    })
    .AddCookie("cookie", options =>
    {
        options.Cookie.Name = "__Host-blazor";
        options.Cookie.SameSite = SameSiteMode.Strict;
    })
    .AddOpenIdConnect("oidc", options =>
    {
        if (builder.Environment.IsDevelopment())
        {
            options.Authority = ExampliumAuthServerConstants.IdentityServerUrlDebug;
            options.ClientId = "Examplium.WebServer.Debug";
        }

        options.ClientSecret = coreApiSecretString;
        options.ResponseType = "code";
        options.ResponseMode = "query";

        options.Scope.Clear();
        options.Scope.Add("openid");
        options.Scope.Add(ExampliumAuthServerConstants.CoreApiName);
        options.Scope.Add("offline_access");

        options.MapInboundClaims = false;
        options.GetClaimsFromUserInfoEndpoint = true;
        options.SaveTokens = true;
    });
```

Add middleware for authentication, authorization and BFF session management.

Near the end, replace the `app.UseAuthorization();` line with these lines:
```
app.UseAuthentication();
app.UseBff();
app.UseAuthorization();

app.MapBffManagementEndpoints();
```

Replace the line `app.MapControllers();` with this:
```
app.MapControllers()
    .RequireAuthorization()
    .AsBffApiEndpoint();

```

<br/>

### Add API secret to Examplium.Server user secrets.

Copy the "CoreApiSecret" key and value from the Examplium.IdentityServer user secrets to the Examplium.Server user secrets.

See the Examplium.Server/SecretsTemplate.txt file for an example of what it should look like.

<br/>

### Update "Startup Project" settings

In the solution explorer right click on "Solution 'Examplium'", then select "Select Startup Projects...".

Select the option "Multiple startup projects".

Move Examplium.IdentityServer to the top, so it starts first, and set the action to "Start".

Set Examplium.Server's action to "Start" as well.

