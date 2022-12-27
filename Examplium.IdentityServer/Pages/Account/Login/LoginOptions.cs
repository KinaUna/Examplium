﻿// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.
namespace Examplium.IdentityServer.Pages.Account.Login
{
    public class LoginOptions
    {
        public static bool AllowLocalLogin = true;
        public static bool AllowRememberLogin = true;
        public static TimeSpan RememberMeLoginDuration = TimeSpan.FromDays(30);
        public static string InvalidCredentialsErrorMessage = "Invalid username or password";
    }
}
