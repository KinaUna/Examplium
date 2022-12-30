// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.
namespace Examplium.IdentityServer.Pages.Ciba
{
    public class CibaViewModel
    {
        public string ClientName { get; set; } = string.Empty;
        public string ClientUrl { get; set; } = string.Empty;
        public string ClientLogoUrl { get; set; } = string.Empty;

        public string BindingMessage { get; set; } = string.Empty;

        public IEnumerable<ScopeViewModel>? IdentityScopes { get; set; }
        public IEnumerable<ScopeViewModel>? ApiScopes { get; set; }
    }

    public class ScopeViewModel
    {
        public string Name { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool Emphasize { get; set; }
        public bool Required { get; set; }
        public bool Checked { get; set; }
        public IEnumerable<ResourceViewModel>? Resources { get; set; }
    }

    public class ResourceViewModel
    {
        public string Name { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
    }
}
