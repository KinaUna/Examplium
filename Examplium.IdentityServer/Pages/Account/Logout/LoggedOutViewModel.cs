// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.
namespace Examplium.IdentityServer.Pages.Account.Logout
{
    public class LoggedOutViewModel
    {
        public string PostLogoutRedirectUri { get; set; } = string.Empty;
        public string ClientName { get; set; } = string.Empty;
        public string SignOutIframeUrl { get; set; } = string.Empty;
        public bool AutomaticRedirectAfterSignOut { get; set; }
    }
}
