﻿// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.
namespace Examplium.IdentityServer.Pages.Account.Login
{
    public class LoginViewModel
    {
        public bool AllowRememberLogin { get; set; } = true;
        public bool EnableLocalLogin { get; set; } = true;

        public IEnumerable<LoginViewModel.ExternalProvider> ExternalProviders { get; set; } = Enumerable.Empty<ExternalProvider>();
        public IEnumerable<LoginViewModel.ExternalProvider> VisibleExternalProviders => ExternalProviders.Where(x => !String.IsNullOrWhiteSpace(x.DisplayName));

        public bool IsExternalLoginOnly => EnableLocalLogin == false && ExternalProviders?.Count() == 1;
        public string? ExternalLoginScheme => IsExternalLoginOnly ? ExternalProviders?.SingleOrDefault()?.AuthenticationScheme : null;

        public class ExternalProvider
        {
            public string DisplayName { get; set; } = string.Empty;
            public string AuthenticationScheme { get; set; } = string.Empty;
        }
    }
}
