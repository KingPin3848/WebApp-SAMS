using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SAMS.Areas.Identity.Pages;
using SAMS.Controllers;
using SAMS.Data;
using SAMS.Models;
using System.ComponentModel.DataAnnotations;

namespace SAMS.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountManagerController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IUserEmailStore<ApplicationUser> _emailStore;
        private readonly ILogger<AccountManagerController> _logger;
        private readonly IEmailSender<ApplicationUser> _emailSender;
        private readonly ApplicationDbContext _context;

        public AccountManagerController(
            RoleManager<IdentityRole> roleManager, 
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            IUserEmailStore<ApplicationUser> emailStore,
            ILogger<AccountManagerController> logger,
            IEmailSender<ApplicationUser> emailSender,
            ApplicationDbContext context
            )
        {
            _context = context;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = emailStore;
            _logger = logger;
            _emailSender = emailSender;
        }

        public InputModel Input { get; set; }
        public class InputModel : ApplicationUser
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
            public string? AssignedRole { get; set; }

            //Navigation
            public StudentInfoModel? StudentInfo { get; set; }


        }


        // GET: AccountManager
        public async Task <IActionResult> Index()
        {
            var users = _userManager.Users.Cast<ApplicationUser>().ToList();

            foreach (var user in users)
            {
                user.Role = await _userManager.GetRolesAsync(user);
            }

            return View(users);
        }

        // GET: AccountManager/Details/5
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: AccountManager/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AccountManager/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InputModel input)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = CreateUser();

                    await _userStore.SetUserNameAsync(user, input.Email, CancellationToken.None);
                    await _emailStore.SetEmailAsync(user, input.Email, CancellationToken.None);
                    var result = await _userManager.CreateAsync(user);

                    if (result.Succeeded)
                    {
                        //TO UPDATE ALL USER'S INFORMATION AKA ADDING THEIR ACTIVATION CODES, SCHOOLISSUEDID, ETC.
                        var foundUser = await _userManager.FindByEmailAsync(input.Email);
                        if (foundUser != null)
                        {
                            foundUser.ActivationCode = input.ActivationCode;
                            foundUser.SchoolId = input.SchoolIssuedId;
                            foundUser.EmailConfirmed = true;
                            var roadroller = await _userManager.AddToRoleAsync(foundUser, input.AssignedRole);
                            if (roadroller.Succeeded)
                            {
                                return View();
                            }
                            else
                            {
                                foreach (var errors in roadroller.Errors)
                                {
                                    ModelState.AddModelError(string.Empty, errors.Description);
                                }
                            }
                        }
                        _logger.LogInformation("User created a new account WITHOUT password.");
                        //SNIPPET 1 FOR EMAIL GOES HERE
                        return View("Create");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
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


        // GET: AccountManager/Edit/5
        public async Task <IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Assuming you have a list of roles in your context similar to the StudentId in your example
            ViewData["RoleId"] = new SelectList(_context.Roles, "Name", "Name", user?.Role?[0]);
            return View(user);
        }

        // POST: AccountManager/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, ApplicationUser model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.FindByIdAsync(id);
                    if (user == null)
                    {
                        return NotFound();
                    }

                    // Update the details
                    user.Email = model.Email;
                    user.ActivationCode = model.ActivationCode;
                    user.SchoolId = model.SchoolId;

                    // Get the current roles of the user
                    var currentRoles = await _userManager.GetRolesAsync(user);

                    // Remove the user from the current roles
                    var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
                    if (!removeResult.Succeeded)
                    {
                        // Handle the error
                        throw new Exception("Failed to remove user roles.");
                    }

                    // Add the user to the new role
                    var addResult = await _userManager.AddToRolesAsync(user, model.Role);
                    if (!addResult.Succeeded)
                    {
                        // Handle the error
                        throw new Exception("Failed to add user roles.");
                    }

                    await _userManager.UpdateAsync(user);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(model.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoleId"] = new SelectList(_context.Roles, "Name", "Name", model.Role[0]);
            return View(model);
        }

        private bool UserExists(string schoolId)
        {
            return _context.Users.Any(e => e.SchoolId == schoolId);
        }


        // GET: AccountManager/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: AccountManager/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return NotFound();
                }

                var result = await _userManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    // Handle the error
                    throw new Exception("Failed to delete user.");
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
