using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
            DateTime.TryParse(camResultTimestamp, out DateTime parsedTimestamp);

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

            //var roomCodes = _context.roomQRCodeModels.Select(a => a.Code).ToList();
            //if (roomCodes.Count == 0)
            //{
            //    return Json(new { dangertext = "Room Codes table is completely empty. Please check in with the developers to resolve this issue asap." });
            //}

            return Json(new { redirectUrl = Url.Action("Index") });
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