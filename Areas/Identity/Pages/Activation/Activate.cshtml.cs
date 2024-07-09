//using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SAMS.Controllers;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace SAMS.Areas.Identity.Pages.Activation
{
    [AllowAnonymous]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public class ActivateModel(/*UserManager<ApplicationUser> userManager, */SignInManager<ApplicationUser> signInManager, ILogger<ActivateModel> logger, IServiceScopeFactory serviceScopeFactory) : PageModel
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
        //private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
        private readonly ILogger<ActivateModel> _logger = logger;
        private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;

        [TempData]
        public string? StatusMessage { get; set; }

        public void OnGet()
        {

        }

        [BindProperty]
        public InputModel Input { get; set; }
        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string? Email { get; set; }
            [Required]
            [StringLength(32, ErrorMessage = "The Unique Code must be at least {2} and at max {1} characters long.", MinimumLength = 32)]
            [Display(Name = "Unique Code")]
            public string? ActivationCode { get; set; }
            [Required]
            [Display(Name = "School Id")]
            public string? SchoolId { get; set; }
        }

        public async Task<IActionResult> OnPostAsync(InputModel input)
        {
            if (ModelState.IsValid)
            {
                Input = input;
                TempData["InputEmail"] = Input.Email;
                var scope = _serviceScopeFactory.CreateAsyncScope();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                var foundUser = await userManager.FindByEmailAsync(Input.Email!);
                if (foundUser == null)
                {
                    return NotFound("User Not Found.");
                }
                var foundUserID = foundUser.Id;
                TempData["UserId"] = foundUser.Id;

                if (foundUser != null)
                {
                    var foundEmail = foundUser.Email!.ToLower();
                    var foundActCode = foundUser.ActivationCode;
                    var foundSchID = foundUser.SchoolId;
                    if ((foundActCode == Input.ActivationCode) && (foundSchID == Input.SchoolId) && (foundEmail == Input.Email))
                    {
                        if (foundUser.UserExperienceEnabled != true)
                        {
                            foundUser.UserExperienceEnabled = true;
                            var savechanges = await userManager.UpdateAsync(foundUser);

                            if (savechanges.Succeeded)
                            {
                                Console.WriteLine("Changes were saved and everything worked until here.");
                                // Clear the existing external cookie to ensure a clean login process
                                await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
                                var provider = "Google";

                                // Request a redirect to the external login provider to link a login for the current user
                                var redirectUrl = Url.Page("./Activate", pageHandler: "LinkLoginCallback");
                                var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl, foundUserID);

                                return new ChallengeResult(provider, properties);
                            }
                            else
                            {
                                ModelState.AddModelError("", "Failed to save changes.");
                                return Page();
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("", "Your account has been activated already. Please login with Google or your password to login.");
                        }
                    }
                    else
                    {
                        return Page();
                    }
                }
                //else
                //{
                //    foundUser.UserExperienceEnabled = false;
                //    await userManager.UpdateAsync(foundUser);
                //    return RedirectToPage("UserNotFound");
                //}
            }
            else
            {
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var modelError in modelState.Errors)
                    {
                        _logger.LogCritical("Error message: \n {ErrorMessage}", modelError.ErrorMessage);
                    }
                }
            }
            // If we got this far, something failed, redisplay form
            return Page();
        }


        public async Task<IActionResult> OnGetLinkLoginCallbackAsync()
        {
            var userId = TempData["UserId"]!.ToString();
            if (string.IsNullOrEmpty(userId))
            {
                return NotFound("User ID not found.");
            }

            var scope = _serviceScopeFactory.CreateAsyncScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var inputUser = await userManager.FindByIdAsync(userId);
            if (inputUser == null)
            {
                scope.Dispose();
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            var info = await _signInManager.GetExternalLoginInfoAsync(userId);
            if (info == null)
            {
                scope.Dispose();
                throw new InvalidOperationException($"Unexpected error occurred loading external login info.");
            }

            var googleemail = info.Principal.FindFirstValue(ClaimTypes.Email);
            if (googleemail == null)
            {
                StatusMessage = "You need to allow SAMS to access only your email address from your Google Account. This is for user verification purposes only. You will need to re-activate your account now.";
                inputUser.UserExperienceEnabled = false;
                await userManager.UpdateAsync(inputUser);
                scope.Dispose();
                return Page();
            }
            var googleuser = await userManager.FindByEmailAsync(googleemail);
            if (googleuser == null)
            {
                StatusMessage = "You are not a registered user on the Synnovation Lab AMS. Please contact the administrators if you think this is wrong.";
                inputUser.UserExperienceEnabled = false;
                await userManager.UpdateAsync(inputUser);
                TempData["Messaage"] = StatusMessage;
                return LocalRedirect("~/Identity/Account/Login");                
            }
            var googleuseremail = googleuser.Email;
            if (googleuseremail != null)
            {
                if (inputUser.Email == googleemail)
                {
                    var result = await userManager.AddLoginAsync(inputUser, info);
                    if (!result.Succeeded)
                    {
                        StatusMessage = "The external login was not added. External logins can only be associated with one account.";
                        inputUser.UserExperienceEnabled = false;
                        await userManager.UpdateAsync(inputUser);
                        scope.Dispose();
                        TempData["Messaage"] = StatusMessage;
                        return LocalRedirect("~/Identity/Account/Login");
                    }
                    // Clear the existing external cookie to ensure a clean login process
                    await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
                    StatusMessage = "The external login was added.";
                    scope.Dispose();
                    TempData["Messaage"] = StatusMessage;
                    return RedirectToPage("ActivationSuccessfull");
                }
                else
                {
                    _logger.LogInformation("The email address from the Google Account was not found with the one in the database.");
                    inputUser.UserExperienceEnabled = false;
                    await userManager.UpdateAsync(inputUser);
                    scope.Dispose();
                    StatusMessage = "Email mismatch. The entered email and Google Account email did not match. Please contact the administrators.";
                    TempData["Messaage"] = StatusMessage;
                    return LocalRedirect("~/Identity/Account/Login");
                }
            }
            else
            {
                _logger.LogInformation("Couldn't find the user from the Google Account email address in our database.");
                inputUser.UserExperienceEnabled = false;
                await userManager.UpdateAsync(inputUser);
                scope.Dispose();
                return RedirectToPage("UserNotFound");
            }
        }
    }
}