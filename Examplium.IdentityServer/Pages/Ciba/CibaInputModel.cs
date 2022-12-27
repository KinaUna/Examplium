// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.
namespace Examplium.IdentityServer.Pages.Ciba
{
    public class CibaInputModel
    {
        public string Button { get; set; }
        public IEnumerable<string> ScopesConsented { get; set; }
        public string Id { get; set; }
        public string Description { get; set; }
    }
}
