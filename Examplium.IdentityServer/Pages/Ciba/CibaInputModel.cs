// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.
namespace Examplium.IdentityServer.Pages.Ciba
{
    public class CibaInputModel
    {
        public string Button { get; set; } = string.Empty;
        public IEnumerable<string>? ScopesConsented { get; set; } 
        public string Id { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
