// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace  VirtualGameStore.Areas.Identity.Pages.Account.Manage
{
    public static class ManageNavPages
    {
        public static string Index => "Index";
        public static string Email => "Email";
        public static string ChangePassword => "ChangePassword";
        public static string DownloadPersonalData => "DownloadPersonalData";
        public static string DeletePersonalData => "DeletePersonalData";
        public static string ExternalLogins => "ExternalLogins";
        public static string PersonalData => "PersonalData";
        public static string TwoFactorAuthentication => "TwoFactorAuthentication";
        public static string Addresses => "Addresses";
        public static string Preferences => "Preferences";
        public static string WishList => "WishList";
        public static string PaymentOptions => "PaymentOptions";
        public static string CreatePaymentOptions => "CreatePaymentOptions";
        public static string EditPaymentOptions => "EditPaymentOptions";
        public static string FriendsAndFamily => "FriendsAndFamily";

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public static string IndexNavClass(ViewContext viewContext) => PageNavClass(viewContext, Index);
        public static string EmailNavClass(ViewContext viewContext) => PageNavClass(viewContext, Email);
        public static string ChangePasswordNavClass(ViewContext viewContext) => PageNavClass(viewContext, ChangePassword);
        public static string DownloadPersonalDataNavClass(ViewContext viewContext) => PageNavClass(viewContext, DownloadPersonalData);
        public static string DeletePersonalDataNavClass(ViewContext viewContext) => PageNavClass(viewContext, DeletePersonalData);
        public static string ExternalLoginsNavClass(ViewContext viewContext) => PageNavClass(viewContext, ExternalLogins);
        public static string PersonalDataNavClass(ViewContext viewContext) => PageNavClass(viewContext, PersonalData);
        public static string TwoFactorAuthenticationNavClass(ViewContext viewContext) => PageNavClass(viewContext, TwoFactorAuthentication);
        public static string PreferencesNavClass(ViewContext viewContext) => PageNavClass(viewContext, Preferences);
        public static string PaymentOptionsNavClass(ViewContext viewContext) => PageNavClass(viewContext, PaymentOptions);
        public static string CreatePaymentOptionsNavClass(ViewContext viewContext) => PageNavClass(viewContext, CreatePaymentOptions);
        public static string EditPaymentOptionsNavClass(ViewContext viewContext) => PageNavClass(viewContext, EditPaymentOptions);
        public static string AddressesNavClass(ViewContext viewContext) => PageNavClass(viewContext, Addresses);
        public static string WishListNavClass(ViewContext viewContext) => PageNavClass(viewContext, WishList);
        public static string FriendsAndFamilyNavClass(ViewContext viewContext) => PageNavClass(viewContext, FriendsAndFamily);

        public static string PageNavClass(ViewContext viewContext, string page)
        {
            var activePage = viewContext.ViewData["ActivePage"] as string
                ?? System.IO.Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);
            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }
    }
}
