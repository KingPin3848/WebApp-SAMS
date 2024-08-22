using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SAMS.Controllers;
using SAMS.Data;
using SAMS.Interfaces;
using SAMS.Models;

namespace SAMS.Areas.Student.Controllers
{
    [Area("Student")]
    public class ScanController(ILogger<ScanController> logger, ApplicationDbContext context, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager) : Controller
    {
        private readonly ILogger<ScanController> _logger = logger;
        private readonly ApplicationDbContext _context = context;
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
        private readonly UserManager<ApplicationUser> _userManager = userManager;

        [HttpGet]
        //[Authorize(Roles = "Student, Developer")]
        public async Task<IActionResult> Scan()
        {
            var user = await _userManager.GetUserAsync(User).ConfigureAwait(true);
            ViewBag.Schoolid = user?.SchoolId;
            return View();
        }

        //[Authorize(Roles = "Student, Developer")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Scan(string ScannedCode, string issuedSchoolId)
        {
            try
            {
                var time = DateTime.Now.TimeOfDay;
                var user = await _userManager.GetUserAsync(User).ConfigureAwait(true);

                if (user == null)
                {
                    return Json(new { dangertext = "User not found for the provided school ID." });
                }

                var chosenBellSchedule = _context.ChosenBellSchedModels.Select(a => a.Name).First();
                if (chosenBellSchedule is null || string.IsNullOrEmpty(chosenBellSchedule))
                {
                    return Json(new { dangertext = "Couldn't find today's bell schedule." });
                }

                var determination = DetermineCurrentBell(chosenBellSchedule);
                var currentBell = determination[0];
                TimeSpan startTimeAsDetermined = TimeSpan.Parse(determination[1]);
                TimeSpan endTimeAsDetermined = TimeSpan.Parse(determination[2]);
                if (currentBell == "School not in session!" || currentBell == "Bell 0")
                {
                    return Json(new { dangertext = "School is not in session and you cannot sign in right now. If you think this is a mistake, please contact the admin and the developers ASAP." });
                }

                var schoolIDdb = user.SchoolId;
                if (schoolIDdb == null)
                {
                    return Json(new { dangertext = $"Unable to find the school id with the user with ID." });
                    //return NotFound($"Unable to find the school id with the user with ID '{_userManager.GetUserId(User)}'.");
                }

                var passedID = issuedSchoolId;
                if (schoolIDdb == passedID)
                {
                    if (int.TryParse(schoolIDdb, out int studId))
                    {
                        //Do nothing
                    }
                    else
                    {
                        return Json(new { dangertext = "Your StudentId could not be retrived because it is not a number. Please contact an administrator ASAP and notify them of this error." });
                    }
                    var sem2start = _context.SchedulerModels.Where(a => a.Type == SchedulerModel.Types.Semester2).Select(a => a.Date).FirstOrDefault();
                    IStudentSchedule? studentSchedule = (DateOnly.FromDateTime(DateTime.Now.Date) >= sem2start) ? (await _context.Sem2StudSchedules.FindAsync(studId)) : (await _context.Sem1StudSchedules.FindAsync(studId));
                    if (studentSchedule == null)
                    {
                        return Json(new { dangertext = "Your Schedule could not be retrived because it is empty. Please contact your counselor ASAP and notify them of this error." });
                    }

                    var courseIdForCurrentBell = GetCourseIdForCurrentBell(currentBell, studentSchedule);
                    if (courseIdForCurrentBell == 0)
                    {
                        return Json(new { dangertext = "Invalid Course Id. This is an error on our end. Please contact the developers and share the whole experience step-by-step on what happened exactly." });
                    }

                    var roomIdForCourse = _context.ActiveCourseInfoModels.Where(a => a.CourseId == courseIdForCurrentBell).Select(a => a.CourseRoomID).First();
                    if (roomIdForCourse == 0)
                    {
                        return Json(new { dangertext = "Room ID not found for the course. Please check with the admins to add the respective roomid in SAMS." });
                    }

                    var expectedQRCode = _context.RoomQRCodeModels.Where(a => a.RoomId == roomIdForCourse).Select(a => a.Code).ToList().First();
                    if (expectedQRCode == null)
                    {
                        return Json(new { dangertext = "Expected QR Code not found for the room you are in at right now. Please contact the admin to add the room in SAMS." });
                    }

                    var passedCode = ScannedCode;
                    if (passedCode == expectedQRCode)
                    {
                        var dailyEntryExists = _context.DailyAttendanceModels.Any(a =>
                            a.StudentId == studId &&
                            a.AttendanceDate == DateOnly.FromDateTime(DateTime.Now.Date) &&
                            a.Status == "Unknown");
                        if (dailyEntryExists)
                        {
                            var dailyAttendanceEntry = _context.DailyAttendanceModels.First(a =>
                                a.StudentId == studId &&
                                a.AttendanceDate == DateOnly.FromDateTime(DateTime.Now.Date) &&
                                a.Status == "Unknown");
                            var timeStamp = new TimestampModel();

                            if (time >= startTimeAsDetermined.Add(TimeSpan.FromMinutes(5)) && time <= endTimeAsDetermined)
                            {
                                dailyAttendanceEntry.Status = "Tardy";
                                timeStamp.Timestamp = DateTime.Now;
                                timeStamp.ActionMade = $"Student Marked Tardy Automatically at {DateTime.Now} for Daily Attendance";
                                timeStamp.MadeBy = $"SAMS Program Scan - {DateTime.Now} - Automatic";
                                timeStamp.Comments = $"Student was marked tardy because the student scanned in 5 minutes after the start of the in-school bell, and before the end of the bell - {DateTime.Now}. Hall-pass feature not available yet, and hall-pass was not checked. Please contact the teacher/admin/attendance office for any questions or concerns regarding this.";
                            }
                            else if (time <= startTimeAsDetermined.Add(TimeSpan.FromMinutes(5)) && time >= startTimeAsDetermined)
                            {
                                dailyAttendanceEntry.Status = "Present";
                                timeStamp.Timestamp = DateTime.Now;
                                timeStamp.ActionMade = $"Student Marked Present Automatically at {DateTime.Now} for Daily Attendance";
                                timeStamp.MadeBy = $"SAMS Program - {DateTime.Now} - Automatic";
                                timeStamp.Comments = $"Student was marked present because the student scanned in within 5 minutes of the start of the in-school bell. {DateTime.Now} Please contact the teacher/admin/attendance office for any questions or concerns regarding this.";
                            }

                            _context.DailyAttendanceModels.Update(dailyAttendanceEntry);
                            _context.TimestampModels.Add(timeStamp);
                            await _context.SaveChangesAsync();
                        }

                        //This is where we would add the algorithm to check for both the field trip
                        //And the hall-pass still using the If statements.
                        //And the same algorithm would go to the form feature as well, but excluding the daily attendance option.

                        var bellEntryExists = _context.BellAttendanceModels.Any(a =>
                            a.StudentId == studId &&
                            a.DateTime.Date == DateTime.Now.Date &&
                            a.BellNumId == currentBell &&
                            a.Status == "Unknown");
                        if (bellEntryExists)
                        {
                            var bellAttendanceEntry = _context.BellAttendanceModels.First(a =>
                                a.StudentId == studId &&
                                a.DateTime.Date == DateTime.Now.Date &&
                                a.BellNumId == currentBell &&
                                a.Status != "Present");
                            var timeStamp = new TimestampModel();

                            if (time >= startTimeAsDetermined.Add(TimeSpan.FromMinutes(5)) && time <= endTimeAsDetermined)
                            {
                                bellAttendanceEntry.Status = "Tardy";
                                timeStamp.Timestamp = DateTime.Now;
                                timeStamp.ActionMade = $"Student Marked Tardy Automatically at {DateTime.Now} for Bell Attendance";
                                timeStamp.MadeBy = $"SAMS Program - {DateTime.Now} - Automatic Scan Update";
                                timeStamp.Comments = "Student was marked tardy because the student scanned in 5 minutes after the start of the bell, and before the end of the bell. Hall-pass feature not available yet, and hall-pass was not checked. Please contact the teacher/admin/attendance office for any questions or concerns regarding this.";
                            }
                            else if (time <= startTimeAsDetermined.Add(TimeSpan.FromMinutes(5)) && time >= startTimeAsDetermined.Subtract(TimeSpan.FromMinutes(5)))
                            {
                                bellAttendanceEntry.Status = "Present";
                                timeStamp.Timestamp = DateTime.Now;
                                timeStamp.ActionMade = $"Student Marked Present Automatically at {DateTime.Now} for Bell Attendance";
                                timeStamp.MadeBy = $"SAMS Program - {DateTime.Now} - Automatic Scan Update";
                                timeStamp.Comments = "Student was marked present because the student scanned in within 5 minutes of the start of the bell. Please contact the teacher/admin/attendance office for any questions or concerns regarding this.";
                            }

                            _context.BellAttendanceModels.Update(bellAttendanceEntry);
                            _context.TimestampModels.Add(timeStamp);
                            await _context.SaveChangesAsync();
                        }
                        else
                        {
                            if (time >= startTimeAsDetermined.Add(TimeSpan.FromMinutes(5)) && time <= endTimeAsDetermined)
                            {
                                var bellattendanceentry = new BellAttendanceModel()
                                {
                                    StudentId = studId,
                                    DateTime = DateTime.Now,
                                    Status = "Tardy",
                                    ReasonForAbsence = "",
                                    BellNumId = currentBell,
                                    CourseId = courseIdForCurrentBell,
                                    ChosenBellSchedule = chosenBellSchedule
                                };
                                var timestamp = new TimestampModel()
                                {
                                    Timestamp = DateTime.Now,
                                    ActionMade = $"Student added and marked Tardy automatically at {DateTime.Now} for Bell Attendance",
                                    MadeBy = $"SAMS Program - {DateTime.Now} - Automatic Scan Add and Update",
                                    Comments = $"Student was marked tardy because the student scanned in 5 minutes after the start of the bell. Please contact the teacher/admin/attendance office for any questions or concerns regarding this."
                                };
                                _context.BellAttendanceModels.Add(bellattendanceentry);
                                _context.TimestampModels.Add(timestamp);
                                await _context.SaveChangesAsync().ConfigureAwait(true);
                            }
                        }

                        var studLocationEntry = _context.StudentLocationModels.Any(a => a.StudentIdMod == studId);
                        if (studLocationEntry)
                        {
                            var studLocation = _context.StudentLocationModels.FirstOrDefault(a => a.StudentIdMod == studId);
                            if (studLocation != null)
                            {
                                var room = await _context.RoomLocationInfoModels.FindAsync(roomIdForCourse);
                                studLocation.StudentLocation = $"{room?.RoomNumberMod} - {room?.Teacher?.TeacherFirstNameMod} {room?.Teacher?.TeacherLastNameMod}";
                                var timestamp = new TimestampModel
                                {
                                    Timestamp = DateTime.Now,
                                    ActionMade = "Student Location Update",
                                    MadeBy = "Scan Feature of SAMS",
                                    Comments = $"The student scanned to update their location to {room?.RoomNumberMod} - {room?.Teacher?.TeacherFirstNameMod} {room?.Teacher?.TeacherLastNameMod}"
                                };
                                _context.TimestampModels.Add(timestamp);
                                _context.StudentLocationModels.Update(studLocation);
                                await _context.SaveChangesAsync();
                            }
                            else
                            {
                                var room = await _context.RoomLocationInfoModels.FindAsync(roomIdForCourse);
                                var location = new StudentLocationModel
                                {
                                    StudentIdMod = studId,
                                    StudentName = $"{_context.StudentInfoModels.Where(a => a.StudentID == studId).Select(a => a.StudentFirstNameMod)} {_context.StudentInfoModels.Where(a => a.StudentID == studId).Select(a => a.StudentMiddleNameMod)} {_context.StudentInfoModels.Where(a => a.StudentID == studId).Select(a => a.StudentLastNameMod)}",
                                    StudentLocation = $"Unknown Tag!  -  {room?.RoomNumberMod} - {room?.Teacher?.TeacherFirstNameMod} {room?.Teacher?.TeacherLastNameMod}"
                                };
                            }
                        }

                        return Json(new { redirectUrl = Url.Action("Index", "Home") });
                    }
                    return Json(new { dangertext = $"You have to be in room {roomIdForCourse}, not room {_context.RoomQRCodeModels.Where(a => a.Code == passedCode).First().RoomId}" });
                }
                return Json(new { dangertext = "You are a hacker!" });
            }
            catch (Exception ex)
            {
                return Json(new { dangertext = $"An error occurred: {ex.Message}" });
            }
        }

