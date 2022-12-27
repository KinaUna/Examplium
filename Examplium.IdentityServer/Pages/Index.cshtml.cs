using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Reflection;

namespace Examplium.IdentityServer.Pages
{
    [AllowAnonymous]
    public class IndexModel : PageModel
    {
        public string Version;

        public void OnGet()
        {
            Version = typeof(Duende.IdentityServer.Hosting.IdentityServerMiddleware).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion.Split('+').First();
        }
    }
}