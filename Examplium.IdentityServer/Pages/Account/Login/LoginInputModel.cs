// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.
using System.ComponentModel.DataAnnotations;

namespace Examplium.IdentityServer.Pages.Account.Login
{
    public class LoginInputModel
    {
        [Required] public string Username { get; set; } = string.Empty;

        [Required] public string Password { get; set; } = string.Empty;

        public bool RememberLogin { get; set; }

        public string ReturnUrl { get; set; } = string.Empty;

        public string Button { get; set; } = string.Empty;
    }
}
