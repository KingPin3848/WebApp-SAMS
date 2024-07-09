using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Common;
using OfficeOpenXml.FormulaParsing.Exceptions;
using SAMS.Controllers;
using SAMS.Data;
using SAMS.Interfaces;
using SAMS.Models;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Drawing.Text;

namespace SAMS.Areas.Class.Controllers
{
    [Area("Class")]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public class ClassKioskController(IServiceScopeFactory serviceScopeFactory) : Controller
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
        private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;
        private string MessAge { get; set; }
        private bool ReFresh { get; set; }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(QRCodeModel input)
        {
            //Necessary variables to get services from ServiceScopeFactory
            using var scope = _serviceScopeFactory.CreateAsyncScope();
            using var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var datetime = DateTime.Now;

            if (input is null)
            {
                return Json(new { error = "The QR Code reading and Pin number cannot be empty. Please try again." });
            }

            //Necessary variables for each element inside QRCodeModel parameter
            string localScannedCode = input.ScannedCode;
            int localStudPin = input.StudentPin;
            string subScanNum = localScannedCode[..4];
            int scannernumber = new();
            if (int.TryParse(subScanNum, out scannernumber))
            {

                ApplicationUser dbStudentUser = userManager.Users.Where(a => a.ActivationCode == localScannedCode).First();
                if (dbStudentUser is null)
                {
                    return Json(new { success = false, message = "Authentication unsuccessful because student details could not be found. Please make sure the student is registered in the system.", refresh = true, seconds = 10000 });
                }

                if (CodeVerifier(localScannedCode) && PinVerifier(localStudPin))
                {
                    var dbname = dbStudentUser.UserName;
                    bool marker = AttendanceMarker(dbStudentUser, scannernumber, datetime).Result;
                    int timseconds = 0;
                    if (marker) { timseconds = 5000; } else { timseconds = 10000; }

                    return Json(new { success = marker, message = $"Authentication was successful, {dbname}.\n\nMessage: {MessAge};", refresh = ReFresh, seconds = timseconds });
                }
                else
                {
                    return Json(new { success = false, message = "Authentication unsuccessful. Please try again by checking/rechecking the Pin entered. In case you forgot your pin, please contact a staff member to obtain your Pin.", refresh = true, seconds = 10000 });
                }

            }
            else
            {
                return Json(new { success = false, message = "Couldn't fetch the scanner details linked to this device. Please notify the administrators about this issue ASAP.", refresh = true, seconds = 10 });
            }
        }

        private bool CodeVerifier(string code)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            using UserStore<ApplicationUser> userStore = scope.ServiceProvider.GetRequiredService<UserStore<ApplicationUser>>();
            ApplicationUser dbStudentUser = userStore.Users.Where(a => a.ActivationCode == code).First();