        private static int GetCourseIdForCurrentBell(string currentBell, IStudentSchedule studentSchedule)
        {
            switch (currentBell)
            {
                case "Bell 1":
                    return studentSchedule.Bell1CourseIDMod;
                case "Bell 2":
                    if (DateTime.Now.Date.DayOfWeek == DayOfWeek.Monday || DateTime.Now.Date.DayOfWeek == DayOfWeek.Wednesday)
                    {
                        return studentSchedule.Bell2MonWedCourseIDMod;
                    }
                    return studentSchedule.Bell2TueThurCourseIDMod;
                case "Bell 3":
                    if (DateTime.Now.Date.DayOfWeek == DayOfWeek.Monday || DateTime.Now.Date.DayOfWeek == DayOfWeek.Wednesday)
                    {
                        return studentSchedule.Bell3MonWedCourseIDMod;
                    }
                    return studentSchedule.Bell3TueThurCourseIDMod;
                case "Bell 4":
                    if (DateTime.Now.Date.DayOfWeek == DayOfWeek.Monday || DateTime.Now.Date.DayOfWeek == DayOfWeek.Wednesday)
                    {
                        return studentSchedule.Bell4MonWedCourseIDMod;
                    }
                    return studentSchedule.Bell4TueThurCourseIDMod;
                case "Bell 5":
                    if (DateTime.Now.Date.DayOfWeek == DayOfWeek.Monday || DateTime.Now.Date.DayOfWeek == DayOfWeek.Wednesday)
                    {
                        return studentSchedule.Bell5MonWedCourseIDMod;
                    }
                    return studentSchedule.Bell5TueThurCourseIDMod;
                case "Bell 6":
                    if (DateTime.Now.Date.DayOfWeek == DayOfWeek.Monday || DateTime.Now.Date.DayOfWeek == DayOfWeek.Wednesday)
                    {
                        return studentSchedule.Bell6MonWedCourseIDMod;
                    }
                    return studentSchedule.Bell6TueThurCourseIDMod;
                case "Bell 7":
                    if (DateTime.Now.Date.DayOfWeek == DayOfWeek.Monday || DateTime.Now.Date.DayOfWeek == DayOfWeek.Wednesday)
                    {
                        return studentSchedule.Bell7MonWedCourseIDMod;
                    }
                    return studentSchedule.Bell7TueThurCourseIDMod;
                default:
                    return 0;
            }
        }

