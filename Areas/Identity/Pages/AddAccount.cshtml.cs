using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SAMS.Controllers;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Encodings.Web;
using System.Text;

namespace SAMS.Areas.Identity.Pages
{
    public class AddAccountModel : PageModel
    {
        //PRIVATE VARIABLES
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IUserEmailStore<ApplicationUser> _emailStore;
        private readonly ILogger<AddAccountModel> _logger;
        private readonly IEmailSender _emailSender;

        //CONSTRUCTOR FROM PROGRAM.CS
        public AddAccountModel(
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            SignInManager<ApplicationUser> signInManager,
            ILogger<AddAccountModel> logger,
            IEmailSender emailSender)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        public InputModel Input = new();
        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string? Email { get; set; }

            [Required]
            [StringLength(32, ErrorMessage = "The Activation Code must be at least {2} and at max {1} characters long.", MinimumLength = 32)]
            [Display(Name = "Activation Code")]
            public string? ActivationCode { get; set; }

            [Required]
            [Display(Name = "School Issued Id")]
            public string? SchoolIssuedId { get; set; }
            [Required]
            [Display(Name = "Role")]
            public IList<string>? AssignedRole { get; set; }
            [Display(Name = "Enable User Experience")]
            public Boolean? UserExperience {  get; set; }
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync(InputModel input)
        {
            Input = input;

            if (ModelState.IsValid)
            {
                var user = CreateUser();

                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                user.Role = input.AssignedRole;
                user.SchoolId = input.SchoolIssuedId!;
                user.ActivationCode = input.ActivationCode!;
                user.UserExperienceEnabled = false;
                user.Email = input.Email;
                user.EmailConfirmed = true;
                var result = await _userManager.CreateAsync(user);

                if (result.Succeeded)
                {
                    //TO UPDATE ALL USER'S INFORMATION AKA ADDING THEIR ACTIVATION CODES, SCHOOLISSUEDID, ETC.
                    var foundUser = await _userManager.FindByEmailAsync(Input.Email!);
                    if (foundUser != null)
                    {
                        var roadroller = await _userManager.AddToRolesAsync(foundUser, Input.AssignedRole!);
                        if (roadroller.Succeeded)
                        {
                            return Page();
                        } else
                        {
                            foreach (var errors in roadroller.Errors)
                            {
                                ModelState.AddModelError(string.Empty, errors.Description);
                            }
                        }
                    }
                    _logger.LogInformation("User created a new account WITHOUT password.");
                    //SNIPPET 1 FOR EMAIL GOES HERE
                    return Page();
                } else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private ApplicationUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                    $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
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
//SNIPPET 1 STARTS HERE
//var userId = await _userManager.GetUserIdAsync(user);
// var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
//code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
//var callbackUrl = Url.Page(
//    "/Account/ConfirmEmail",
//    pageHandler: null,
//    values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
//    protocol: Request.Scheme);

//await _emailSender.SendEmailAsync(input.Email, "Confirm your email",
//    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
//SNIPPET 1 ENDS HERE