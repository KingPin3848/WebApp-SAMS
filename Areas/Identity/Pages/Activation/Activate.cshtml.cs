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

namespace SAMS.Areas.Identity.Pages.Activation
{
    [AllowAnonymous]
    public class ActivateModel : PageModel
    {
        private readonly UserManager<ApplicationUser>? _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<ActivateModel> _logger;

        [TempData]
        public string? StatusMessage { get; set; }

        public ActivateModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ILogger<ActivateModel> logger)
        {
            _userManager = userManager;
            Input = new InputModel();
            _signInManager = signInManager;
            _logger = logger;
        }

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
            //[StringLength(32, ErrorMessage = "The activation code must be exactly 32 characters long.", MinimumLength = 32)]
            [Display(Name = "Activation Code")]
            public string? ActivationCode { get; set; }

            [Required]
            [Display(Name = "School Issued ID")]
            public string? SchoolIssuedID { get; set; }
        }

        public async Task<IActionResult> OnPostAsync(InputModel input)
        {
            if (ModelState.IsValid)
            {
                var foundUser = await _userManager.FindByEmailAsync(input.Email);
                var foundUserID = foundUser.Id;
                TempData["UserId"] = foundUser.Id;

                if (foundUser != null)
                {
                    var foundEmail = foundUser.NormalizedEmail.ToLower();
                    var foundActCode = foundUser.ActivationCode;
                    var foundSchID = foundUser.SchoolId;
                    if ((foundActCode == input.ActivationCode) && (foundSchID == input.SchoolIssuedID) && (foundEmail == input.Email))
                    {
                        if(foundUser.UserExperienceEnabled != true)
                        {
                            foundUser.UserExperienceEnabled = true;
                            var savechanges = await _userManager.UpdateAsync(foundUser);

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
                        } else
                        {
                            ModelState.AddModelError("", "Your account has been activated already. Please login with Google or your password to login.");
                        }
                    }
                    else
                    {
                        return Page();
                    }
                }
                else
                {
                    return RedirectToPage("UserNotFound");
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }


        public async Task<IActionResult> OnGetLinkLoginCallbackAsync()
        {
            var userId = TempData["UserId"]?.ToString();
            if(string.IsNullOrEmpty(userId))
            {
                return NotFound("User ID not found in TempData.");
            }

            var foundUser = await _userManager.FindByIdAsync(userId);
            if (foundUser == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            var info = await _signInManager.GetExternalLoginInfoAsync(userId);
            if (info == null)
            {
                throw new InvalidOperationException($"Unexpected error occurred loading external login info.");
            }

            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            var dbuser = await _userManager.FindByEmailAsync(email);
            var dbuseremail = dbuser.Email;
            if (dbuseremail != null)
            {
                if (dbuseremail == email)
                {
                    var result = await _userManager.AddLoginAsync(foundUser, info);
                    if (!result.Succeeded)
                    {
                        StatusMessage = "The external login was not added. External logins can only be associated with one account.";
                        return RedirectToPage("../../../../../");
                    }
                    // Clear the existing external cookie to ensure a clean login process
                    await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
                    StatusMessage = "The external login was added.";
                    return RedirectToPage("ActivationSuccessfull");
                }
                else
                {
                    _logger.LogInformation("The email address from the Google Account was not found with the one in the database.");
                    return RedirectToPage("UserNotFound");
                }
            }
            else
            {
                _logger.LogInformation("Couldn't find the user from the Google Account email address in our database.");
                return RedirectToPage("UserNotFound");
            }
        }
    }
}
