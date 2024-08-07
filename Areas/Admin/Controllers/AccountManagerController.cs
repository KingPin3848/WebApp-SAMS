using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SAMS.Controllers;
using SAMS.Data;
using SAMS.Models;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Globalization;
using System.Security.Claims;

namespace SAMS.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountManagerController : Controller
    {
        private readonly ILogger<AccountManagerController> _logger;
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> userManager;

        //private readonly IEmailSender<ApplicationUser> _emailSender = emailSender;

        public AccountManagerController(ILogger<AccountManagerController> logger, ApplicationDbContext Context, UserManager<ApplicationUser> UserManager)
        {
            _logger = logger;
            context = Context;
            userManager = UserManager;
        }

        [HttpGet]
        // GET: AccountManager
        public async Task<IActionResult> Index()
        {
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

            var loggedinuserid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (loggedinuserid is null)
            {
                return RedirectToAction("AutomatedError", "Error", new { number = 01, description = "Are you logged in?", reference = "Event ID = 1, Identifier = Acc72Det"});
            }

            var loggedinuser = await userManager.FindByIdAsync(loggedinuserid).ConfigureAwait(true);
            if (loggedinuser is null)
            {
                return RedirectToAction("AutomatedError", "Error", new { number = 01, description = "Are you an alien? We couldn't authenticate you.", reference = "Event ID = 1, Identifier = Acc78Det"});
            }


            if (id == null)
            {
                //EventId = 2; CodeIdentifierNumber = Acc100Det
                return RedirectToAction("AutomatedError", "Error", new { number = 02, description = "Did you forget to select a user?", reference = "Event ID = 2, Identifier = Acc100Det", user = loggedinuser });
            }

            var user = await userManager.FindByIdAsync(id).ConfigureAwait(true);
            if (user == null)
            {
                //EventId = 2; CodeIdentifierNumber = 106
                return RedirectToAction("AutomatedError", "Error", new { number = 02, description = "Seems like you are not a registered user. Make sure the username is spelled correctly or logged in with your school Google Account.", reference = "Event ID = 2, Identifier = Acc100Det", user = loggedinuser });
            }

            return View(user);

        }



        // GET: AccountManager/Create
        [HttpGet]
        [RequireHttps]
        public ActionResult Create()
        {

            ViewData["RoleId"] = new SelectList(context.Roles, "Name", "Name");
            ViewData["Counselors"] = new SelectList(context.CounselorModels, "CounselorId", "CounselorId");
            ViewData["Rooms"] = new SelectList(context.RoomLocationInfoModels, "RoomNumberMod", "RoomNumberMod");
            return View();
        }

        // POST: AccountManager/Create
        [HttpPost]
        [RequireHttps]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InputModel input)
        {
#pragma warning disable CA1031 // Do not catch general exception types
            try
            {
                //var loggedinuserid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                //if (loggedinuserid is null)
                //{
                //    return RedirectToAction("Error", "Error", new { number = 01, description = "Are you logged in?", reference = "Event ID = 1, Identifier = Acc72Det"});
                //}

                //var loggedinuser = await userManager.FindByIdAsync(loggedinuserid).ConfigureAwait(true);
                //if (loggedinuser is null)
                //{
                //    return RedirectToAction("Error", "Error", new { number = 01, description = "Are you an alien? We couldn't authenticate you.", reference = "Event ID = 1, Identifier = Acc78Det"});
                //}

                if (input is null)
                {
                    return NotFound();
                    //return RedirectToAction("Error", "Error", new { number = 02, description = "Did you forget to enter data into correct fields?", reference = "Event ID = 2, Identifier = Acc100Det", user = loggedinuser });
                }

                if (ModelState.IsValid)
                {
                    var user = CreateUser();

                    IEnumerable<string> listRoles = input.Role!;
                    user.Role = input.Role;
                    user.SchoolId = input.SchoolId!;
                    user.ActivationCode = input.ActivationCode!;
                    user.UserExperienceEnabled = false;
                    user.Email = input.Email;
                    user.EmailConfirmed = true;
                    await userManager.SetUserNameAsync(user, input.SchoolId).ConfigureAwait(true);
                    await userManager.SetEmailAsync(user, input.Email).ConfigureAwait(true);
                    var result = await userManager.CreateAsync(user).ConfigureAwait(true);
                    var roadroller = await userManager.AddToRolesAsync(user, listRoles).ConfigureAwait(true);

                    if (result.Succeeded)
                    {
                        LoggerMessage.Define(logLevel: LogLevel.Information, eventId: new EventId(30, "User creation"), "User created a new account without a password.");
                        //SNIPPET 1 FOR EMAIL GOES HERE

                        foreach (var role in input.Role)
                        {
                            bool worked;
                            switch (role)
                            {
                                case "HS School Admin":
                                    {
                                        worked = await AdminCreator(input.SchoolId, input.FirstName, input.MiddleName, input.LastName, input.PreferredName, input.Email, input.PhoneNumber, "High School").ConfigureAwait(true);
                                        break;
                                    }

                                case "Counselor":
                                    {
                                        worked = await CounselorCreator(input.SchoolId, input.FirstName, input.MiddleName, input.LastName, input.PreferredName, input.Email, input.PhoneNumber).ConfigureAwait(true);
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
                                            //return RedirectToAction("Error", "Error", new { number = 02, description = "Did you write a letter instead of number for student id?", reference = "Event ID = 2, Identifier = Acc236Cret", user = loggedinuser });
                                        }
                                        if (input.StudentGradYearMod.HasValue)
                                        {
                                            worked = await StudentCreator(studId, input.FirstName, input.MiddleName, input.LastName, input.PreferredName, input.Email, input.PhoneNumber, input.StudentGradYearMod.Value, input.StudentCounselorID!, input.Parentguard1NameMod!, input.Parentguard1EmailMod!, input.Parentguard2NameMod, input.Parentguard2EmailMod).ConfigureAwait(true);
                                        }
                                        else
                                        {
                                            return NotFound();
                                            //return RedirectToAction("Error", "Error", new { number = 02, description = "Did you forget to add the student's grad year?", reference = "Event ID = 2, Identifier = Acc244Cret", user = loggedinuser });
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

            await context.AdminInfoModels.AddAsync(admin).ConfigureAwait(true);
            return (context.SaveChangesAsync().IsCompletedSuccessfully);
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

            await context.StudentInfoModels.AddAsync(student).ConfigureAwait(true);
            return (context.SaveChangesAsync().IsCompletedSuccessfully);
        }
        private string CounselorIdReturn(string fname)
        {
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

            await context.TeacherInfoModels.AddAsync(sub).ConfigureAwait(true);
            return (context.SaveChangesAsync().IsCompletedSuccessfully);
        }*/


        // GET: AccountManager/Edit/5
        [HttpGet]
        [RequireHttps]
        public async Task<IActionResult> Edit(string id)
        {
            //var loggedinuserid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //if (loggedinuserid is null)
            //{
            //    return RedirectToAction("Error", "Error", new { number = 01, description = "Are you logged in?", reference = "Event ID = 1, Identifier = Acc72Det"});
            //}

            //var loggedinuser = await userManager.FindByIdAsync(loggedinuserid).ConfigureAwait(true);
            //if (loggedinuser is null)
            //{
            //    return RedirectToAction("Error", "Error", new { number = 01, description = "Are you an alien? We couldn't authenticate you.", reference = "Event ID = 1, Identifier = Acc78Det"});
            //}

            if (id == null)
            {
                return NotFound();
            }

            var user = await userManager.FindByIdAsync(id).ConfigureAwait(true);
            if (user == null)
            {
                return NotFound();
            }
            var userroles = await userManager.GetRolesAsync(user).ConfigureAwait(true);
            InputModel inputmodel = new()
            {
                Email = string.Empty,
                FirstName = string.Empty,
                LastName = string.Empty,
                ActivationCode = user.ActivationCode,
                Role = user.Role,
                SchoolId = string.Empty
            };
            foreach (var role in userroles)
            {
                switch (role)
                {
                    case "HS School Admin":
                        {
                            var info = context.AdminInfoModels.Where(a => a.AdminID == user.SchoolId).First();
                            inputmodel.SchoolId = info.AdminID;
                            inputmodel.FirstName = info.AdminFirstNameMod;
                            inputmodel.MiddleName = info.AdminMiddleNameMod;
                            inputmodel.LastName = info.AdminLastNameMod;
                            inputmodel.PreferredName = info.AdminPreferredNameMod;
                            inputmodel.PhoneNumber = info.AdminPhoneMod;
                            inputmodel.Email = info.AdminEmailMod;
                            break;
                        }
                    case "Synnovation Lab Admin":
                        {
                            var info = context.AdminInfoModels.Where(a => a.AdminID == user.SchoolId).First();
                            inputmodel.SchoolId = info.AdminID;
                            inputmodel.FirstName = info.AdminFirstNameMod;
                            inputmodel.MiddleName = info.AdminMiddleNameMod;
                            inputmodel.LastName = info.AdminLastNameMod;
                            inputmodel.PreferredName = info.AdminPreferredNameMod;
                            inputmodel.PhoneNumber = info.AdminPhoneMod;
                            inputmodel.Email = info.AdminEmailMod;
                            break;
                        }
                    case "Attendance Office Member":
                        {
                            var info = context.AttendanceOfficeMemberModels.Where(a => a.AoMemberID == user.SchoolId).First();
                            inputmodel.SchoolId = info.AoMemberID;
                            inputmodel.FirstName = info.AoMemberFirstNameMod;
                            inputmodel.MiddleName = info.AoMemberMiddleNameMod;
                            inputmodel.LastName = info.AoMemberLastNameMod;
                            inputmodel.PreferredName = info.AoMemberPreferredNameMod;
                            inputmodel.PhoneNumber = info.AoMemberPhoneMod;
                            inputmodel.Email = info.AoMemberEmailMod;
                            break;
                        }
                    case "Nurse":
                        {
                            var info = context.NurseInfoModels.Where(a => a.NurseID == user.SchoolId).First();
                            inputmodel.SchoolId = info.NurseID;
                            inputmodel.FirstName = info.NurseFirstNameMod;
                            inputmodel.MiddleName = info.NurseMiddleNameMod;
                            inputmodel.LastName = info.NurseLastNameMod;
                            inputmodel.PreferredName = info.NursePreferredNameMod;
                            inputmodel.PhoneNumber = info.NursePhoneMod;
                            inputmodel.Email = info.NurseEmailMod;
                            break;
                        }
                    case "Law Enforcement":
                        {
                            var info = context.LawEnforcementInfoModels.Where(a => a.LawenfID == user.SchoolId).First();
                            inputmodel.SchoolId = info.LawenfID;
                            inputmodel.FirstName = info.LaweFirstNameMod;
                            inputmodel.MiddleName = info.LaweMiddleNameMod;
                            inputmodel.LastName = info.LaweLastNameMod;
                            inputmodel.PreferredName = info.LawePreferredNameMod;
                            inputmodel.PhoneNumber = info.LawePhoneMod;
                            inputmodel.Email = info.LaweEmailMod;
                            break;
                        }
                    case "Teacher":
                        {
                            var info = context.TeacherInfoModels.Where(a => a.TeacherID == user.SchoolId).First();
                            inputmodel.SchoolId = info.TeacherID;
                            inputmodel.FirstName = info.TeacherFirstNameMod;
                            inputmodel.MiddleName = info.TeacherMiddleNameMod;
                            inputmodel.LastName = info.TeacherLastNameMod;
                            inputmodel.PreferredName = info.TeacherPreferredNameMod;
                            inputmodel.PhoneNumber = info.TeacherPhoneMod;
                            inputmodel.Email = info.TeacherEmailMod;
                            inputmodel.Teaches5Days = info.Teaches5Days;
                            inputmodel.RoomAssignedId = info.RoomAssignedId;
                            break;
                        }
                    case "Student":
                        {
                            if (int.TryParse(user.SchoolId, out int studentid))
                            {
                                var info = context.StudentInfoModels.Where(a => a.StudentID == studentid).First();
                                inputmodel.SchoolId = studentid.ToString(CultureInfo.CurrentCulture);
                                inputmodel.FirstName = info.StudentFirstNameMod;
                                inputmodel.MiddleName = info.StudentMiddleNameMod;
                                inputmodel.LastName = info.StudentLastNameMod;
                                inputmodel.PreferredName = info.StudentPreferredNameMod;
                                inputmodel.PhoneNumber = info.StudentPhoneMod;
                                inputmodel.Email = info.StudentEmailMod;
                                inputmodel.Parentguard1NameMod = info.Parentguard1NameMod;
                                inputmodel.Parentguard1EmailMod = info.Parentguard1EmailMod;
                                inputmodel.Parentguard2NameMod = info.Parentguard2NameMod;
                                inputmodel.Parentguard2EmailMod = info.Parentguard2EmailMod;
                            }
                            break;
                        }
                    case "District Admin":
                        {
                            var info = context.AttendanceOfficeMemberModels.Where(a => a.AoMemberID == user.SchoolId).First();
                            inputmodel.SchoolId = info.AoMemberID;
                            inputmodel.FirstName = info.AoMemberFirstNameMod;
                            inputmodel.MiddleName = info.AoMemberMiddleNameMod;
                            inputmodel.LastName = info.AoMemberLastNameMod;
                            inputmodel.PreferredName = info.AoMemberPreferredNameMod;
                            inputmodel.PhoneNumber = info.AoMemberPhoneMod;
                            inputmodel.Email = info.AoMemberEmailMod;
                            break;
                        }
                    case "Counselor":
                        {
                            var info = context.CounselorModels.Where(a => a.CounselorId == user.SchoolId).First();
                            inputmodel.SchoolId = info.CounselorId;
                            inputmodel.FirstName = info.CounselorFirstName;
                            inputmodel.MiddleName = info.CounselorMiddleName;
                            inputmodel.LastName = info.CounselorLastName;
                            inputmodel.PreferredName = info.CounselorPreferredName;
                            inputmodel.PhoneNumber = info.CounselorPhone;
                            inputmodel.Email = info.CounselorEmail;
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }

            var model = new Tuple<ApplicationUser, InputModel>(user, inputmodel);
            ViewData["RoleId"] = new SelectList(context.Roles, "Name", "Name", user.Role);
            return View(model);
        }




        // POST: AccountManager/Edit/5
        [HttpPost]
        [RequireHttps]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, ApplicationUser appuser, InputModel input)
        {
            //var loggedinuserid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //if (loggedinuserid is null)
            //{
            //    return RedirectToAction("Error", "Error", new { number = 01, description = "Are you logged in?", reference = "Event ID = 1, Identifier = Acc72Det"});
            //}

            //var loggedinuser = await userManager.FindByIdAsync(loggedinuserid).ConfigureAwait(true);
            //if (loggedinuser is null)
            //{
            //    return RedirectToAction("Error", "Error", new { number = 01, description = "Are you an alien? We couldn't authenticate you.", reference = "Event ID = 1, Identifier = Acc78Det"});
            //}

            if (appuser is null)
            {
                return NotFound();
            }

            if (input is null)
            {
                return NotFound();
            }

            if (id != appuser.Id)
            {
                return NotFound();
            }


            if (ModelState.IsValid)
            {
                try
                {
                    var user = await userManager.FindByIdAsync(id).ConfigureAwait(true);
                    if (user == null)
                    {
                        return NotFound();
                    }

                    // Update the details
                    user.Email = input.Email;
                    user.ActivationCode = input.ActivationCode;
                    user.SchoolId = input.SchoolId;
                    user.Role = input.Role;

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
                    var addResult = await userManager.AddToRolesAsync(user, input.Role).ConfigureAwait(true);
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
                    if (!UserExists(appuser.Id))
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


            ViewData["RoleId"] = new SelectList(context.Roles, "Name", "Name", input.Role);
            return View(appuser);

        }

        private bool Switcher(InputModel input, ApplicationUser appuser)
        {
            bool worked = new();
            foreach (var role in input.Role)
            {
                switch (role)
                {
                    case "HS School Admin":
                        {
                            worked = AdminEditor(input, appuser, "High School");
                            if (worked == false)
                            {
                                return false;
                            }
                            break;
                        }

                    case "Counselor":
                        {
                            worked = CounselorEditor(input, appuser);
                            if (worked == false)
                            {
                                return false;
                            }
                            break;
                        }

                    case "Synnovation Lab Admin":
                        {
                            worked = AdminEditor(input, appuser, "Synnovation Lab");
                            if (worked == false)
                            {
                                return false;
                            }
                            break;
                        }

                    case "Attendance Office Member":
                        {
                            worked = AttOfficeMemberEditor(input, appuser);
                            if (worked == false)
                            {
                                return false;
                            }
                            break;
                        }

                    case "Nurse":
                        {
                            worked = NurseEditor(input, appuser);
                            if (worked == false)
                            {
                                return false;
                            }
                            break;
                        }

                    case "Law Enforcement":
                        {
                            worked = LawEnfEditor(input, appuser);
                            if (worked == false)
                            {
                                return false;
                            }
                            break;
                        }

                    case "Teacher":
                        {
                            worked = TeacherEditor(input, appuser);
                            if (worked == false)
                            {
                                return false;
                            }
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
                                return false;
                            }
                            if (input.StudentGradYearMod.HasValue)
                            {
                                worked = StudentEditor(input, appuser);
                                if (worked == false)
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                return false;
                            }
                            break;
                        }

                    case "District Admin":
                        {
                            worked = AdminEditor(input, appuser, "District");
                            if (worked == false)
                            {
                                return false;
                            }
                            break;
                        }
                    default:
                        {
                            worked = true;
                            break;
                        }
                }
            }
            return worked;
        }

        private bool TeacherEditor(InputModel input, ApplicationUser appuser)
        {
            var teacheruser = appuser;
            var teacher = context.TeacherInfoModels.Where(a => a.TeacherID.Equals(input.SchoolId, StringComparison.Ordinal)).First();

            if (teacheruser is null)
            {
                return false;
            }

            if (teacher is null)
            {
                return false;
            }

            //Edit all AppUser fields.
            teacheruser.Email = input.Email;
            teacheruser.ActivationCode = input.ActivationCode;


            //Edit all TeacherInfoModel fields.
            teacher.TeacherID = input.SchoolId;
            teacher.TeacherFirstNameMod = input.FirstName;
            teacher.TeacherMiddleNameMod = input.MiddleName;
            teacher.TeacherLastNameMod = input.LastName;
            teacher.TeacherPreferredNameMod = input.PreferredName;
            teacher.TeacherEmailMod = input.Email;
            teacher.TeacherPhoneMod = input.PhoneNumber;
            teacher.Teaches5Days = input.Teaches5Days;
            teacher.RoomAssignedId = input.RoomAssignedId;

            context.TeacherInfoModels.Update(teacher);
            return (context.SaveChangesAsync().IsCompletedSuccessfully);
        }

        private bool AdminEditor(InputModel input, ApplicationUser appuser, string label)
        {
            var adminuser = appuser;
            var admin = context.AdminInfoModels.Where(a => a.AdminID.Equals(input.SchoolId, StringComparison.Ordinal)).First();

            if (adminuser is null)
            {
                return false;
            }

            if (admin is null)
            {
                return false;
            }

            //Edit all AppUser fields.
            adminuser.Email = input.Email;
            adminuser.ActivationCode = input.ActivationCode;


            //Edit all TeacherInfoModel fields.
            admin.AdminID = input.SchoolId;
            admin.AdminFirstNameMod = input.FirstName;
            admin.AdminMiddleNameMod = input.MiddleName;
            admin.AdminLastNameMod = input.LastName;
            admin.AdminPreferredNameMod = input.PreferredName;
            admin.AdminEmailMod = input.Email;
            admin.AdminPhoneMod = input.PhoneNumber;
            admin.AdminLabelMod = label;

            context.AdminInfoModels.Update(admin);
            return (context.SaveChangesAsync().IsCompletedSuccessfully);
        }

        private bool AttOfficeMemberEditor(InputModel input, ApplicationUser appuser)
        {
            var attofficememuser = appuser;
            var attofficemem = context.AttendanceOfficeMemberModels.Where(a => a.AoMemberID.Equals(input.SchoolId, StringComparison.Ordinal)).First();

            if (attofficememuser is null)
            {
                return false;
            }

            if (attofficemem is null)
            {
                return false;
            }

            //Edit all AppUser fields.
            attofficememuser.Email = input.Email;
            attofficememuser.ActivationCode = input.ActivationCode;


            //Edit all TeacherInfoModel fields.
            attofficemem.AoMemberID = input.SchoolId;
            attofficemem.AoMemberFirstNameMod = input.FirstName;
            attofficemem.AoMemberMiddleNameMod = input.MiddleName;
            attofficemem.AoMemberLastNameMod = input.LastName;
            attofficemem.AoMemberPreferredNameMod = input.PreferredName;
            attofficemem.AoMemberEmailMod = input.Email;
            attofficemem.AoMemberPhoneMod = input.PhoneNumber;

            context.AttendanceOfficeMemberModels.Update(attofficemem);
            return (context.SaveChangesAsync().IsCompletedSuccessfully);
        }

        private bool NurseEditor(InputModel input, ApplicationUser appuser)
        {
            var nurseuser = appuser;
            var nurse = context.NurseInfoModels.Where(a => a.NurseID.Equals(input.SchoolId, StringComparison.Ordinal)).First();

            if (nurseuser is null)
            {
                return false;
            }

            if (nurse is null)
            {
                return false;
            }

            //Edit all AppUser fields.
            nurseuser.Email = input.Email;
            nurseuser.ActivationCode = input.ActivationCode;


            //Edit all TeacherInfoModel fields.
            nurse.NurseID = input.SchoolId;
            nurse.NurseFirstNameMod = input.FirstName;
            nurse.NurseMiddleNameMod = input.MiddleName;
            nurse.NurseLastNameMod = input.LastName;
            nurse.NursePreferredNameMod = input.PreferredName;
            nurse.NurseEmailMod = input.Email;
            nurse.NursePhoneMod = input.PhoneNumber;

            context.NurseInfoModels.Update(nurse);
            return (context.SaveChangesAsync().IsCompletedSuccessfully);
        }

        private bool LawEnfEditor(InputModel input, ApplicationUser appuser)
        {
            var lawenfuser = appuser;
            var lawenf = context.LawEnforcementInfoModels.Where(a => a.LawenfID.Equals(input.SchoolId, StringComparison.Ordinal)).First();

            if (lawenfuser is null)
            {
                return false;
            }

            if (lawenf is null)
            {
                return false;
            }

            //Edit all AppUser fields.
            lawenfuser.Email = input.Email;
            lawenfuser.ActivationCode = input.ActivationCode;


            //Edit all TeacherInfoModel fields.
            lawenf.LawenfID = input.SchoolId;
            lawenf.LaweFirstNameMod = input.FirstName;
            lawenf.LaweMiddleNameMod = input.MiddleName;
            lawenf.LaweLastNameMod = input.LastName;
            lawenf.LawePreferredNameMod = input.PreferredName;
            lawenf.LaweEmailMod = input.Email;
            lawenf.LawePhoneMod = input.PhoneNumber;

            context.LawEnforcementInfoModels.Update(lawenf);
            return (context.SaveChangesAsync().IsCompletedSuccessfully);
        }

        private bool StudentEditor(InputModel input, ApplicationUser appuser)
        {
            var studentuser = appuser;
            if (int.TryParse(input.SchoolId, out int studentid))
            {
                var student = context.StudentInfoModels.Where(a => a.StudentID == studentid).First();

                if (studentuser is null)
                {
                    return false;
                }

                if (student is null)
                {
                    return false;
                }

                //Edit all AppUser fields.
                studentuser.Email = input.Email;
                studentuser.ActivationCode = input.ActivationCode;


                //Edit all TeacherInfoModel fields.
                student.StudentID = studentid;
                student.StudentFirstNameMod = input.FirstName;
                student.StudentMiddleNameMod = input.MiddleName;
                student.StudentLastNameMod = input.LastName;
                student.StudentPreferredNameMod = input.PreferredName;
                student.StudentEmailMod = input.Email;
                student.StudentPhoneMod = input.PhoneNumber;
                student.Parentguard1NameMod = input.Parentguard1NameMod!;
                student.Parentguard2NameMod = input.Parentguard2NameMod!;
                student.Parentguard1EmailMod = input.Parentguard1EmailMod!;
                student.Parentguard2EmailMod = input.Parentguard2EmailMod!;
                student.StudentGradYearMod = input.StudentGradYearMod!.Value;
                student.StudentCounselorID = input.StudentCounselorID!;
                student.HasEASupport = input.HasEASupport;
                student.StudentEAID = input.StudentEAID;

                context.StudentInfoModels.Update(student);
                return (context.SaveChangesAsync().IsCompletedSuccessfully);
            }
            return false;
        }

        private bool CounselorEditor(InputModel input, ApplicationUser appuser)
        {
            var counseloruser = appuser;
            var counselor = context.CounselorModels.Where(a => a.CounselorId.Equals(input.SchoolId, StringComparison.Ordinal)).First();

            if (counseloruser is null)
            {
                return false;
            }

            if (counselor is null)
            {
                return false;
            }

            //Edit all AppUser fields.
            counseloruser.Email = input.Email;
            counseloruser.ActivationCode = input.ActivationCode;


            //Edit all TeacherInfoModel fields.
            counselor.CounselorId = input.SchoolId;
            counselor.CounselorFirstName = input.FirstName;
            counselor.CounselorMiddleName = input.MiddleName;
            counselor.CounselorLastName = input.LastName;
            counselor.CounselorPreferredName = input.PreferredName;
            counselor.CounselorEmail = input.Email;
            counselor.CounselorPhone = input.PhoneNumber;

            context.CounselorModels.Update(counselor);
            return (context.SaveChangesAsync().IsCompletedSuccessfully);
        }

        private bool UserExists(string schoolId)
        {
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
                    case "Counselor":
                        {
                            ViewData["Counselor Info"] = "Present";
                            break;
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

                                    var sem1sched = context.Sem1StudSchedules.Where(a => a.StudentID == studId).First();
                                    if (sem1sched is null)
                                    {
                                        _logger.LogInformation("Unable to retrieve student schedule 1 for deletion.");
                                    }
                                    else
                                    {
                                        var sem1schedDeletion = context.Sem1StudSchedules.Remove(sem1sched);
                                        await context.SaveChangesAsync().ConfigureAwait(true);
                                    }

                                    var sem2sched = context.Sem2StudSchedules.Where(a => a.StudentID == studId).First();
                                    if (sem2sched is null)
                                    {
                                        _logger.LogInformation("Unable to retrieve student schedule 2 for deletion.");
                                    }
                                    else
                                    {
                                        var sem2schedDeletion = context.Sem2StudSchedules.Remove(sem2sched);
                                        await context.SaveChangesAsync().ConfigureAwait(true);
                                    }

                                    var studentInfo = await context.StudentInfoModels.FindAsync(studId).ConfigureAwait(true);
                                    if (studentInfo is null)
                                    {
                                        _logger.LogInformation("Unable to retrieve student data for deletion.");
                                    }
                                    else
                                    {
                                        var studInfoDeletion = context.StudentInfoModels.Remove(studentInfo);
                                        await context.SaveChangesAsync().ConfigureAwait(true);
                                    }
                                    break;
                                }
                            case "Teacher":
                                {
                                    var teacherid = schoolId;
                                    List<ActiveCourseInfoModel> courses = [.. context.ActiveCourseInfoModels.Where(a => a.CourseTeacherID == teacherid)];
                                    List<TeacherInfoModel> teacherInfo = [.. context.TeacherInfoModels.Where(a => a.TeacherID == teacherid)];

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
                                    var attinfo = context.AttendanceOfficeMemberModels.Where(a => a.AoMemberID == attid).ToList();
                                    if (attinfo.Count <= 0)
                                    {
                                        _logger.LogInformation("Unable to retrieve attendance office member data for deletion.");
                                    }
                                    else
                                    {
                                        var deletion = context.Remove(attinfo);
                                        await context.SaveChangesAsync().ConfigureAwait(true);
                                    }
                                    break;
                                }
                            case "HS School Admin":
                                {
                                    var adminid = schoolId;
                                    var admininfo = context.AdminInfoModels.Where(a => a.AdminID == adminid).ToList();
                                    if (admininfo.Count <= 0)
                                    {
                                        _logger.LogInformation("Unable to retrieve admin data for deletion.");
                                    }
                                    else
                                    {
                                        var deletion = context.Remove(admininfo);
                                        await context.SaveChangesAsync().ConfigureAwait(true);
                                    }
                                    break;
                                }
                            case "Synnovation Lab Admin":
                                {
                                    var adminid = schoolId;
                                    var admininfo = context.AdminInfoModels.Where(a => a.AdminID == adminid).ToList();

                                    if (admininfo.Count == 0)
                                    {
                                        LoggerMessage.Define(logLevel: LogLevel.Critical, eventId: new EventId(700, "Data retrieval failed."), $"Something went wrong. Following are the details. adminInfo is empty.");
                                    }
                                    else
                                    {
                                        var deletion = context.Remove(admininfo);
                                        await context.SaveChangesAsync().ConfigureAwait(true);
                                    }

                                    break;
                                }
                            case "Nurse":
                                {
                                    var nurseid = schoolId;
                                    var nurseinfo = context.NurseInfoModels.Where(a => a.NurseID == nurseid).ToList();
                                    if (nurseinfo.Count <= 0)
                                    {
                                        _logger.LogInformation("Unable to retrieve nurse data for deletion.");
                                    }
                                    else
                                    {
                                        var deletion = context.Remove(nurseinfo);
                                        await context.SaveChangesAsync().ConfigureAwait(true);
                                    }
                                    break;
                                }
                            case "Law Enforcement":
                                {
                                    var lawenfid = schoolId;
                                    var lawenfinfo = context.LawEnforcementInfoModels.Where(a => a.LawenfID == lawenfid).ToList();
                                    if (lawenfinfo.Count <= 0)
                                    {
                                        _logger.LogInformation("Unable to retrieve law enforcement officer data for deletion.");
                                    }

                                    var deletion = context.Remove(lawenfinfo);
                                    await context.SaveChangesAsync().ConfigureAwait(true);
                                    break;
                                }
                            /*case "Synnovation Lab QR Code Scanner Management":
                                {
                                    var adminid = schoolId;
                                    var admininfo = context.HandheldScannerNodeModels.Where(a => a.ScannerID == adminid).ToList() ?? throw new Exception("Unable to retrieve scanner node data for deletion.");

                                    var deletion = context.Remove(admininfo);
                                    await context.SaveChangesAsync().ConfigureAwait(true);
                                    break;
                                }*/
                            case "Substitute Teacher":
                                {
                                    var teacherid = schoolId;
                                    var courses = context.ActiveCourseInfoModels.Where(a => a.CourseTeacherID == teacherid).ToList();
                                    if (courses.Count <= 0)
                                    {
                                        _logger.LogInformation("Unable to retrieve teacher's courses for deletion.");
                                    }
                                    else
                                    {
                                        var deletion1 = context.Remove(courses);
                                        await context.SaveChangesAsync().ConfigureAwait(true);
                                    }
                                    var teacherInfo = context.TeacherInfoModels.Where(a => a.TeacherID == teacherid).ToList();
                                    if (teacherInfo.Count <= 0)
                                    {
                                        _logger.LogInformation("Unable to retrieve teacher data for deletion.");
                                    }
                                    else
                                    {
                                        var deletion2 = context.Remove(teacherInfo);
                                        await context.SaveChangesAsync().ConfigureAwait(true);
                                    }
                                    break;
                                }
                            case "District Admin":
                                {
                                    var adminid = schoolId;
                                    var admininfo = context.AdminInfoModels.Where(a => a.AdminID == adminid).ToList();
                                    if (admininfo.Count <= 0)
                                    {
                                        _logger.LogInformation("Unable to retrieve admin data for deletion.");
                                    }
                                    else
                                    {
                                        var deletion = context.Remove(admininfo);
                                        await context.SaveChangesAsync().ConfigureAwait(true);
                                    }
                                    break;
                                }
                            case "Counselor":
                                {
                                    var cslrid = schoolId;
                                    var cslrinfo = context.CounselorModels.Where(a => a.CounselorId == cslrid).ToList();
                                    if (cslrinfo.Count <= 0)
                                    {
                                        _logger.LogInformation("Unable to retrieve counselor data for deletion.");
                                    }
                                    else
                                    {
                                        var deletion = context.Remove(cslrinfo);
                                        await context.SaveChangesAsync().ConfigureAwait(true);
                                    }
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
        public required string FirstName { get; set; }


        [Display(Name = "Middle Name (Optional)")]
        public string? MiddleName { get; set; }


        [Display(Name = "Last Name")]
        public required string LastName { get; set; }


        [Display(Name = "Preferred Name (Optional)")]
        public string? PreferredName { get; set; }


        [Display(Name = "Phone Number/School Extension (Optional)")]
        public string? PhoneNumber { get; set; }
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
        public bool? HasEASupport { get; set; }


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
        public bool? Teaches5Days { get; set; }

        [Display(Name = "Assigned Room")]
        public int? RoomAssignedId { get; set; }
        //ALL TEACHER REQUIRED INFORMATION ENDS HERE
    }
}
