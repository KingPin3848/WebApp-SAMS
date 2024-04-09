using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Signing;
using SAMS.Data;
using SAMS.Interfaces;
using SAMS.Models;
using System.Diagnostics;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SAMS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        //[Authorize(Roles = "Student, Developer")]
        public async Task<IActionResult> Scan()
        {
            var user = await _userManager.GetUserAsync(User);
            ViewBag.Schoolid = user?.SchoolId;
            return View();

        }

        [Authorize(Roles = "Student, Developer")]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Scan(string ScannedCode, string issuedSchoolId)
        {
            try
            {
                var time = DateTime.Now.TimeOfDay;
                var user = await _userManager.GetUserAsync(User);

                if (user == null)
                {
                    _logger.LogCritical("User not found");
                    return Json(new { dangertext = "User not found for the provided school ID." });
                }

                var chosenBellSchedule = _context.chosenBellSchedModels.Select(a => a.Name).ToList();
                if (chosenBellSchedule == null || chosenBellSchedule.Count == 0)
                {
                    _logger.LogCritical("Chosen Bell Schedule for the day not found. Please contact the administrator and developer for additional assistance.");
                    return Json(new { dangertext = "Chosen Bell Schedule for the day not found. Please contact the administrator and developer for additional assistance." });
                }

                var determination = DetermineCurrentBell(chosenBellSchedule[0]!);
                var currentBell = determination[0];
                TimeSpan startTimeAsDetermined = TimeSpan.Parse(determination[1]);
                TimeSpan endTimeAsDetermined = TimeSpan.Parse(determination[2]);
                if (currentBell == "School not in session!")
                {
                    _logger.LogCritical("School is not in session and you cannot sign in right now. If you think this is a mistake, please contact the admin and the developers ASAP.");
                    return Json(new { dangertext = "School is not in session and you cannot sign in right now. If you think this is a mistake, please contact the admin and the developers ASAP." });
                }

                var schoolIDdb = user.SchoolId;
                if (schoolIDdb == null)
                {
                    _logger.LogCritical($"Unable to find the school id with the user with ID {_userManager.GetUserId(User)}.");
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
                        _logger.LogCritical("Your Schedule could not be retrived because it is completely empty. Please contact your counselor ASAP and notify them of this error.");
                        return Json(new { dangertext = "Your Schedule could not be retrived because it is completely empty. Please contact your counselor ASAP and notify them of this error." });
                    }

                    var courseIdForCurrentBell = GetCourseIdForCurrentBell(currentBell, studentSchedule);
                    if (courseIdForCurrentBell == 0)
                    {
                        _logger.LogCritical("Invalid Course Id. This is an error on our end. Please contact the developers and share the whole experience step-by-step on what happened exactly.");
                        return Json(new { dangertext = "Invalid Course Id. This is an error on our end. Please contact the developers and share the whole experience step-by-step on what happened exactly." });
                    }

                    var roomIdForCourse = _context.activeCourseInfoModels.Where(a => a.CourseId == courseIdForCurrentBell).Select(a => a.CourseRoomID).FirstOrDefault();
                    if (roomIdForCourse == 0)
                    {
                        _logger.LogCritical("Room ID not found for the course. Please check with the admins to add the respective roomid in SAMS.");
                        return Json(new { dangertext = "Room ID not found for the course. Please check with the admins to add the respective roomid in SAMS." });
                    }

                    var expectedQRCode = _context.roomQRCodeModels.Where(a => a.RoomId == roomIdForCourse).Select(a => a.Code).ToList().FirstOrDefault();
                    if (expectedQRCode == null)
                    {
                        _logger.LogCritical("Expected QR Code not found for the room you are in at right now. Please contact the admin to add the room in SAMS.");
                        return Json(new { dangertext = "Expected QR Code not found for the room you are in at right now. Please contact the admin to add the room in SAMS." });
                    }

                    var passedCode = ScannedCode;
                    if (passedCode == expectedQRCode)
                    {
                        var dailyEntryExists = _context.dailyAttendanceModels.Any(a =>
                            a.StudentId == studId &&
                            a.AttendanceDate == DateOnly.FromDateTime(DateTime.Now) &&
                            a.Status == "Unknown");
                        if (dailyEntryExists)
                        {
                            var dailyAttendanceEntry = _context.dailyAttendanceModels.First(a =>
                                a.StudentId == studId &&
                                a.AttendanceDate == DateOnly.FromDateTime(DateTime.Now) &&
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
                case "Bell 5 A":
                    if (DateTime.Now.Date.DayOfWeek == DayOfWeek.Monday || DateTime.Now.Date.DayOfWeek == DayOfWeek.Wednesday)
                    {
                        return studentSchedule.Bell5MonWedCourseIDMod;
                    }
                    return studentSchedule.Bell5TueThurCourseIDMod;
                case "Bell 5 B":
                    if (DateTime.Now.Date.DayOfWeek == DayOfWeek.Monday || DateTime.Now.Date.DayOfWeek == DayOfWeek.Wednesday)
                    {
                        return studentSchedule.Bell5MonWedCourseIDMod;
                    }
                    return studentSchedule.Bell5TueThurCourseIDMod;
                case "Bell 5 C":
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
                                returnStuff[0] = bellName;
                                returnStuff[1] = startTimeDetermined;
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
                                returnStuff[0] = bellName;
                                returnStuff[1] = startTimeDetermined;
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
                                returnStuff[0] = bellName;
                                returnStuff[1] = startTimeDetermined;
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

        [HttpGet]
        public async Task<IActionResult> Form()
        {
            var user = await _userManager.GetUserAsync(User);
            ViewBag.Schoolid = user.SchoolId;
            return View();
        }

        //[HttpPost]
        //public async Task<IActionResult> Form()
        //{
        //    var user = await _userManager.GetUserAsync(User);
        //    ViewBag.Schoolid = user.SchoolId;

        //    return View();
        //}
    }
}

/*
             var roomCodes = _context.roomQRCodeModels.Select(a => a.Code).ToList();
            var chosenBellSched = _context.chosenBellSchedModels.Select(a => a.Name).ToList();
            var dailyBellSched = _context.dailyBellScheduleModels.Select(a => a.StartTime).ToList();
            var dailyBellDuration = _context.dailyBellScheduleModels.Select(a =>a.Duration).ToList();

            var twoHourDelaySchedule = _context.twoHrDelayBellScheduleModels.Select(a => a.StartTime).ToList();
            var twoHourDelayDuration = _context.twoHrDelayBellScheduleModels.Select(a => a.Duration).ToList();

            var pepRallySchedule = _context.pepRallyBellScheduleModels.Select(a => a.StartTime).ToList();
            var pepRallyDuration = _context.pepRallyBellScheduleModels.Select(a => a.Duration).ToList();

            var extendedAvesSchedule = _context.extendedAvesModels.Select(a => a.StartTime).ToList();
            var extendedAvesDuration = _context.extendedAvesModels.Select(a => a.Duration).ToList();

            var studentBellSchedule = _context.studentScheduleInfoModels.Select(a => a.Bell1EnrollmentCodeMod).ToList();

            for (int indexer = 0; indexer < roomCodes.Count; indexer++)
            {
                if (roomCodes[indexer].Equals(camResult))
                {
                    if (chosenBellSched.Equals("Daily Bell Schedule"))
                    {
                        for (int i = 0; i < dailyBellSched.Count; i++)
                        {
                            if (parsedTimestamp.Subtract(dailyBellSched[i]).TimeOfDay < dailyBellDuration[i])
                            {
                                return NotFound($"Your atendance has been marked for '{studentBellSchedule[0]}'"); //change this to view later

                            }
                        }
                        return NotFound($"You are not supposed to be in this bell right now'"); //change this to view later
                    }
                    else if (chosenBellSched.Equals("2 Hour Delay Bell Schedule"))
                    {
                        for (int i = 0; i < twoHourDelaySchedule.Count; i++)
                        {
                            if (parsedTimestamp.Subtract(twoHourDelaySchedule[i]).TimeOfDay < twoHourDelayDuration[i])
                            {
                                return NotFound($"Your atendance has been marked for '{studentBellSchedule[0]}'"); //change this to view later

                            }
                        }
                        return NotFound($"You are not supposed to be in this bell right now'"); //change this to view later

                    }
                    else if (chosenBellSched.Equals("Pep Rally Bell Schedule"))
                    {
                        for (int i = 0; i < pepRallySchedule.Count; i++)
                        {
                            if (parsedTimestamp.Subtract(pepRallySchedule[i]).TimeOfDay < pepRallyDuration[i])
                            {
                                return NotFound($"Your atendance has been marked for '{studentBellSchedule[0]}'"); //change this to view later

                            }
                        }
                        return NotFound($"You are not supposed to be in this bell right now'"); //change this to view later

                    }
                    else if (chosenBellSched.Equals("Extended Aves Bell Schedule"))
                    {
                        for (int i = 0; i < extendedAvesSchedule.Count; i++)
                        {
                            if (parsedTimestamp.Subtract(extendedAvesSchedule[i]).TimeOfDay < extendedAvesDuration[i])
                            {
                                return NotFound($"Your atendance has been marked for '{studentBellSchedule[0]}'"); //change this to view later

                            }
                        }
                        return NotFound($"You are not supposed to be in this bell right now'"); //change this to view later
                    }

                }
            }
 */
/*
So this is what I want to do with those two files:

Home controller's scan method would get two things: decoded qr code data and the schoolissuedid from the method constructor/parameter. Then it needs to get the user by searching for the user using the schoolissuedid (which is not a primary key) and once found, it should get the entire entry.

Then there is a database table that has codes for all rooms which later in the view are going to be converted into a qr code. Plus it is going to get the bell schedule that is running for the day which is through this code: var chosenBellSched = _context.ChosenBellSchedModel.Select(a => a.Name).ToList(); var scheduleForTheDay = chosenBellSched[0];

There are 4 different types of bell schedules and inside the ChosenBellSchedModel, there will only be one entry with the name of the bell schedule that will be used to run the schedule for the day. It can be either "Daily Bell Schedule", or "Extended Aves Bell Schedule", or "Pep Rally Bell Schedule" or "2 Hour Delay Bell Schedule". Only one of those 4. And each bell schedule has its own model and is connected to an interface called IBellSchedule - so like daily bell schedule has a model called DailyBellScheduleModel.cs, pep rally has PepRallyBellScheduleModel.cs, extended aves has ExtendedAvesBellScheduleModel.cs, and 2 hour delay has TwoHrBellScheduleModel.cs. Each bell schedule has the same properties but unique data. Properties are:
public string BellName { get; set; }
public TimeSpan StartTime { get; set; }
public TimeSpan EndTime { get; set; }
public TimeSpan Duration { get; set; }
StartTime is the primary key in all of the bell schedule models. Now, once you have determined the chosen bell schedule for the day, you need to get the current time and based on that determine what bell it is from the respective bell schedule model that has been chosen through chosenBellSched. Once you get that, then it needs to get the student's schedule which has all the courses 
 */








/*string camResult = ScannedCode;
var camDateTime = DateTime.Now.TimeOfDay;

// Parsing the timestamp since it's a string
//DateTime.TryParse(camResultTimestamp, out DateTime parsedTimestamp);

TimeOnly timeOnly = TimeOnly.FromTimeSpan(camDateTime);

string passedschoolid = issuedSchoolId;

var user = await _userManager.GetUserAsync(User);
if (user == null)
{
    return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
}

var schoolIDdb = user.SchoolId;
if (schoolIDdb == null)
{
    return Json(new { dangertext = $"Unable to find the school id with the user with ID '{_userManager.GetUserId(User)}'." });
    //return NotFound($"Unable to find the school id with the user with ID '{_userManager.GetUserId(User)}'.");
}
else
{
    if (schoolIDdb == passedschoolid)
    {
        //Do nothing yet
    }
    else
    {
        return Json(new { dangertext = "The School ID was not found and wasn't the same as in our records. Please try again or contact the developers for additional assistance." });
        //return NotFound("The School ID was not found and wasn't the same as in our records. Please try again or contact the developers for additional assistance.");
    }
}

var roomCodes = _context.roomQRCodeModels.Select(a => a.Code).ToList();
var chosenBellSched = _context.ChosenBellSchedModel.Select(a => a.Name).ToList();
var scheduleForTheDay = chosenBellSched[0];

var studentBellSchedule = await _context.studentScheduleInfoModels.FindAsync(passedschoolid);

switch (scheduleForTheDay)
{
    case "Daily Bell Schedule":
        {
            var dailyBellSchedNames = _context.dailyBellScheduleModels.Select(a => a.BellName).ToList();
            var dailyBellSchedStart = _context.dailyBellScheduleModels.Select(a => a.StartTime).ToList();
            var dailyBellSchedEnd = _context.dailyBellScheduleModels.Select(a => a.EndTime).ToList();
            var dailyBellDuration = _context.dailyBellScheduleModels.Select(a => a.Duration).ToList();

            string atTheTimeBell = string.Empty;
            int curBell = 0;
            for (int i = 0; i < dailyBellSchedStart.Count; i++)
            {
                if ((timeOnly.CompareTo(dailyBellSchedStart[i]) > 0) && (timeOnly.CompareTo(dailyBellSchedEnd[i]) < 0))
                {
                    atTheTimeBell = dailyBellSchedNames[i];
                    int studidpassed = int.Parse(passedschoolid);
                    var dailyAttRecord = await _context.dailyAttendanceModels.Where(a => (a.StudentId == studidpassed) && (a.AttendanceDate == DateTime.Now.Date)).FirstOrDefaultAsync();

                    if (dailyAttRecord == null)
                    {
                        return NotFound();
                    }

                    if (dailyAttRecord.Status == "Unknown")
                    {
                        string bellBasedCourseCode;
                        switch (atTheTimeBell)
                        {
                            case "Bell 1":
                                {
                                    bellBasedCourseCode = studentBellSchedule.Bell1CourseIDMod.ToString();
                                    if (bellBasedCourseCode == "Late Arrival")
                                    {
                                        TimeOnly start = new TimeOnly(07, 15, 00, 0000); // 7:20 AM
                                        TimeOnly end = new TimeOnly(08, 10, 00, 0000); // 8:10 AM

                                        if (timeOnly.IsBetween(start, end))
                                        {
                                            dailyAttRecord.Status = "Present";
                                            var timestamprecords = new TimestampModel
                                            {

                                            };

                                            _context.timestampModels.Add(timestamprecords);
                                            await _context.SaveChangesAsync();
                                        }
                                    }
                                    break;
                                }

                            case "Bell 2":
                                {
                                    bellBasedCourseCode = studentBellSchedule.Bell2CourseIDMod.ToString();
                                    break;
                                }

                            case "Bell 3":
                                {
                                    bellBasedCourseCode = studentBellSchedule.Bell3CourseIDMod.ToString();
                                    break;
                                }

                            case "Bell 4":
                                {
                                    bellBasedCourseCode = studentBellSchedule.Bell4CourseIDMod.ToString();
                                    break;
                                }

                            case "Bell 5":
                                {
                                    bellBasedCourseCode = studentBellSchedule.Bell5CourseIDMod.ToString();
                                    break;
                                }

                            case "Bell 6":
                                {
                                    bellBasedCourseCode = studentBellSchedule.Bell6CourseIDMod.ToString();
                                    break;
                                }

                            case "Bell 7":
                                {
                                    bellBasedCourseCode = studentBellSchedule.Bell7CourseIDMod.ToString();
                                    break;
                                }

                            default:
                                {
                                    break;
                                }
                        }
                    }
                    //THIS IS WHERE WE NEED TO CHECK THE HALL-PASSES IF THERE IS ONE, AND IF THERE IS ONE THEN WE 
                    else if (true)
                    {

                    }
                }
            }


            break;
        }

    case "Pep Rally Bell Schedule":
        {
            var peprallyBellSchedStart = _context.pepRallyBellScheduleModels.Select(a => a.StartTime).ToList();
            var peprallyBellSchedEnd = _context.pepRallyBellScheduleModels.Select(a => a.EndTime).ToList();
            var peprallyBellDuration = _context.pepRallyBellScheduleModels.Select(a => a.Duration).ToList();


            break;
        }

    case "2 Hour Delay Bell Schedule":
        {
            var _2hrdelayBellSchedStart = _context.twoHrDelayBellScheduleModels.Select(a => a.StartTime).ToList();
            var _2hrdelayBellSchedEnd = _context.twoHrDelayBellScheduleModels.Select(a => a.EndTime).ToList();
            var _2hrdelayBellDuration = _context.twoHrDelayBellScheduleModels.Select(a => a.Duration).ToList();


            break;
        }

    case "Extended Aves Bell Schedule":
        {
            var extavesBellSchedStart = _context.extendedAvesModels.Select(a => a.StartTime).ToList();
            var extavesBellSchedEnd = _context.extendedAvesModels.Select(a => a.EndTime).ToList();
            var extavesBellDuration = _context.extendedAvesModels.Select(a => a.Duration).ToList();


            break;
        }

    default:
        {
            return NotFound();
        }

}


//for (int indexer = 0; indexer < roomCodes.Count; indexer++)
//{
//    if (roomCodes[indexer].Equals(camResult))
//    {
//        //the qr code is one of the classes in the school

//        if (chosenBellSched[0].Equals("Daily Bell Schedule"))
//        {
//            //we are using daily bell schedule
//            for (int i = 0; i < dailyBellSchedStart.Count; i++)
//            {
//                if ((timeOnly.CompareTo(dailyBellSchedStart[i]) > 0) && (timeOnly.CompareTo(dailyBellSchedEnd[i]) < 0))
//                {
//                    var nullcheckForDailyAttendance = await _context.dailyAttendanceModels.FindAsync(passedschoolid);
//                    if (nullcheckForDailyAttendance != null)
//                    {
//                        if (nullcheckForDailyAttendance.Status == "Unknown")
//                        {

//                            nullcheckForDailyAttendance.Status = "Unknown";
//                        }
//                    }
//                    return Json(new { redirectUrl = Url.Action("Privacy") });
//                    //The qr code was scanned during the school hours

//                }
//            }
//            return Json(new { redirectUrl = Url.Action("Scan") });
//            //The qr code was not scanned during the school hours
//        }
//        else if (chosenBellSched[0].Equals("2 Hour Delay Bell Schedule"))
//        {
//            for (int i = 0; i < twoHourDelaySchedule.Count; i++)
//            {
//                if (timeOnly.Subtract(twoHourDelaySchedule[i]).TimeOfDay < twoHourDelayDuration[i])
//                {
//                    return NotFound($"Your atendance has been marked for '{studentBellSchedule[0]}'"); //change this to view later

//                }
//            }
//            return NotFound($"You are not supposed to be in this bell right now'"); //change this to view later

//        }
//        else if (chosenBellSched[0].Equals("Pep Rally Bell Schedule"))
//        {
//            for (int i = 0; i < pepRallySchedule.Count; i++)
//            {
//                if (timeOnly.Subtract(pepRallySchedule[i]).TimeOfDay < pepRallyDuration[i])
//                {
//                    return NotFound($"Your atendance has been marked for '{studentBellSchedule[0]}'"); //change this to view later

//                }
//            }
//            return NotFound($"You are not supposed to be in this bell right now'"); //change this to view later

//        }
//        else if (chosenBellSched[0].Equals("Extended Aves Bell Schedule"))
//        {
//            for (int i = 0; i < extendedAvesSchedule.Count; i++)
//            {
//                if (timeOnly.Subtract(extendedAvesSchedule[i]).TimeOfDay < extendedAvesDuration[i])
//                {
//                    return NotFound($"Your atendance has been marked for '{studentBellSchedule[0]}'"); //change this to view later

//                }
//            }
//            return NotFound($"You are not supposed to be in this bell right now'"); //change this to view later
//        }

//    }
//}

return Json(new { redirectUrl = Url.Action("Index") });


//var roomCodes = _context.roomQRCodeModels.Select(a => a.Code).ToList();
//if (roomCodes.Count == 0)
//{
//    return Json(new { dangertext = "Room Codes table is completely empty. Please check in with the developers to resolve this issue asap." });
//}

//return Json(new { redirectUrl = Url.Action("Index") });*/