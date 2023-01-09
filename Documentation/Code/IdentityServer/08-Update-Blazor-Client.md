# Update Examplium.Client 

For the original Duende documentation for Blazor WASM see this link: https://docs.duendesoftware.com/identityserver/v6/quickstarts/7_blazor/

<br/>

### Update App.razor

Replace the content of Examplium.Client/App.razor with this code:
```
<CascadingAuthenticationState>
    <Router AppAssembly="@typeof(App).Assembly">
        <Found Context="routeData">
            <RouteView RouteData="@routeData" DefaultLayout="@typeof(MainLayout)" />
            <FocusOnNavigate RouteData="@routeData" Selector="h1" />
        </Found>
        <NotFound>
            <PageTitle>Not found</PageTitle>
            <LayoutView Layout="@typeof(MainLayout)">
                <p role="alert">Sorry, there's nothing at this address.</p>
            </LayoutView>
        </NotFound>
    </Router>
</CascadingAuthenticationState>
```

<br/>

### Update MainLayout.razor

Replace the content of Examplium.Client/Shared/MainLayout.razor with this code:
```
@inherits LayoutComponentBase

<div class="page">
    <div class="sidebar">
        <NavMenu/>
    </div>

    <div class="main">
        <div class="top-row px-4">
            <AuthorizeView>
                <Authorized>
                    <strong>Hello, @context.User.Identity.Name!</strong>
                    <a href="@context.User.FindFirst("bff:logout_url")?.Value">Log out</a>
                </Authorized>
                <NotAuthorized>
                    <a href="bff/login">Log in</a>
                </NotAuthorized>
            </AuthorizeView>
        </div>

        <div class="content px-4">
            @Body
        </div>
    </div>
</div>
```

<br/>

### Add Authentication State Provider

Add a new folder to the Examplium.Client project, name it "BFF".

Add a new class to this folder and name it "BffAuthenticationStateProvider", then update the content with the following code:

```
using System.Net;
using System.Net.Http.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace Examplium.Client.BFF
{
    public class BffAuthenticationStateProvider : AuthenticationStateProvider
    {
        private static readonly TimeSpan UserCacheRefreshInterval
        = TimeSpan.FromSeconds(60);

        private readonly HttpClient _client;
        private readonly ILogger<BffAuthenticationStateProvider> _logger;

        private DateTimeOffset _userLastCheck
            = DateTimeOffset.FromUnixTimeSeconds(0);
        private ClaimsPrincipal _cachedUser
            = new ClaimsPrincipal(new ClaimsIdentity());

        public BffAuthenticationStateProvider(
            HttpClient client,
            ILogger<BffAuthenticationStateProvider> logger)
        {
            _client = client;
            _logger = logger;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            return new AuthenticationState(await GetUser());
        }

        private async ValueTask<ClaimsPrincipal> GetUser(bool useCache = true)
        {
            var now = DateTimeOffset.Now;
            if (useCache && now < _userLastCheck + UserCacheRefreshInterval)
            {
                _logger.LogDebug("Taking user from cache");
                return _cachedUser;
            }

            _logger.LogDebug("Fetching user");
            _cachedUser = await FetchUser();
            _userLastCheck = now;

            return _cachedUser;
        }

        record ClaimRecord(string Type, object Value);

        private async Task<ClaimsPrincipal> FetchUser()
        {
            try
            {
                _logger.LogInformation("Fetching user information.");
                var response = await _client.GetAsync("bff/user?slide=false");

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var claims = await response.Content.ReadFromJsonAsync<List<ClaimRecord>>();

                    var identity = new ClaimsIdentity(
                        nameof(BffAuthenticationStateProvider),
                        "name",
                        "role");

                    foreach (var claim in claims)
                    {
                        identity.AddClaim(new Claim(claim.Type, claim.Value.ToString()));
                    }

                    return new ClaimsPrincipal(identity);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Fetching user failed.");
            }

            return new ClaimsPrincipal(new ClaimsIdentity());
        }
    }
}
```

<br/>

### Add Anti-Forgery Handler

In the "BFF" folder add a new class with the name "AntiForgeryHandler".

Replace the code with this:
```
namespace Examplium.Client.BFF
{
    public class AntiForgeryHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Add("X-CSRF", "1");
            return base.SendAsync(request, cancellationToken);
        }
    }
}
```

<br/>

### Update services

In Examplium.Client/Program.cs replace the content with this:
```
using Examplium.Client;
using Examplium.Client.BFF;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddTransient<AntiForgeryHandler>();

builder.Services.AddHttpClient("Examplium.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler<AntiForgeryHandler>();

// Supply HttpClient instances that include access tokens when making requests to the server project
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("Examplium.ServerAPI"));

builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, BffAuthenticationStateProvider>();

builder.Services.AddApiAuthorization();

await builder.Build().RunAsync();
```

<br/>
