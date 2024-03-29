﻿// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.
namespace Examplium.IdentityServer.Pages.Consent
{
    public class ConsentInputModel
    {
        public string Button { get; set; } = string.Empty;
        public IEnumerable<string>? ScopesConsented { get; set; }
        public bool RememberConsent { get; set; } = true;
        public string ReturnUrl { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
