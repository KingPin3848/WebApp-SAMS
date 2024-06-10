using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SAMS.Controllers;
using SAMS.Data;
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
            _emailStore = GetEmailStore();
            _logger = logger;
            _emailSender = emailSender;
            Input = new InputModel();
        }

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
            [Required]
            [Display(Name = "Role(s)")]
            public IList<string>? Role { get; set; }
        }


        // GET: AccountManager
        public async Task<IActionResult> Index()
        {
            var users = _userManager.Users.ToList();
            List<ApplicationUser> unreals = [];
            foreach (var user in users)
            {
                user.Role = await _userManager.GetRolesAsync(user);
                foreach (var role in user.Role)
                {
                    if (role == "Developer")
                    {
                        unreals.Add(user);
                        break;
                    }
                }
            }

            var castedUsers = users.Except(unreals).ToList();
            return View(castedUsers);
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
            ViewData["RoleId"] = new SelectList(_context.Roles, "Name", "Name");
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

                    IEnumerable<string> listRoles = Input.Role!;
                    await _userStore.SetUserNameAsync(user, Input.SchoolId, CancellationToken.None);
                    await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                    user.Role = Input.Role;
                    user.SchoolId = Input.SchoolId!;
                    user.ActivationCode = input.ActivationCode!;
                    user.UserExperienceEnabled = false;
                    user.Email = input.Email;
                    user.EmailConfirmed = true;
                    var result = await _userManager.CreateAsync(user);

                    if (result.Succeeded)
                    {
                        //TO UPDATE ALL USER'S INFORMATION AKA ADDING THEIR ACTIVATION CODES, SCHOOLISSUEDID, ETC.
                        var foundUser = await _userManager.FindByEmailAsync(input.Email!);
                        if (foundUser != null)
                        {
                            var roadroller = await _userManager.AddToRolesAsync(foundUser, listRoles);
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
                        return RedirectToAction(nameof(Index));
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
            catch (Exception ex)
            {
                _logger.LogCritical("{Message}", ex.Message);
                _logger.LogCritical("{Source}", ex.Source);
                _logger.LogCritical("{InnerException}", ex.InnerException);
                _logger.LogCritical("{StackTrace}", ex.StackTrace);
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
        public async Task<IActionResult> Edit(string id)
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
            ViewData["RoleId"] = new SelectList(_context.Roles, "Name", "Name", user.Role);
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
                    user.Role = model.Role;

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
                    var addResult = await _userManager.AddToRolesAsync(user, model.Role!);
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


            ViewData["RoleId"] = new SelectList(_context.Roles, "Name", "Name", model.Role);
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

            var roles = await _userManager.GetRolesAsync(user) ?? throw new Exception("Unable to retrieve user roles. Please make sure you have assigned roles to the selected user. Use the Edit action to add roles to the user.");
            foreach (var role in roles)
            {
                if (role == null)
                {
                    throw new NullReferenceException("Null reference to retrieval of user role.");
                }
                switch (role)
                {
                    case "Student":
                        {
                            ViewData["Student Info"] = "Present";
                            break;
                        }
                    case "Teacher":
                        {
                            ViewData["Teacher Info"] = "Present";
                            break;
                        }
                    case "Attendance Office Member":
                        {
                            ViewData["Attendance Office Member Info"] = "Present";
                            break;
                        }
                    case "HS School Admin":
                        {
                            ViewData["HS School Admin Info"] = "Present";
                            break;
                        }
                    case "Synnovation Lab Admin":
                        {
                            ViewData["Synnovation Lab Admin Info"] = "Present";
                            break;
                        }
                    case "Education Support (EA)":
                        {
                            ViewData["EA Support Info"] = "Present";
                            break;
                        }
                    case "Nurse":
                        {
                            ViewData["Nurse Info"] = "Present";
                            break;
                        }
                    case "Law Enforcement":
                        {
                            ViewData["Law Enforcement Info"] = "Present";
                            break;
                        }
                    case "Synnovation Lab QR Code Scanner Management":
                        {
                            ViewData["QR Code Scanner Management Info"] = "Present";
                            break;
                        }
                    case "Substitute Teacher":
                        {
                            ViewData["Substitute Teacher Info"] = "Present";
                            break;
                        }
                    case "District Admin":
                        {
                            ViewData["District Admin Info"] = "Present";
                            break;
                        }
                    case "Developer":
                        {
                            ViewData["Developer Info"] = "Present";
                            break;
                            //return NotFound("Role not found!");
                        }
                    default:
                        {
                            return NotFound("Role not found.");
                        }
                }
            }

            return View(user);
        }

        // POST: AccountManager/Delete/5
        [HttpPost, ActionName("DeleteConfirmed")]
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

                var schoolId = user.SchoolId ?? throw new Exception("Failed to retrieve School Id of the user for deletion.");
                var roles = await _userManager.GetRolesAsync(user) ?? throw new Exception("Failed to retrieve user roles.");
                foreach (var role in roles)
                {
                    switch (role)
                    {
                        case "Student":
                            {
                                var studId = int.Parse(schoolId);
                                var studentInfo = await _context.StudentInfoModels.FindAsync(studId) ?? throw new Exception("Unable to retrieve student data for deletion.");
                                var sem1sched = await _context.Sem1StudSchedules.FindAsync(studId) ?? throw new Exception("Unable to retrieve student schedule 1 for deletion.");
                                var sem2sched = await _context.Sem2StudSchedules.FindAsync(studId) ?? throw new Exception("Unable to retrieve student schedule 2 for deletion.");

                                var studInfoDeletion = _context.StudentInfoModels.Remove(studentInfo);
                                var sem1schedDeletion = _context.Sem1StudSchedules.Remove(sem1sched);
                                var sem2schedDeletion = _context.Sem2StudSchedules.Remove(sem2sched);
                                await _context.SaveChangesAsync();
                                break;
                            }
                        case "Teacher":
                            {
                                var teacherid = schoolId;
                                var courses = _context.ActiveCourseInfoModels.Where(a => a.CourseTeacherID == teacherid).ToList() ?? throw new Exception("Unable to retrieve teacher's courses for deletion.");
                                var teacherInfo = _context.TeacherInfoModels.Where(a => a.TeacherID == teacherid).ToList() ?? throw new Exception("Unable to retrieve teacher data for deletion.");

                                var deletion1 = _context.Remove(courses);
                                var deletion2 = _context.Remove(teacherInfo);
                                await _context.SaveChangesAsync();
                                break;
                            }
                        case "Attendance Office Member":
                            {
                                var attid = schoolId;
                                var attinfo = _context.AttendanceOfficeMemberModels.Where(a => a.AoMemberID == attid).ToList() ?? throw new Exception("Unable to retrieve attendance office member data for deletion.");

                                var deletion = _context.Remove(attinfo);
                                await _context.SaveChangesAsync();
                                break;
                            }
                        case "HS School Admin":
                            {
                                var adminid = schoolId;
                                var admininfo = _context.AdminInfoModels.Where(a => a.AdminID == adminid).ToList() ?? throw new Exception("Unable to retrieve admin data for deletion.");

                                var deletion = _context.Remove(admininfo);
                                await _context.SaveChangesAsync();
                                break;
                            }
                        case "Synnovation Lab Admin":
                            {
                                var adminid = schoolId;
                                var admininfo = _context.AdminInfoModels.Where(a => a.AdminID == adminid).ToList() ?? throw new Exception("Unable to retrieve admin data for deletion.");

                                var deletion = _context.Remove(admininfo);
                                await _context.SaveChangesAsync();
                                break;
                            }
                        case "Education Support (EA)":
                            {
                                var eaid = schoolId;
                                var eainfo = _context.EASuportInfoModels.Where(a => a.EaID == eaid).ToList() ?? throw new Exception("Unable to retrieve ea data for deletion.");

                                var deletion = _context.Remove(eainfo);
                                await _context.SaveChangesAsync();
                                break;
                            }
                        case "Nurse":
                            {
                                var nurseid = schoolId;
                                var nurseinfo = _context.NurseInfoModels.Where(a => a.NurseID == nurseid).ToList() ?? throw new Exception("Unable to retrieve nurse data for deletion.");

                                var deletion = _context.Remove(nurseinfo);
                                await _context.SaveChangesAsync();
                                break;
                            }
                        case "Law Enforcement":
                            {
                                var adminid = schoolId;
                                var admininfo = _context.LawEnforcementInfoModels.Where(a => a.LawenfID == adminid).ToList() ?? throw new Exception("Unable to retrieve law enforcement officer data for deletion.");

                                var deletion = _context.Remove(admininfo);
                                await _context.SaveChangesAsync();
                                break;
                            }
                        case "Synnovation Lab QR Code Scanner Management":
                            {
                                var adminid = schoolId;
                                var admininfo = _context.SynnLabQRNodeModels.Where(a => a.ScannerID == adminid).ToList() ?? throw new Exception("Unable to retrieve scanner node data for deletion.");

                                var deletion = _context.Remove(admininfo);
                                await _context.SaveChangesAsync();
                                break;
                            }
                        case "Substitute Teacher":
                            {
                                var teacherid = schoolId;
                                var courses = _context.ActiveCourseInfoModels.Where(a => a.CourseTeacherID == teacherid).ToList() ?? throw new Exception("Unable to retrieve teacher's courses for deletion.");
                                var teacherInfo = _context.TeacherInfoModels.Where(a => a.TeacherID == teacherid).ToList() ?? throw new Exception("Unable to retrieve teacher data for deletion.");

                                var deletion1 = _context.Remove(courses);
                                var deletion2 = _context.Remove(teacherInfo);
                                await _context.SaveChangesAsync();
                                break;
                            }
                        case "District Admin":
                            {
                                var adminid = schoolId;
                                var admininfo = _context.AdminInfoModels.Where(a => a.AdminID == adminid).ToList() ?? throw new Exception("Unable to retrieve admin data for deletion.");

                                var deletion = _context.Remove(admininfo);
                                await _context.SaveChangesAsync();
                                break;
                            }
                        case "Developer":
                            {
                                return NotFound("Role not found!");
                            }
                        default:
                            {
                                return NotFound("Role not found.");
                            }
                    }
                }
                var result = await _userManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    // Handle the error
                    throw new Exception("Failed to delete user.");
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogCritical("{Exception}", ex.ToString());

                return View();
            }

        }
    }
}
