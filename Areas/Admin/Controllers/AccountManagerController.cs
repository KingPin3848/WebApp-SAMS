using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SAMS.Controllers;
using SAMS.Data;
using SAMS.Models;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace SAMS.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountManagerController : Controller
    {
        private readonly ILogger<AccountManagerController> _logger;
        private readonly IServiceScopeFactory scopeFactory;
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> userManager;
        //private readonly IEmailSender<ApplicationUser> _emailSender = emailSender;

#pragma warning disable IDE0290 // Use primary constructor
        public AccountManagerController(ILogger<AccountManagerController> logger, IServiceScopeFactory serviceScopeFactory, ApplicationDbContext Context, UserManager<ApplicationUser> UserManager)
#pragma warning restore IDE0290 // Use primary constructor
        {
            _logger = logger;
            scopeFactory = serviceScopeFactory;
            context = Context;
            userManager = UserManager;
        }

        [HttpGet]
        // GET: AccountManager
        public async Task<IActionResult> Index()
        {
            using var scope = scopeFactory.CreateAsyncScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            //EventId = 1; CodeIdentifier = 74
            var users = userManager.Users.ToList();
            List<ApplicationUser> unreals = [];
            foreach (var user in users)
            {
                user.Role = await userManager.GetRolesAsync(user).ConfigureAwait(true);
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
        [HttpGet]
        [RequireHttps]
        public async Task<IActionResult> Details(string? id)
        {
            using var scope = scopeFactory.CreateAsyncScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            if (id == null)
            {
                //EventId = 2; CodeIdentifierNumber = 100
                return NotFound();
            }

            var user = await userManager.FindByIdAsync(id).ConfigureAwait(true);
            if (user == null)
            {
                //EventId = 2; CodeIdentifierNumber = 106
                return NotFound();
            }

            return View(user);
        }



        // GET: AccountManager/Create
        [HttpGet]
        [RequireHttps]
        public ActionResult Create()
        {
            var scope = scopeFactory.CreateAsyncScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            ViewData["RoleId"] = new SelectList(context.Roles, "Name", "Name");
            ViewData["Counselors"] = new SelectList(context.CounselorModels, "CounselorId", "CounselorId");
            ViewData["EAs"] = new SelectList(context.EASuportInfoModels, "EaID", "EaPreferredNameMod");
            return View();
        }

        // POST: AccountManager/Create
        [HttpPost]
        [RequireHttps]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Email,ActivationCode,SchoolId,Role")] InputModel input)
        {
#pragma warning disable CA1031 // Do not catch general exception types
            try
            {
                if (input is null)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    var user = CreateUser();
                    using var scope = scopeFactory.CreateAsyncScope();
                    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                    IEnumerable<string> listRoles = input.Role!;
                    await userManager.SetUserNameAsync(user, input.SchoolId).ConfigureAwait(true);
                    await userManager.SetEmailAsync(user, input.Email).ConfigureAwait(true);
                    user.Role = input.Role;
                    user.SchoolId = input.SchoolId!;
                    user.ActivationCode = input.ActivationCode!;
                    user.UserExperienceEnabled = false;
                    user.Email = input.Email;
                    user.EmailConfirmed = true;
                    var result = await userManager.CreateAsync(user).ConfigureAwait(true);

                    if (result.Succeeded)
                    {
                        //TO UPDATE ALL USER'S INFORMATION AKA ADDING THEIR ACTIVATION CODES, SCHOOLISSUEDID, ETC.
                        var foundUser = await userManager.FindByEmailAsync(input.Email!).ConfigureAwait(true);
                        if (foundUser != null)
                        {
                            var roadroller = await userManager.AddToRolesAsync(foundUser, listRoles).ConfigureAwait(true);
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
                        LoggerMessage.Define(logLevel: LogLevel.Information, eventId: new EventId(30, "User creation"), "User created a new account without a password.");
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

                    foreach(var role in input.Role)
                    {
                        bool worked;
                        switch (role)
                        {
                            case "HS School Admin":
                                {
                                    worked = await AdminCreator(input.SchoolId, input.FirstName, input.MiddleName, input.LastName, input.PreferredName, input.Email, input.PhoneNumber, "High School").ConfigureAwait(true);
                                    break;
                                }

                            case "Synnovation Lab Admin":
                                {
                                    worked = await AdminCreator(input.SchoolId, input.FirstName, input.MiddleName, input.LastName, input.PreferredName, input.Email, input.PhoneNumber, "Synnovation Lab").ConfigureAwait(true);
                                    break;
                                }

                            case "Attendance Office Member":
                                {
                                    worked = await AttOfficeMemberCreator(input.SchoolId, input.FirstName, input.MiddleName, input.LastName, input.PreferredName, input.Email, input.PhoneNumber).ConfigureAwait(true);
                                    break;
                                }

                            case "Nurse":
                                {
                                    worked = await NurseCreator(input.SchoolId, input.FirstName, input.MiddleName, input.LastName, input.PreferredName, input.Email, input.PhoneNumber).ConfigureAwait(true);
                                    break;
                                }

                            case "Law Enforcement":
                                {
                                    worked = await LawEnfCreator(input.SchoolId, input.FirstName, input.MiddleName, input.LastName, input.PreferredName, input.Email, input.PhoneNumber).ConfigureAwait(true);
                                    break;
                                }

                            case "Teacher":
                                {
                                    worked = await TeacherCreator(input.SchoolId, input.FirstName, input.MiddleName, input.LastName, input.PreferredName, input.Email, input.PhoneNumber, input.Teaches5Days, input.RoomAssignedId).ConfigureAwait(true);
                                    break;
                                }

                            //case "Substitute Teacher":
                            //    {
                            //        break;
                            //    }

                            case "Student":
                                {
                                    int studId = new();
                                    if (int.TryParse(input.SchoolId, out studId))
                                    {
                                        //Do nothing
                                    }
                                    else
                                    {
                                        return NotFound();
                                    }
                                    if(input.StudentGradYearMod.HasValue)
                                    {
                                        worked = await StudentCreator(studId, input.FirstName, input.MiddleName, input.LastName, input.PreferredName, input.Email, input.PhoneNumber, input.StudentGradYearMod.Value, input.StudentCounselorID!, input.Parentguard1NameMod!, input.Parentguard1EmailMod!, input.Parentguard2NameMod, input.Parentguard2EmailMod).ConfigureAwait(true);
                                    }
                                    else
                                    {
                                        return NotFound();
                                    }
                                    break;
                                }

                            case "District Admin":
                                {
                                    worked = await AdminCreator(input.SchoolId, input.FirstName, input.MiddleName, input.LastName, input.PreferredName, input.Email, input.PhoneNumber, "District").ConfigureAwait(true);
                                    break;
                                }
                            default:
                                {
                                    worked = true;
                                    break;
                                }
                        }
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentNullException ex)
            {
                LoggerMessage.Define(logLevel: LogLevel.Critical, eventId: new EventId(1, "Argument Null Exc."), $"An exception occurred. Please contact the admins or developers for the issue to be resolved. \n\n Message = {ex.Message}");
                LoggerMessage.Define(logLevel: LogLevel.Critical, eventId: new EventId(1, "Argument Null Exc."), $"Source: \n\n {ex.Source}");
                LoggerMessage.Define(logLevel: LogLevel.Critical, eventId: new EventId(1, "Argument Null Exc."), $"Inner Exception: \n\n {ex.InnerException}");
                LoggerMessage.Define(logLevel: LogLevel.Critical, eventId: new EventId(1, "Argument Null Exc."), $"Stack Trace: \n\n {ex.StackTrace}");
                LoggerMessage.Define(logLevel: LogLevel.Critical, eventId: new EventId(1, "Argument Null Exc."), $"Target Site/Method: \n\n {ex.TargetSite}");
                return View();
            }
            catch (ArgumentException ex)
            {
                LoggerMessage.Define(logLevel: LogLevel.Critical, eventId: new EventId(1, "Argument Exc."), $"Argument Exception occurred. \n\n Message = {ex.Message}");
                LoggerMessage.Define(logLevel: LogLevel.Critical, eventId: new EventId(1, "Argument Exc."), $"Source: \n\n {ex.Source}");
                LoggerMessage.Define(logLevel: LogLevel.Critical, eventId: new EventId(1, "Argument Exc."), $"Inner Exception: \n\n {ex.InnerException}");
                LoggerMessage.Define(logLevel: LogLevel.Critical, eventId: new EventId(1, "Argument Exc."), $"Stack Trace: \n\n {ex.StackTrace}");
                LoggerMessage.Define(logLevel: LogLevel.Critical, eventId: new EventId(1, "Argument Exc."), $"Target Site/Method: \n\n {ex.TargetSite}");
                return View();
            }
            catch (InvalidOperationException ex)
            {
                LoggerMessage.Define(logLevel: LogLevel.Critical, eventId: new EventId(1, "Invalid Operation Exc."), $"Invalid Operation Exception occurred. \n\n Message = {ex.Message}");
                LoggerMessage.Define(logLevel: LogLevel.Critical, eventId: new EventId(1, "Invalid Operation Exc."), $"Source: \n\n {ex.Source}");
                LoggerMessage.Define(logLevel: LogLevel.Critical, eventId: new EventId(1, "Invalid Operation Exc."), $"Inner Exception: \n\n {ex.InnerException}");
                LoggerMessage.Define(logLevel: LogLevel.Critical, eventId: new EventId(1, "Invalid Operation Exc."), $"Stack Trace: \n\n {ex.StackTrace}");
                LoggerMessage.Define(logLevel: LogLevel.Critical, eventId: new EventId(1, "Invalid Operation Exc."), $"Target Site/Method: \n\n {ex.TargetSite}");
                return View();
            }
            catch (Exception ex)
            {
                LoggerMessage.Define(logLevel: LogLevel.Critical, eventId: new EventId(1, "Exception"), $"Exception occurred. \n\n Message = {ex.Message}");
                LoggerMessage.Define(logLevel: LogLevel.Critical, eventId: new EventId(1, "Exception"), $"Source: \n\n {ex.Source}");
                LoggerMessage.Define(logLevel: LogLevel.Critical, eventId: new EventId(1, "Exception"), $"Inner Exception: \n\n {ex.InnerException}");
                LoggerMessage.Define(logLevel: LogLevel.Critical, eventId: new EventId(1, "Exception"), $"Stack Trace: \n\n {ex.StackTrace}");
                LoggerMessage.Define(logLevel: LogLevel.Critical, eventId: new EventId(1, "Exception"), $"Target Site/Method: \n\n {ex.TargetSite}");
                return View();
            }
#pragma warning restore CA1031 // Do not catch general exception types

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

        private async Task<bool> TeacherCreator(string id, string fname, string? mname, string lname, string? pname, string email, string? phone, bool? days, int? room)
        {
            TeacherInfoModel teacher = new()
            {
                TeacherID = id,
                TeacherFirstNameMod = fname,
                TeacherMiddleNameMod = mname,
                TeacherLastNameMod = lname,
                TeacherPreferredNameMod = pname,
                TeacherEmailMod = email,
                TeacherPhoneMod = phone,
                Teaches5Days = days,
                RoomAssignedId = room
            };

            using var scope = scopeFactory.CreateAsyncScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            await context.TeacherInfoModels.AddAsync(teacher).ConfigureAwait(true);
            return (context.SaveChangesAsync().IsCompletedSuccessfully);
        }
        private async Task<bool> AdminCreator(string id, string fname, string? mname, string lname, string? pname, string email, string? phone, string label)
        {
            AdminInfoModel admin = new()
            {
                AdminID = id,
                AdminFirstNameMod = fname,
                AdminMiddleNameMod = mname,
                AdminLastNameMod = lname,
                AdminPreferredNameMod = pname,
                AdminEmailMod = email,
                AdminPhoneMod = phone,
                AdminLabelMod = label
            };

            using var scope = scopeFactory.CreateAsyncScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            await context.AdminInfoModels.AddAsync(admin).ConfigureAwait(true);
            return (context.SaveChangesAsync().IsCompletedSuccessfully);
        }
        private static string TypeOfAdminDetermination(List<string> roles)
        {

            foreach (var role in roles)
            {
                if (role.Contains("Admin", StringComparison.Ordinal))
                {
                    return role;
                }
            }

            return "Not an admin.";
        }
        private async Task<bool> AttOfficeMemberCreator(string id, string fname, string? mname, string lname, string? pname, string email, string? phone)
        {
            AttendanceOfficeMemberModel attendanceMember = new()
            {
                AoMemberID = id,
                AoMemberFirstNameMod = fname,
                AoMemberMiddleNameMod = mname,
                AoMemberLastNameMod = lname,
                AoMemberPreferredNameMod = pname,
                AoMemberEmailMod = email,
                AoMemberPhoneMod = phone
            };

            using var scope = scopeFactory.CreateAsyncScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            await context.AttendanceOfficeMemberModels.AddAsync(attendanceMember).ConfigureAwait(true);
            return (context.SaveChangesAsync().IsCompletedSuccessfully);
        }
        private async Task<bool> NurseCreator(string id, string fname, string? mname, string lname, string? pname, string email, string? phone)
        {
            NurseInfoModel nurse = new()
            {
                NurseID = id,
                NurseFirstNameMod = fname,
                NurseMiddleNameMod = mname,
                NurseLastNameMod = lname,
                NursePreferredNameMod = pname,
                NurseEmailMod = email,
                NursePhoneMod = phone
            };

            using var scope = scopeFactory.CreateAsyncScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            await context.NurseInfoModels.AddAsync(nurse).ConfigureAwait(true);
            return (context.SaveChangesAsync().IsCompletedSuccessfully);
        }
        private async Task<bool> LawEnfCreator(string id, string fname, string? mname, string lname, string? pname, string email, string? phone)
        {
            LawEnforcementInfoModel lawenf = new()
            {
                LawenfID = id,
                LaweFirstNameMod = fname,
                LaweMiddleNameMod = mname,
                LaweLastNameMod = lname,
                LawePreferredNameMod = pname,
                LaweEmailMod = email,
                LawePhoneMod = phone
            };

            using var scope = scopeFactory.CreateAsyncScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            await context.LawEnforcementInfoModels.AddAsync(lawenf).ConfigureAwait(true);
            return (context.SaveChangesAsync().IsCompletedSuccessfully);
        }
        private async Task<bool> StudentCreator(int id, string fname, string? mname, string lname, string? pname, string email, string? phone, DateTime date, string cslrid, string p1name, string p1email, string? p2name, string? p2email)
        {
            StudentInfoModel student = new()
            {
                StudentID = id,
                StudentFirstNameMod = fname,
                StudentMiddleNameMod = mname,
                StudentLastNameMod = lname,
                StudentPreferredNameMod = pname,
                StudentEmailMod = email,
                StudentPhoneMod = phone,
                StudentGradYearMod = date,
                StudentCounselorID = cslrid,
                Parentguard1NameMod = p1name,
                Parentguard1EmailMod = p1email,
                Parentguard2NameMod = p2name,
                Parentguard2EmailMod = p2email
            };

            using var scope = scopeFactory.CreateAsyncScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            await context.StudentInfoModels.AddAsync(student).ConfigureAwait(true);
            return (context.SaveChangesAsync().IsCompletedSuccessfully);
        }
        private string CounselorIdReturn(string fname)
        {
            using var scope = scopeFactory.CreateAsyncScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            return context.CounselorModels.Where(a => a.CounselorFirstName == fname).Select(a => a.CounselorId).First();
        }
        private async Task<bool> CounselorCreator(string id, string fname, string? mname, string lname, string? pname, string email, string? phone)
        {
            CounselorModel counselor = new()
            {
                CounselorId = id,
                CounselorFirstName = fname,
                CounselorMiddleName = mname,
                CounselorLastName = lname,
                CounselorPreferredName = pname,
                CounselorEmail = email,
                CounselorPhone = phone
            };

            using var scope = scopeFactory.CreateAsyncScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            await context.CounselorModels.AddAsync(counselor).ConfigureAwait(true);
            return (context.SaveChangesAsync().IsCompletedSuccessfully);
        }
        /*private async Task<bool> SubCreator(string id, string fname, string? mname, string lname, string? pname, string email, string? phone, bool? days, int? room)
        {
            SubTeacherModel sub = new()
            {
                SubID = id,
                SubFirstNameMod = fname,
                SubMiddleNameMod = mname,
                SubLastNameMod = lname,
                SubPreferredNameMod = pname,
                SubEmailMod = email,
                SubPhoneMod = phone
            };

            using var scope = scopeFactory.CreateAsyncScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            await context.TeacherInfoModels.AddAsync(sub).ConfigureAwait(true);
            return (context.SaveChangesAsync().IsCompletedSuccessfully);
        }*/


        // GET: AccountManager/Edit/5
        [HttpGet]
        [RequireHttps]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await userManager.FindByIdAsync(id).ConfigureAwait(true);
            if (user == null)
            {
                return NotFound();
            }

            ViewData["RoleId"] = new SelectList(context.Roles, "Name", "Name", user.Role);
            return View(user);
        }




        // POST: AccountManager/Edit/5
        [HttpPost]
        [RequireHttps]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, ApplicationUser model)
        {
            if (model is null)
            {
                return NotFound();
            }

            if (id != model.Id)
            {
                return NotFound();
            }

            using var scope = scopeFactory.CreateAsyncScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            if (ModelState.IsValid)
            {
                try
                {
                    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                    var user = await userManager.FindByIdAsync(id).ConfigureAwait(true);
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
                    var currentRoles = await userManager.GetRolesAsync(user).ConfigureAwait(true);

                    // Remove the user from the current roles
                    var removeResult = await userManager.RemoveFromRolesAsync(user, currentRoles).ConfigureAwait(true);
                    if (!removeResult.Succeeded)
                    {
                        LoggerMessage.Define(logLevel: LogLevel.Error, eventId: new EventId(2, "Task failure."), "Failed to remove user roles.");
                        // Handle the error
                        return RedirectToAction("Error", new { Message = "Failed to remove user roles." });
                    }

                    // Add the user to the new role
                    var addResult = await userManager.AddToRolesAsync(user, model.Role!).ConfigureAwait(true);
                    if (!addResult.Succeeded)
                    {
                        LoggerMessage.Define(logLevel: LogLevel.Error, eventId: new EventId(2, "Task failure."), "Failed to add user roles.");
                        // Handle the error
                        return RedirectToAction("Error", new { Message = "Failed to add user roles." });
                    }

                    await userManager.UpdateAsync(user).ConfigureAwait(true);
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
                        LoggerMessage.Define(logLevel: LogLevel.Critical, eventId: new EventId(3, "ModelState error."), $"{modelError.ErrorMessage}, \n\n {modelError.Exception}");
                    }
                }
            }


            ViewData["RoleId"] = new SelectList(context.Roles, "Name", "Name", model.Role);
            return View(model);

        }

        private bool UserExists(string schoolId)
        {
            using var scope = scopeFactory.CreateAsyncScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            return context.Users.Any(e => e.SchoolId == schoolId);
        }




        // GET: AccountManager/Delete/5
        [HttpGet]
        [RequireHttps]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            using var scope = scopeFactory.CreateAsyncScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            var user = await userManager.FindByIdAsync(id).ConfigureAwait(true);
            if (user == null)
            {
                return NotFound();
            }

            var roles = await userManager.GetRolesAsync(user).ConfigureAwait(true) ?? throw new InvalidOperationException("Unable to retrieve user roles. Please make sure you have assigned roles to the selected user. Use the Edit action to add roles to the user.");
            foreach (var role in roles)
            {
                if (role == null)
                {
                    return RedirectToAction("Error", new { Message = "Couldn't retrieve a role for the user." });

                    //throw new ArgumentNullException(nameof(id));
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
#pragma warning disable CA1031 // Do not catch general exception types
            try
            {
                using var scope = scopeFactory.CreateAsyncScope();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                var user = await userManager.FindByIdAsync(id).ConfigureAwait(true);
                if (user == null)
                {
                    return NotFound();
                }

                var schoolId = user.SchoolId ?? null;
                if (schoolId is null)
                {
                    return NotFound("Failed to retrieve user's School Id.");
                }

                var roles = await userManager.GetRolesAsync(user).ConfigureAwait(true) ?? null;
                if (roles == null)
                {
                    return RedirectToAction("Error", new { Message = "Couldn't retrieve a role for the user." });
                }
                else
                {
                    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    foreach (var role in roles)
                    {
                        switch (role)
                        {
                            case "Student":
                                {
                                    int studId = new();
                                    if (int.TryParse(schoolId, out studId))
                                    {
                                        //Do nothing
                                    }
                                    else
                                    {
                                        return NotFound("Could not parse the school id for student. Please make sure the student Id is correct.");
                                    }

                                    var studentInfo = await context.StudentInfoModels.FindAsync(studId).ConfigureAwait(true) ?? throw new InvalidOperationException("Unable to retrieve student data for deletion.");
                                    var sem1sched = await context.Sem1StudSchedules.FindAsync(studId).ConfigureAwait(true) ?? throw new InvalidOperationException("Unable to retrieve student schedule 1 for deletion.");
                                    var sem2sched = await context.Sem2StudSchedules.FindAsync(studId).ConfigureAwait(true) ?? throw new InvalidOperationException("Unable to retrieve student schedule 2 for deletion.");

                                    var studInfoDeletion = context.StudentInfoModels.Remove(studentInfo);
                                    var sem1schedDeletion = context.Sem1StudSchedules.Remove(sem1sched);
                                    var sem2schedDeletion = context.Sem2StudSchedules.Remove(sem2sched);
                                    await context.SaveChangesAsync().ConfigureAwait(true);
                                    break;
                                }
                            case "Teacher":
                                {
                                    var teacherid = schoolId;
                                    List<ActiveCourseInfoModel>? courses = context.ActiveCourseInfoModels.Where(a => a.CourseTeacherID == teacherid).ToList();
                                    List<TeacherInfoModel>? teacherInfo = context.TeacherInfoModels.Where(a => a.TeacherID == teacherid).ToList();

                                    if (courses.Count == 0)
                                    {
#pragma warning disable CA1848 // Use the LoggerMessage delegates
                                        _logger.LogInformation("Courses couldn't be found. Unable to retrieve teacher's courses for deletion.");
#pragma warning restore CA1848 // Use the LoggerMessage delegates
                                    }
                                    else
                                    {
                                        var deletion1 = context.Remove(courses);
                                        await context.SaveChangesAsync().ConfigureAwait(true);
                                    }

                                    if (teacherInfo.Count == 0)
                                    {
#pragma warning disable CA1848 // Use the LoggerMessage delegates
                                        _logger.LogInformation("Teacher information couldn't be found. Unable to retrieve teacher data for deletion.");
#pragma warning restore CA1848 // Use the LoggerMessage delegates
                                    }
                                    else
                                    {
                                        var deletion2 = context.Remove(teacherInfo);
                                        await context.SaveChangesAsync().ConfigureAwait(true);
                                    }

                                    break;
                                }
                            case "Attendance Office Member":
                                {
                                    var attid = schoolId;
                                    var attinfo = context.AttendanceOfficeMemberModels.Where(a => a.AoMemberID == attid).ToList() ?? throw new InvalidOperationException("Unable to retrieve attendance office member data for deletion.");

                                    var deletion = context.Remove(attinfo);
                                    await context.SaveChangesAsync().ConfigureAwait(true);
                                    break;
                                }
                            case "HS School Admin":
                                {
                                    var adminid = schoolId;
                                    var admininfo = context.AdminInfoModels.Where(a => a.AdminID == adminid).ToList() ?? throw new InvalidOperationException("Unable to retrieve admin data for deletion.");

                                    var deletion = context.Remove(admininfo);
                                    await context.SaveChangesAsync().ConfigureAwait(true);
                                    break;
                                }
                            case "Synnovation Lab Admin":
                                {
                                    var adminid = schoolId;
                                    var admininfo = context.AdminInfoModels.Where(a => a.AdminID == adminid).ToList();

                                    if (admininfo.Count == 0)
                                    {
                                        _logger.LogCritical("adminInfo is empty.");
                                    }
                                    else
                                    {
                                        var deletion = context.Remove(admininfo);
                                        await context.SaveChangesAsync().ConfigureAwait(true);
                                    }

                                    break;
                                }
                            case "Education Support (EA)":
                                {
                                    var eaid = schoolId;
                                    var eainfo = context.EASuportInfoModels.Where(a => a.EaID == eaid).ToList() ?? throw new InvalidOperationException("Unable to retrieve ea data for deletion.");

                                    var deletion = context.Remove(eainfo);
                                    await context.SaveChangesAsync().ConfigureAwait(true);
                                    break;
                                }
                            case "Nurse":
                                {
                                    var nurseid = schoolId;
                                    var nurseinfo = context.NurseInfoModels.Where(a => a.NurseID == nurseid).ToList() ?? throw new InvalidOperationException("Unable to retrieve nurse data for deletion.");

                                    var deletion = context.Remove(nurseinfo);
                                    await context.SaveChangesAsync().ConfigureAwait(true);
                                    break;
                                }
                            case "Law Enforcement":
                                {
                                    var adminid = schoolId;
                                    var admininfo = context.LawEnforcementInfoModels.Where(a => a.LawenfID == adminid).ToList() ?? throw new InvalidOperationException("Unable to retrieve law enforcement officer data for deletion.");

                                    var deletion = context.Remove(admininfo);
                                    await context.SaveChangesAsync().ConfigureAwait(true);
                                    break;
                                }
                            //case "Synnovation Lab QR Code Scanner Management":
                            //    {
                            //        var adminid = schoolId;
                            //        var admininfo = context.HandheldScannerNodeModels.Where(a => a.ScannerID == adminid).ToList() ?? throw new Exception("Unable to retrieve scanner node data for deletion.");

                            //        var deletion = context.Remove(admininfo);
                            //        await context.SaveChangesAsync().ConfigureAwait(true);
                            //        break;
                            //    }
                            case "Substitute Teacher":
                                {
                                    var teacherid = schoolId;
                                    var courses = context.ActiveCourseInfoModels.Where(a => a.CourseTeacherID == teacherid).ToList() ?? throw new InvalidOperationException("Unable to retrieve teacher's courses for deletion.");
                                    var teacherInfo = context.TeacherInfoModels.Where(a => a.TeacherID == teacherid).ToList() ?? throw new InvalidOperationException("Unable to retrieve teacher data for deletion.");

                                    var deletion1 = context.Remove(courses);
                                    var deletion2 = context.Remove(teacherInfo);
                                    await context.SaveChangesAsync().ConfigureAwait(true);
                                    break;
                                }
                            case "District Admin":
                                {
                                    var adminid = schoolId;
                                    var admininfo = context.AdminInfoModels.Where(a => a.AdminID == adminid).ToList() ?? throw new InvalidOperationException("Unable to retrieve admin data for deletion.");

                                    var deletion = context.Remove(admininfo);
                                    await context.SaveChangesAsync().ConfigureAwait(true);
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

                    var result = await userManager.DeleteAsync(user).ConfigureAwait(true);
                    if (!result.Succeeded)
                    {
                        // Handle the error
                        return NotFound();
                        //throw new InvalidOperationException("Failed to delete user.");
                    }

                    return RedirectToAction(nameof(Index));
                }
            }
            catch (InvalidOperationException ex)
            {
                LoggerMessage.Define(logLevel: LogLevel.Critical, eventId: new EventId(1, "Data retrieval failed."), $"Something went wrong. Following are the details. \n\n {ex.Message} \n\n {ex.Data} \n\n {ex.InnerException} \n\n {ex.Source} \n\n {ex.StackTrace} \n\n {ex.TargetSite}");
                return View();
            }
            catch (Exception ex)
            {
                LoggerMessage.Define(logLevel: LogLevel.Critical, eventId: new EventId(1, "Data retrieval failed."), $"Something went wrong. Following are the details. \n\n {ex.Message} \n\n {ex.Data} \n\n {ex.InnerException} \n\n {ex.Source} \n\n {ex.StackTrace} \n\n {ex.TargetSite}");

                return View();
            }
#pragma warning restore CA1031 // Do not catch general exception types

        }



        [HttpGet]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(string Message)
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, Message = Message });
        }
    }

    public class InputModel
    {
        //ALL REQUIRED ACCOUNT INFORMATION STARTS FROM HERE
        [EmailAddress]
        [Display(Name = "Email")]
        public required string Email { get; set; }


        [StringLength(32, ErrorMessage = "The Unique Code must be at least {2} and at max {1} characters long.", MinimumLength = 32)]
        [Display(Name = "Unique Code")]
        public required string ActivationCode { get; set; }


        [Display(Name = "School Id")]
        public required string SchoolId { get; set; }


        [Display(Name = "Role(s)")]
        [Required]
        public required IList<string> Role { get; init; }
        //ALL REQUIRED ACCOUNT INFORMATION ENDS HERE



        //ALL COMMON REQUIRED AND OPTIONAL PERSONALLY IDENTIFIABLE INFORMATION OR PROTECTED PERSONAL INFORMATION STARTS HERE
        [Display(Name = "First Name")]
        [Required]
        public required string FirstName { get; init; }


        [Display(Name = "Middle Name (Optional)")]
        public string? MiddleName { get; init; }


        [Display(Name = "Last Name")]
        public required string LastName { get; init; }


        [Display(Name = "Preferred Name (Optional)")]
        public string? PreferredName { get; init; }


        [Display(Name = "Phone Number/School Extension (Optional)")]
        public string? PhoneNumber { get; init; }
        //ALL COMMON REQUIRED PERSONALLY IDENTIFIABLE INFORMATION OR PROTECTED PERSONAL INFORMATION ENDS HERE



        //ALL ADMIN REQUIRED INFORMATION STARTS HERE
        //ALL ADMIN REQUIRED INFORMATION ENDS HERE



        //ALL ATT. OFFICE MEMBER REQUIRED INFORMATION STARTS HERE
        //ALL ATT. OFFICE MEMBER REQUIRED INFORMATION STARTS HERE



        //ALL COUNSELOR REQUIRED INFORMATION STARTS HERE
        //ALL COUNSELOR REQUIRED INFORMATION ENDS HERE



        //ALL EA REQUIRED INFORMATION STARTS HERE
        //ALL EA REQUIRED INFORMATION ENDS HERE



        //ALL LAW-ENF REQUIRED INFORMATION STARTS HERE
        //ALL LAW-ENF REQUIRED INFORMATION ENDS HERE



        //ALL NURSE REQUIRED INFORMATION STARTS HERE
        //ALL NURSE REQUIRED INFORMATION ENDS HERE



        //ALL STUDENT REQUIRED INFORMATION STARTS HERE
        [Display(Name = ("Student Graduation Year (Required)"))]
        public DateTime? StudentGradYearMod { get; set; }


        [Display(Name = ("Assigned Counselor (Required)"))]
        public string? StudentCounselorID { get; set; }


        [Display(Name = ("EA Support?"))]
        public Boolean? HasEASupport { get; set; }


        [Display(Name = ("EA ID"))]
        public string? StudentEAID { get; set; }


        [Display(Name = ("Parent/Guardian 1 Name (Required)"))]
        public string? Parentguard1NameMod { get; set; }


        [Display(Name = ("Parent/Guardian 1 Email Address (Required)"))]
        [EmailAddress]
        public string? Parentguard1EmailMod { get; set; }


        [Display(Name = ("Parent/Guardian 2 Name (Optional)"))]
        public string? Parentguard2NameMod { get; set; }


        [Display(Name = ("Parent/Guardian 2 Email Address (Optional)"))]
        public string? Parentguard2EmailMod { get; set; }
        //ALL STUDENT REQUIRED INFORMATION ENDS HERE



        //ALL TEACHER REQUIRED INFORMATION STARTS HERE
        [Display(Name = ("Teaches all 5 days?"))]
        public Boolean? Teaches5Days { get; set; }

        [Display(Name = "Assigned Room")]
        public int? RoomAssignedId { get; set; }
        //ALL TEACHER REQUIRED INFORMATION ENDS HERE
    }
}
