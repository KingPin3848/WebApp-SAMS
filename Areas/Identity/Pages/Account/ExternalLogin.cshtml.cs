// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using SAMS.Controllers;
using Microsoft.AspNetCore.Authentication.Google;

namespace SAMS.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ExternalLoginModel(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            ILogger<ExternalLoginModel> logger
            /*IEmailSender emailSender*/) : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IUserStore<ApplicationUser> _userStore = userStore;
        //private readonly IUserEmailStore<ApplicationUser> _emailStore = GetEmailStore();
        //private readonly IEmailSender _emailSender = emailSender;
        private readonly ILogger<ExternalLoginModel> _logger = logger;


        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ProviderDisplayName { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string ErrorMessage { get; set; }

        public IUserEmailStore<ApplicationUser> EmailStore => GetEmailStore();

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public IActionResult OnGet() => RedirectToPage("./Login");

        public IActionResult OnPost(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Page("./ExternalLogin", pageHandler: "Callback", values: new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        public async Task<IActionResult> OnGetCallbackAsync(string returnUrl = null, string remoteError = null)
        {
            if(User.IsInRole("Teacher"))
            {
                returnUrl = returnUrl ?? Url.Action("Dashbaord", "TeacherDashboard");
            }
            else
            {
                returnUrl = returnUrl ?? Url.Content("~/");
            }
            if (remoteError != null)
            {
                ErrorMessage = $"Error from external provider: {remoteError}";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ErrorMessage = "Error loading external login information.";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

            var googleemail = info.Principal.FindFirstValue(ClaimTypes.Email);
            if (googleemail == null)
            {
                ErrorMessage = "Error loading email address for user verification.";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

            var dbuser = await _userManager.FindByEmailAsync(googleemail);
            if (dbuser == null)
            {
                ErrorMessage = "Not a registered user on the Synnovation Lab AMS. Please contact the administrators if you think this is wrong.";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }
            var dbuseremail = dbuser.Email;
            if (dbuseremail != null)
            {
                if (dbuseremail == googleemail)
                {
                    // Sign in the user with this external login provider if the user already has a login.
                    var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
                    if (result.Succeeded)
                    {
                        if (result.IsLockedOut)
                        {
                            return RedirectToPage("./Lockout");
                        }
                        else
                        {
                            _logger.LogInformation("THE ACCOUNT IS NOT LOCKED OUT.");
                            _logger.LogInformation("{Name} logged in with {LoginProvider} provider.", info.Principal.Identity.Name, info.LoginProvider);
                            if (User.IsInRole("Teacher"))
                            {
                                returnUrl = "~/Dashboard/Dashboard";
                            }
                            return LocalRedirect(returnUrl);
                        }
                    }
                    else
                    {
                        return RedirectToPage("LoginUnsuccessfull");
                    }
                }
                else
                {
                    return RedirectToPage("EmailUnmatch");
                }
            }
            else
            {
                return RedirectToPage("EmailUnmatch");
            }
        }

        private IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<ApplicationUser>)_userStore;
        }
    }
}


/*
 CODE NOT TO BE USED
else
                    {
                        //// If the user does not have an account, then ask the user to create an account.
                        //ReturnUrl = returnUrl;
                        //ProviderDisplayName = info.ProviderDisplayName;
                        //if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
                        //{
                        //    Input = new InputModel
                        //    {
                        //        Email = info.Principal.FindFirstValue(ClaimTypes.Email)
                        //    };
                        //}
                        //return Page();
                    }
 */