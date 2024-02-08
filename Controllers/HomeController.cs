using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SAMS.Data;
using SAMS.Models;
using System.Diagnostics;

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
        [Authorize(Roles = "Student, Developer")]
        public async Task<IActionResult> Scan()
        {
            var user = await _userManager.GetUserAsync(User);
            ViewBag.Schoolid = user.SchoolId;
            return View();

        }

        [HttpPost]
        public async Task<IActionResult> Scan(string ScannedCode, string ScannedCodeTimestamp, string issuedSchoolId)
        {
            string camResult = ScannedCode;
            string camResultTimestamp = ScannedCodeTimestamp;

            // Parsing the timestamp since it's a string
            //DateTime.TryParse(camResultTimestamp, out DateTime parsedTimestamp);

            TimeSpan timeSpan = TimeSpan.Parse(camResultTimestamp);
            TimeOnly timeOnly = TimeOnly.FromTimeSpan(timeSpan);

            string passedschoolid = issuedSchoolId;

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var schoolIDdb = user.SchoolId;
            if (schoolIDdb == null)
            {
                return Json(new { dangertext = $"Unable to find the school id with the user with ID '{_userManager.GetUserId(User)}'."});
                //return NotFound($"Unable to find the school id with the user with ID '{_userManager.GetUserId(User)}'.");
            }
            else
            {
                if (schoolIDdb == passedschoolid)
                {
                    int bellScheduleEnabled = 0;
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
                                                bellBasedCourseCode = studentBellSchedule.Bell1EnrollmentCodeMod.ToString();
                                                break;
                                            }

                                        case "Bell 2":
                                            {
                                                bellBasedCourseCode = studentBellSchedule.Bell2EnrollmentCodeMod.ToString();
                                                break;
                                            }

                                        case "Bell 3":
                                            {
                                                bellBasedCourseCode = studentBellSchedule.Bell3EnrollmentCodeMod.ToString();
                                                break;
                                            }

                                        case "Bell 4":
                                            {
                                                bellBasedCourseCode = studentBellSchedule.Bell4EnrollmentCodeMod.ToString();
                                                break;
                                            }

                                        case "Bell 5":
                                            {
                                                bellBasedCourseCode = studentBellSchedule.Bell5EnrollmentCodeMod.ToString();
                                                break;
                                            }

                                        case "Bell 6":
                                            {
                                                bellBasedCourseCode = studentBellSchedule.Bell6EnrollmentCodeMod.ToString();
                                                break;
                                            }

                                        case "Bell 7":
                                            {
                                                bellBasedCourseCode = studentBellSchedule.Bell7EnrollmentCodeMod.ToString();
                                                break;
                                            }

                                        default:
                                            {
                                                break;
                                            }
                                    }
                                }
                                //THIS IS WHERE WE NEED TO CHECK THE HALL-PASSES IF THERE IS ONE, AND IF THERE IS ONE THEN WE 
                                else if(true)
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
                        break;
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

            //return Json(new { redirectUrl = Url.Action("Index") });
        }

        private async Task ProcessDailyBellSchedule(TimeOnly timeOnly, StudentScheduleInfoModel studentSchedule, int studID)
        {
            var dailyBellSchedules = await _context.dailyBellScheduleModels.ToListAsync();

            //Identify the active bell
            var activeBell = dailyBellSchedules.FirstOrDefault(schedule => timeOnly.CompareTo(schedule.StartTime) >= 0 && timeOnly.CompareTo(schedule.EndTime) <= 0);

            if (activeBell != null)
            {
                
            }
        }
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