            if (code == dbStudentUser.ActivationCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool PinVerifier(int pin)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            using var userStore = scope.ServiceProvider.GetRequiredService<UserStore<ApplicationUser>>();
            ApplicationUser dbStudentUser = userStore.Users.Where(a => a.StudentPin == pin).First();

            if(pin == dbStudentUser.StudentPin)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private async Task<bool> AttendanceMarker(ApplicationUser student, int scannernumber, DateTime datetime)
        {
            if (student == null)
            {

                MessAge = "No details found for the student user. Please make sure the student is registered in the system.";
                ReFresh = true;
                return false;
            }
            else
            {
                using var scope = _serviceScopeFactory.CreateScope();
                using var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                using var dbcontext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var time = datetime.TimeOfDay;

                var bellschedule = GetBellSchedule();
                var lists = GetCurrentBell(bellschedule);
                var currentbell = lists[0];
                TimeSpan starttime = new();
                if (TimeSpan.TryParse(lists[1], out starttime))
                {
                    //We do nothing here.
                }
                TimeSpan endtime = new();
                if (TimeSpan.TryParse(lists[2], out endtime))
                {
                    //We do nothing here.
                }

                if (currentbell == "School not in session!")
                {
                    MessAge = "School is not in session and you cannot sign in right now. If you think this is a mistake, please contact the admin and the developers ASAP.";
                    ReFresh = true;
                    return false;
                }

                if (await StudentRoleChecker(student).ConfigureAwait(true))
                {
                    int studId = new();
                    if (int.TryParse(student.SchoolId, out studId))
                    {

                        var sem2start = dbcontext.SchedulerModels.Where(a => a.Type == "Semester 2").Select(a => a.Date).First();
                        IStudentSchedule? studentSchedule = (DateOnly.FromDateTime(DateTime.Now.Date) >= sem2start) ? (await dbcontext.Sem2StudSchedules.FindAsync(studId).ConfigureAwait(true)) : (await dbcontext.Sem1StudSchedules.FindAsync(studId).ConfigureAwait(true));
                        if (studentSchedule == null)
                        {
                            MessAge = "Your Class Schedule could not be retrived because it is empty. Please contact your counselor ASAP and notify them of this error.";
                            ReFresh = true;
                            return false;
                        }

                        var courseIdForCurrentBell = GetCourseIdForCurrentBell(currentbell, studentSchedule);
                        if (courseIdForCurrentBell == 0)
                        {
                            MessAge = "Couldn't retrieve your course for this bell. (Invalid Course Id.) Please contact the developers and share the whole experience step-by-step on what happened exactly.";
                            ReFresh = true;
                            return false;
                        }

                        var roomIdByCourseCurrentBell = dbcontext.ActiveCourseInfoModels.Where(a => a.CourseId == courseIdForCurrentBell).Select(a => a.CourseRoomID).First();
                        if (roomIdByCourseCurrentBell == 0)
                        {
                            MessAge = "Room ID not found for the course. Please check with the admins to add the respective room in the system.";
                            ReFresh = true;
                            return false;
                        }

                        var roomIdByScanner = GetRoomFromScanner(scannernumber);
                        if (roomIdByScanner == -1)
                        {
                            MessAge = "There has been a havoc inside the database by Microsoft. Please be patient, and let the administrators and developers know about this issue and wait for the issue to be resolved.";
                            ReFresh = true;
                            return false;
                        }

                        if (roomIdByScanner == roomIdByCourseCurrentBell)
                        {
                            string finalmessage = "The following attendance category(ies) was/were successfully updated: ";

                            var dailyEntryExists = dbcontext.DailyAttendanceModels.Any(a =>
                            a.StudentId == studId &&
                            a.AttendanceDate == DateOnly.FromDateTime(DateTime.Now.Date) &&
                            a.Status == "Unknown");
                            if (dailyEntryExists)
                            {
                                var dailyAttendanceEntry = dbcontext.DailyAttendanceModels.First(a =>
                                a.StudentId == studId &&
                                a.AttendanceDate == DateOnly.FromDateTime(DateTime.Now.Date) &&
                                a.Status == "Unknown");
                                var timeStamp = new TimestampModel();

                                if (time >= starttime.Add(TimeSpan.FromMinutes(5)) && time <= endtime)
                                {
                                    datetime = DateTime.Now;

                                    dailyAttendanceEntry.Status = "Tardy";
                                    timeStamp.Timestamp = datetime;
                                    timeStamp.ActionMade = $"Student Marked Tardy Automatically at {datetime} for Daily Attendance";
                                    timeStamp.MadeBy = $"SAMS Program Scan - {datetime} - Automatic Class Kiosk";
                                    timeStamp.Comments = $"Student was marked tardy because the student scanned in 5 minutes after the start of the in-school bell, and before the end of the bell - {datetime}. Hall-pass feature not available yet, and hall-pass was not checked. Please contact the teacher/admin/attendance office for any questions or concerns regarding this.";
                                }
                                else if (time <= starttime.Add(TimeSpan.FromMinutes(5)) && time >= starttime.Subtract(TimeSpan.FromMinutes(4)))
                                {
                                    datetime = DateTime.Now;

                                    dailyAttendanceEntry.Status = "Present";
                                    timeStamp.Timestamp = datetime;
                                    timeStamp.ActionMade = $"Student Marked Present Automatically at {datetime} for Daily Attendance";
                                    timeStamp.MadeBy = $"SAMS Program - {datetime} - Automatic Class Kiosk";
                                    timeStamp.Comments = $"Student was marked present because the student scanned in within 5 minutes of the start of the in-school bell. {datetime} Please contact the teacher/admin/attendance office for any questions or concerns regarding this.";
                                }
                                finalmessage += " Daily Attendance; ";
                                dbcontext.DailyAttendanceModels.Update(dailyAttendanceEntry);
                                dbcontext.TimestampModels.Add(timeStamp);
                                await dbcontext.SaveChangesAsync().ConfigureAwait(true);
                            }

                            var bellEntryExists = dbcontext.BellAttendanceModels.Any(a =>
                            a.StudentId == studId &&
                            a.DateTime.Date == DateTime.Now.Date &&
                            a.BellNumId == currentbell &&
                            a.Status == "Unknown");
                            if (bellEntryExists)
                            {
                                var bellAttendanceEntry = dbcontext.BellAttendanceModels.First(a =>
                                    a.StudentId == studId &&
                                    a.DateTime.Date == DateTime.Now.Date &&
                                    a.BellNumId == currentbell &&
                                    a.Status != "Present");
                                var timeStamp = new TimestampModel();

                                if (time >= starttime.Add(TimeSpan.FromMinutes(5)) && time <= starttime)
                                {
                                    datetime = DateTime.Now;

                                    bellAttendanceEntry.Status = "Tardy";
                                    timeStamp.Timestamp = DateTime.Now;
                                    timeStamp.ActionMade = $"Student Marked Tardy Automatically at {DateTime.Now} for Bell Attendance";
                                    timeStamp.MadeBy = $"SAMS Program - {datetime} - Automatic Class Kiosk";
                                    timeStamp.Comments = "Student was marked tardy because the student scanned in 5 minutes after the start of the bell, and before the end of the bell. Hall-pass feature not available yet, and hall-pass was not checked. Please contact the teacher/admin/attendance office for any questions or concerns regarding this.";
                                }
                                else if (time <= starttime.Add(TimeSpan.FromMinutes(5)) && time >= starttime.Subtract(TimeSpan.FromMinutes(4)))
                                {
                                    datetime = DateTime.Now;

                                    bellAttendanceEntry.Status = "Present";
                                    timeStamp.Timestamp = DateTime.Now;
                                    timeStamp.ActionMade = $"Student Marked Present Automatically at {datetime} for Bell Attendance";
                                    timeStamp.MadeBy = $"SAMS Program - {datetime} - Automatic Class Kiosk";
                                    timeStamp.Comments = "Student was marked present because the student scanned in within 5 minutes of the start of the bell. Please contact the teacher/admin/attendance office for any questions or concerns regarding this.";
                                }
                                finalmessage += "Bell Attendance; ";
                                dbcontext.BellAttendanceModels.Update(bellAttendanceEntry);
                                dbcontext.TimestampModels.Add(timeStamp);
                                await dbcontext.SaveChangesAsync().ConfigureAwait(true);
                            }

                            var studLocationEntry = dbcontext.StudentLocationModels.Any(a => a.StudentId == studId);
                            if (studLocationEntry)
                            {
                                var studLocation = dbcontext.StudentLocationModels.First(a => a.StudentId == studId);
                                if (studLocation != null)
                                {
                                    datetime = DateTime.Now;

                                    var room = await dbcontext.RoomLocationInfoModels.FindAsync(roomIdByScanner).ConfigureAwait(true);
                                    studLocation.StudentLocation = $"{room!.RoomNumberMod} - {room!.Teacher!.TeacherFirstNameMod} {room!.Teacher!.TeacherLastNameMod}";
                                    var timestamp = new TimestampModel
                                    {
                                        Timestamp = datetime,
                                        ActionMade = "Student Location Update",
                                        MadeBy = "Class Kiosk Feature of SAMS",
                                        Comments = $"Student scanned at the Class Kiosk updating location to {room!.RoomNumberMod} - {room!.Teacher?.TeacherFirstNameMod} {room!.Teacher?.TeacherLastNameMod}"
                                    };
                                    finalmessage += "✓✓";
                                    dbcontext.TimestampModels.Add(timestamp);
                                    dbcontext.StudentLocationModels.Update(studLocation);
                                    await dbcontext.SaveChangesAsync().ConfigureAwait(true);
                                }
                                else
                                {
                                    MessAge = "Student location through attendance is not configured for you yet. Please contact the administrators as soon as possible and let them know about this to be configured.";
                                    ReFresh = true;
                                    return false;
                                }
                            }
                            else
                            {
                                var room = await dbcontext.RoomLocationInfoModels.FindAsync(roomIdByScanner).ConfigureAwait(true);
                                var studLocation = new StudentLocationModel
                                {
                                    StudentId = studId,
                                    StudentName = (dbcontext.StudentInfoModels.Where(a => a.StudentID == studId).First().StudentFirstNameMod) + (dbcontext.StudentInfoModels.Where(a => a.StudentID == studId).First().StudentLastNameMod),
                                    StudentLocation = $"{room!.RoomNumberMod} - {room!.Teacher!.TeacherFirstNameMod} {room!.Teacher!.TeacherLastNameMod}"
                                };

                                var timestamp = new TimestampModel
                                {
                                    Timestamp = datetime,
                                    ActionMade = "Student Location Update",
                                    MadeBy = "Class Kiosk Feature of SAMS",
                                    Comments = $"Student scanned at the Class Kiosk updating location to {room!.RoomNumberMod} - {room!.Teacher?.TeacherFirstNameMod} {room!.Teacher?.TeacherLastNameMod}"
                                };
                                finalmessage += "✓✓";
                                dbcontext.StudentLocationModels.Add(studLocation);
                                dbcontext.TimestampModels.Add(timestamp);
                                await dbcontext.SaveChangesAsync().ConfigureAwait(true);
                            }
                            MessAge = finalmessage + " \n\n If you think that any of the attendance categories shouldn't have been updated, then please contact the attendance office immediately and report this issue.";
                            ReFresh = true;
                            return true;
                        }
                        else
                        {
                            MessAge = "The class is not taught here, or at least not updated in the system, if you think this is an issue. Please contact the counselors or administrators for this issue.";
                            ReFresh = true;
                            return false;
                        }

                    } else
                    {
                        MessAge = "Couldn't retrieve the student Id for authentication.";
                        ReFresh = true;
                        return false;
                    }
                }
                else
                {
                    MessAge = "Your role could not be verified. Please contact an administrator to make sure your role is set up correctly.";
                    ReFresh = true;
                    return false;
                }
            }
        }

        private int GetRoomFromScanner(int scannernumber)
        {
            using var scope = _serviceScopeFactory.CreateAsyncScope();
            using var dbcontext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var scannerRoom = dbcontext.HandheldScannerNodeModels.Where(a => a.ScannerID == scannernumber).Select(a => a.RoomIDMod).First();
            if (scannerRoom < 1)
            {
                return -1;
            }
            else
            {
                return scannerRoom;
            }
        }

        private async Task<bool> StudentRoleChecker(ApplicationUser student)
        {
            using var scope = _serviceScopeFactory.CreateAsyncScope();
            using var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            var roles = await userManager.GetRolesAsync(student).ConfigureAwait(true);

            foreach (var role in roles)
            {
                if (role == "Student")
                {
                    return true;
                }
            }
            return false;
        }

        private string GetBellSchedule()
        {
            using var scope = _serviceScopeFactory.CreateScope();
            using var dbcontext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            return (dbcontext.ChosenBellSchedModels.First().Name!);
        }

        private List<string> GetCurrentBell(string sched)
        {
            //Variables to get required services through ServiceScopeFactory
            using var scope = _serviceScopeFactory.CreateAsyncScope();
            using var dbcontext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            //Necessary variables
            var dateTime = DateTime.Now;
            var time = dateTime.TimeOfDay;
            string bellName = "";
            var startTimeDetermined = "";
            var endTimeDetermined = "";
            List<string> theList = [bellName, startTimeDetermined, endTimeDetermined];


            switch (sched)
            {
                case "Daily Bell Schedule":
                    {
                        List<IBellSchedule> dailySchedule = [.. dbcontext.DailyBellScheduleModels.OrderBy(a => a.StartTime).Cast<IBellSchedule>()];
                        foreach (var entry in dailySchedule)
                        {
                            IBellSchedule bell = entry;
                            if (time >= bell.StartTime && time <= bell.EndTime)
                            {
                                startTimeDetermined = bell.StartTime.ToString();
                                endTimeDetermined = bell.EndTime.ToString();
                                bellName = bell.BellName;
                                theList[0] = bellName;
                                theList[1] = startTimeDetermined;
                                theList[2] = endTimeDetermined;
                            }
                        }
                        return theList;
                    }

                case "Extended Aves Bell Schedule":
                    {
                        List<IBellSchedule> extSchedule = [.. dbcontext.ExtendedAvesModels.OrderBy(a => a.StartTime).Cast<IBellSchedule>()];
                        foreach (var entry in extSchedule)
                        {
                            IBellSchedule bell = entry;
                            if (time >= bell.StartTime && time <= bell.EndTime)
                            {
                                startTimeDetermined = bell.StartTime.ToString();
                                endTimeDetermined = bell.EndTime.ToString();
                                bellName = bell.BellName;
                                theList[0] = bellName;
                                theList[1] = startTimeDetermined;
                                theList[2] = endTimeDetermined;
                            }
                        }
                        return theList;
                    }

                case "Pep Rally Bell Schedule":
                    {
                        List<IBellSchedule> pepSchedule = [.. dbcontext.PepRallyBellScheduleModels.OrderBy(a => a.StartTime).Cast<IBellSchedule>()];
                        foreach (var entry in pepSchedule)
                        {
                            IBellSchedule bell = entry;
                            if (time >= bell.StartTime && time <= bell.EndTime)
                            {
                                startTimeDetermined = bell.StartTime.ToString();
                                endTimeDetermined = bell.EndTime.ToString();
                                bellName = bell.BellName;
                                theList[0] = bellName;
                                theList[1] = startTimeDetermined;
                                theList[2] = endTimeDetermined;
                            }
                        }
                        return theList;
                    }

                case "2 Hour Delay Bell Schedule":
                    {
                        List<IBellSchedule> twoSchedule = [.. dbcontext.TwoHrDelayBellScheduleModels.OrderBy(a => a.StartTime).Cast<IBellSchedule>()];
                        foreach (var entry in twoSchedule)
                        {
                            IBellSchedule bell = entry;
                            if (time >= bell.StartTime && time <= bell.EndTime)
                            {
                                startTimeDetermined = bell.StartTime.ToString();
                                endTimeDetermined = bell.EndTime.ToString();
                                bellName = bell.BellName;
                                theList[0] = bellName;
                                theList[1] = startTimeDetermined;
                                theList[2] = endTimeDetermined;
                            }
                        }
                        return theList;
                    }

                case "Custom Bell Schedule":
                    {
                        List<IBellSchedule> custSchedule = [.. dbcontext.CustomSchedules.OrderBy(a => a.StartTime).Where(a => a.BellName.Contains("Bell ")).Cast<IBellSchedule>()];
                        foreach (var entry in custSchedule)
                        {
                            IBellSchedule bell = entry;
                            if (time >= bell.StartTime && time <= bell.EndTime)
                            {
                                startTimeDetermined = bell.StartTime.ToString();
                                endTimeDetermined = bell.EndTime.ToString();
                                bellName = bell.BellName;
                                theList[0] = bellName;
                                theList[1] = startTimeDetermined;
                                theList[2] = endTimeDetermined;
                            }
                        }
                        return theList;
                    }

                default:
                    {
                        bellName = "School not in session!";
                        theList[0] = bellName;
                        startTimeDetermined = string.Empty;
                        theList[1] = startTimeDetermined;
                        endTimeDetermined = string.Empty;
                        theList[2] = endTimeDetermined;
                        return (theList);
                    }
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
    }

    public class QRCodeModel
    {
        [Required]
        [StringLength(32, ErrorMessage = "The Unique Code must be at least {2} and at max {1} characters long.", MinimumLength = 32)]
        public string ScannedCode { get; set; } = default!;
        [Required]
        public int StudentPin { get; set; } = default!;
    }
}