        private List<string> DetermineCurrentBell(string bellschedname)
        {
            var dateTime = DateTime.Now;
            var time = dateTime.TimeOfDay;
            string bellName = null!;
            var startTimeDetermined = "";
            var endTimeDetermined = "";
            List<string> returnStuff = [bellName, startTimeDetermined, endTimeDetermined];

            switch (bellschedname)
            {
                case "Daily Bell Schedule":
                    {
                        List<IBellSchedule> dailySchedule = [.. _context.DailyBellScheduleModels.OrderBy(a => a.StartTime).Cast<IBellSchedule>()];
                        foreach (var entry in dailySchedule)
                        {
                            IBellSchedule bell = entry;
                            if (time >= bell.StartTime && time <= bell.EndTime)
                            {
                                startTimeDetermined = bell.StartTime.ToString();
                                endTimeDetermined = bell.EndTime.ToString();
                                bellName = bell.BellName;
                                returnStuff[0] = bellName;
                                returnStuff[1] = startTimeDetermined;
                                returnStuff[2] = endTimeDetermined;
                            }
                        }
                        return returnStuff;
                    }
                case "Extended Aves Bell Schedule":
                    {
                        List<IBellSchedule> extSchedule = [.. _context.ExtendedAvesModels.OrderBy(a => a.StartTime).Cast<IBellSchedule>()];
                        foreach (var entry in extSchedule)
                        {
                            IBellSchedule bell = entry;
                            if (time >= bell.StartTime && time <= bell.EndTime)
                            {
                                startTimeDetermined = bell.StartTime.ToString();
                                endTimeDetermined = bell.EndTime.ToString();
                                bellName = bell.BellName;
                                returnStuff[0] = bellName;
                                returnStuff[1] = startTimeDetermined;
                                returnStuff[2] = endTimeDetermined;
                            }
                        }
                        return returnStuff;
                    }
                case "Pep Rally Bell Schedule":
                    {
                        List<IBellSchedule> pepSchedule = [.. _context.PepRallyBellScheduleModels.OrderBy(a => a.StartTime).Cast<IBellSchedule>()];
                        foreach (var entry in pepSchedule)
                        {
                            IBellSchedule bell = entry;
                            if (time >= bell.StartTime && time <= bell.EndTime)
                            {
                                startTimeDetermined = bell.StartTime.ToString();
                                endTimeDetermined = bell.EndTime.ToString();
                                bellName = bell.BellName;
                                returnStuff[0] = bellName;
                                returnStuff[1] = startTimeDetermined;
                                returnStuff[2] = endTimeDetermined;
                            }
                        }
                        return returnStuff;
                    }
                case "2 Hour Delay Bell Schedule":
                    {
                        List<IBellSchedule> twoSchedule = [.. _context.TwoHrDelayBellScheduleModels.OrderBy(a => a.StartTime).Cast<IBellSchedule>()];
                        foreach (var entry in twoSchedule)
                        {
                            IBellSchedule bell = entry;
                            if (time >= bell.StartTime && time <= bell.EndTime)
                            {
                                startTimeDetermined = bell.StartTime.ToString();
                                endTimeDetermined = bell.EndTime.ToString();
                                bellName = bell.BellName;
                                returnStuff[0] = bellName;
                                returnStuff[1] = startTimeDetermined;
                                returnStuff[2] = endTimeDetermined;
                            }
                        }
                        return returnStuff;
                    }
                case "Custom Bell Schedule":
                    {
                        List<IBellSchedule> custSchedule = [.. _context.CustomSchedules.OrderBy(a => a.StartTime).Where(a => a.BellName.Contains("Bell ")).Cast<IBellSchedule>()];
                        foreach (var entry in custSchedule)
                        {
                            IBellSchedule bell = entry;
                            if (time >= bell.StartTime && time <= bell.EndTime)
                            {
                                startTimeDetermined = bell.StartTime.ToString();
                                endTimeDetermined = bell.EndTime.ToString();
                                bellName = bell.BellName;
                                returnStuff[0] = bellName;
                                returnStuff[1] = startTimeDetermined;
                                returnStuff[2] = endTimeDetermined;
                            }
                        }
                        return returnStuff;
                    }
                default:
                    {
                        bellName = "School not in session!";
                        returnStuff[0] = bellName;
                        startTimeDetermined = string.Empty;
                        returnStuff[1] = startTimeDetermined;
                        endTimeDetermined = string.Empty;
                        returnStuff[2] = endTimeDetermined;
                        return (returnStuff);
                    }
            }
        }
    }
}
