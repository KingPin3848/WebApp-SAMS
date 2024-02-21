using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SAMS.Controllers;
using SAMS.Data;
using SAMS.Interfaces;
using SAMS.Models;
using System.Security.Policy;

namespace SAMS.Areas.Student.Controllers
{
    [Area("Student")]
    public class ScanController : Controller
    {
        private readonly ILogger<ScanController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public ScanController(ILogger<ScanController> logger, ApplicationDbContext context, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet]
        [Authorize(Roles = "Student, Developer")]
        public async Task<IActionResult> Scan()
        {
            var user = await _userManager.GetUserAsync(User);
            ViewBag.Schoolid = user?.SchoolId;
            return View();
        }

        [Authorize(Roles = "Student, Developer")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Scan(string ScannedCode, string issuedSchoolId)
        {
            try
            {
                var time = DateTime.Now.TimeOfDay;
                var user = await _userManager.GetUserAsync(User);

                if (user == null)
                {
                    return Json(new { dangertext = "User not found for the provided school ID." });
                }

                var chosenBellSchedule = _context.chosenBellSchedModels.Select(a => a.Name).ToList();
                if (chosenBellSchedule == null || chosenBellSchedule.Count == 0)
                {
                    return Json(new { dangertext = "Chosen Bell Schedule for the day not found. Please contact the administrator and developer for additional assistance." });
                }

                var determination = DetermineCurrentBell(chosenBellSchedule[0]);
                var currentBell = determination[0];
                TimeSpan startTimeAsDetermined = TimeSpan.Parse(determination[1]);
                TimeSpan endTimeAsDetermined = TimeSpan.Parse(determination[2]);
                if (currentBell == "School not in session!")
                {
                    return Json(new { dangertext = "School is not in session and you cannot sign in right now. If you think this is a mistake, please contact the admin and the developers ASAP." });
                }

                var schoolIDdb = user.SchoolId;
                if (schoolIDdb == null)
                {
                    return Json(new { dangertext = $"Unable to find the school id with the user with ID '{_userManager.GetUserId(User)}'." });
                    //return NotFound($"Unable to find the school id with the user with ID '{_userManager.GetUserId(User)}'.");
                }
                
                var passedID = issuedSchoolId;
                if (schoolIDdb == passedID)
                {
                    var studId = int.Parse(schoolIDdb);
                    var studentSchedule = await _context.sem1StudSchedules.FindAsync(studId);
                    //var sem2start = _context.schedulerModels.Where(a => a.Type == "Semester 2").Select(a => a.Date).FirstOrDefault();
                    //if (DateOnly.FromDateTime(DateTime.Now.Date) >= sem2start)
                    //{
                    //    //studentSchedule = await _context.sem2StudSchedules.FindAsync(studId);
                    //} else
                    //{
                    //    studentSchedule = await _context.sem1StudSchedules.FindAsync(studId);
                    //}
                    if (studentSchedule == null)
                    {
                        return Json(new { dangertext = "Your Schedule could not be retrived because it is completely empty. Please contact your counselor ASAP and notify them of this error." });
                    }

                    var courseIdForCurrentBell = GetCourseIdForCurrentBell(currentBell, studentSchedule);
                    if (courseIdForCurrentBell == 0)
                    {
                        return Json(new { dangertext = "Invalid Course Id. This is an error on our end. Please contact the developers and share the whole experience step-by-step on what happened exactly." });
                    }

                    var roomIdForCourse = _context.activeCourseInfoModels.Where(a => a.CourseId == courseIdForCurrentBell).Select(a => a.CourseRoomID).ToList().FirstOrDefault();
                    if (roomIdForCourse == 0)
                    {
                        return Json(new { dangertext = "Room ID not found for the course. Please check with the admins to add the respective roomid in SAMS." });
                    }

                    var expectedQRCode = _context.roomQRCodeModels.Where(a => a.RoomId == roomIdForCourse).Select(a => a.Code).ToList().FirstOrDefault();
                    if (expectedQRCode == null)
                    {
                        return Json(new { dangertext = "Expected QR Code not found for the room you are in at right now. Please contact the admin to add the room in SAMS." });
                    }

                    var passedCode = ScannedCode;
                    if (passedCode == expectedQRCode)
                    {
                        var dailyEntryExists = _context.dailyAttendanceModels.Any(a =>
                            a.StudentId == studId &&
                            a.AttendanceDate == DateTime.Now.Date &&
                            a.Status == "Unknown");
                        if (dailyEntryExists)
                        {
                            var dailyAttendanceEntry = _context.dailyAttendanceModels.First(a =>
                                a.StudentId == studId &&
                                a.AttendanceDate == DateTime.Now.Date &&
                                a.Status == "Unknown");
                            var timeStamp = new TimestampModel();

                            if (time >= startTimeAsDetermined.Add(TimeSpan.FromMinutes(25)) && time <= endTimeAsDetermined)
                            {
                                dailyAttendanceEntry.Status = "Tardy";
                                timeStamp.Timestamp = DateTime.Now;
                                timeStamp.ActionMade = $"Student Marked Tardy Automatically at {DateTime.Now} for Daily Attendance";
                                timeStamp.MadeBy = $"SAMS Program - {DateTime.Now} - Automatic";
                                timeStamp.Comments = "Student was marked tardy because the student scanned in 25 minutes after the start of the in-school bell, and before the end of the bell. Hall-pass feature not available yet, and hall-pass was not checked. Please contact the teacher/admin/attendance office for any questions or concerns regarding this.";
                            }
                            else if (time <= startTimeAsDetermined.Add(TimeSpan.FromMinutes(25)) && time >= startTimeAsDetermined)
                            {
                                dailyAttendanceEntry.Status = "Present";
                                timeStamp.Timestamp = DateTime.Now;
                                timeStamp.ActionMade = $"Student Marked Present Automatically at {DateTime.Now} for Daily Attendance";
                                timeStamp.MadeBy = $"SAMS Program - {DateTime.Now} - Automatic";
                                timeStamp.Comments = "Student was marked present because the student scanned in within 25 minutes of the start of the in-school bell. Please contact the teacher/admin/attendance office for any questions or concerns regarding this.";
                            }

                            _context.dailyAttendanceModels.Update(dailyAttendanceEntry);
                            _context.timestampModels.Add(timeStamp);
                        }

                        //This is where we would add the algorithm to check for both the field trip
                        //And the hall-pass still using the If statements.
                        //And the same algorithm would go to the form feature as well, but excluding the daily attendance option.

                        var bellEntryExists = _context.bellAttendanceModels.Any(a =>
                            a.StudentId == studId &&
                            a.DateTime.Date == DateTime.Now.Date &&
                            a.BellNumId == currentBell &&
                            a.Status == "Unknown");
                        if (bellEntryExists)
                        {
                            var bellAttendanceEntry = _context.bellAttendanceModels.First(a =>
                                a.StudentId == studId &&
                                a.DateTime.Date == DateTime.Now.Date &&
                                a.BellNumId == currentBell &&
                                a.Status == "Unknown");
                            var timeStamp = new TimestampModel();

                            if (time >= startTimeAsDetermined.Add(TimeSpan.FromMinutes(25)) && time <= endTimeAsDetermined)
                            {
                                bellAttendanceEntry.Status = "Tardy";
                                timeStamp.Timestamp = DateTime.Now;
                                timeStamp.ActionMade = $"Student Marked Tardy Automatically at {DateTime.Now} for Bell Attendance";
                                timeStamp.MadeBy = $"SAMS Program - {DateTime.Now} - Automatic Scan Update";
                                timeStamp.Comments = "Student was marked tardy because the student scanned in 25 minutes after the start of the bell, and before the end of the bell. Hall-pass feature not available yet, and hall-pass was not checked. Please contact the teacher/admin/attendance office for any questions or concerns regarding this.";
                            }
                            else if (time <= startTimeAsDetermined.Add(TimeSpan.FromMinutes(20)) && time >= startTimeAsDetermined.Subtract(TimeSpan.FromMinutes(5)))
                            {
                                bellAttendanceEntry.Status = "Present";
                                timeStamp.Timestamp = DateTime.Now;
                                timeStamp.ActionMade = $"Student Marked Present Automatically at {DateTime.Now} for Bell Attendance";
                                timeStamp.MadeBy = $"SAMS Program - {DateTime.Now} - Automatic Scan Update";
                                timeStamp.Comments = "Student was marked present because the student scanned in within 25 minutes of the start of the bell. Please contact the teacher/admin/attendance office for any questions or concerns regarding this.";
                            }

                            _context.bellAttendanceModels.Update(bellAttendanceEntry);
                            _context.timestampModels.Add(timeStamp);
                        }

                        var studLocationEntry = _context.studentLocationModels.Any(a => a.StudentId == studId);
                        if (studLocationEntry)
                        {
                            var studLocation = _context.studentLocationModels.FirstOrDefault(a => a.StudentId == studId);
                            if (studLocation != null)
                            {
                                var room = await _context.roomLocationInfoModels.FindAsync(roomIdForCourse);
                                studLocation.StudentLocation = $"{room?.RoomNumberMod} - {room?.Teacher?.TeacherFirstNameMod} {room?.Teacher?.TeacherLastNameMod}";
                                var timestamp = new TimestampModel
                                {
                                    Timestamp = DateTime.Now,
                                    ActionMade = "Student Location Update",
                                    MadeBy = "Scan Feature of SAMS",
                                    Comments = $"The student scanned to update their location to {room?.RoomNumberMod} - {room?.Teacher?.TeacherFirstNameMod} {room?.Teacher?.TeacherLastNameMod}"
                                };
                                _context.timestampModels.Add(timestamp);
                                _context.studentLocationModels.Update(studLocation);
                            }
                        }

                        return Json(new { redirectUrl = Url.Action("Index", "Home") });
                    }
                }
                return Json(new { dangertext = "You are a hacker! What did you do wrong?" });
            }
            catch (Exception ex)
            {
                return Json(new { dangertext = $"An error occurred: {ex.Message}" });
            }
        }

        private int GetCourseIdForCurrentBell(string currentBell, Sem1StudSchedule studentSchedule)
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
                    throw new ArgumentOutOfRangeException(nameof(currentBell), "Invalid bell name provided.");
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
                        List<IBellSchedule> dailySchedule = _context.dailyBellScheduleModels.OrderBy(a => a.StartTime).Cast<IBellSchedule>().ToList();
                        foreach (var entry in dailySchedule)
                        {
                            IBellSchedule bell = entry;
                            if (time >= bell.StartTime && time <= bell.EndTime)
                            {
                                startTimeDetermined = bell.StartTime.ToString();
                                endTimeDetermined = bell.EndTime.ToString();
                                bellName = bell.BellName;
                                returnStuff[0] = startTimeDetermined;
                                returnStuff[1] = bellName;
                                returnStuff[2] = endTimeDetermined;
                            }
                        }
                        return returnStuff;
                    }
                case "Extended Aves Bell Schedule":
                    {
                        List<IBellSchedule> extSchedule = _context.extendedAvesModels.OrderBy(a => a.StartTime).Cast<IBellSchedule>().ToList();
                        foreach (var entry in extSchedule)
                        {
                            IBellSchedule bell = entry;
                            if (time >= bell.StartTime && time <= bell.EndTime)
                            {
                                startTimeDetermined = bell.StartTime.ToString();
                                endTimeDetermined = bell.EndTime.ToString();
                                bellName = bell.BellName;
                                returnStuff[0] = startTimeDetermined;
                                returnStuff[1] = bellName;
                                returnStuff[2] = endTimeDetermined;
                            }
                        }
                        return returnStuff;
                    }
                case "Pep Rally Bell Schedule":
                    {
                        List<IBellSchedule> pepSchedule = _context.pepRallyBellScheduleModels.OrderBy(a => a.StartTime).Cast<IBellSchedule>().ToList();
                        foreach (var entry in pepSchedule)
                        {
                            IBellSchedule bell = entry;
                            if (time >= bell.StartTime && time <= bell.EndTime)
                            {
                                startTimeDetermined = bell.StartTime.ToString();
                                endTimeDetermined = bell.EndTime.ToString();
                                bellName = bell.BellName;
                                returnStuff[0] = startTimeDetermined;
                                returnStuff[1] = bellName;
                                returnStuff[2] = endTimeDetermined;
                            }
                        }
                        return returnStuff;
                    }
                case "2 Hour Delay Bell Schedule":
                    {
                        List<IBellSchedule> twoSchedule = _context.twoHrDelayBellScheduleModels.OrderBy(a => a.StartTime).Cast<IBellSchedule>().ToList();
                        foreach (var entry in twoSchedule)
                        {
                            IBellSchedule bell = entry;
                            if (time >= bell.StartTime && time <= bell.EndTime)
                            {
                                startTimeDetermined = bell.StartTime.ToString();
                                endTimeDetermined = bell.EndTime.ToString();
                                bellName = bell.BellName;
                                returnStuff[0] = startTimeDetermined;
                                returnStuff[1] = bellName;
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
