// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Examplium.IdentityServer.Pages.Redirect
{
    [AllowAnonymous]
    public class IndexModel : PageModel
    {
        public string RedirectUri { get; set; } = string.Empty;

        public IActionResult OnGet(string redirectUri)
        {
            if (!Url.IsLocalUrl(redirectUri))
            {
                return RedirectToPage("/Error/Index");
            }

            RedirectUri = redirectUri;
            return Page();
        }
    }
}
