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
