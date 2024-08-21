using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SAMS.Data;
using System.Security.Claims;
using SAMS.Interfaces;
using System.Text.RegularExpressions;

namespace SAMS.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ILogger<DashboardController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public DashboardController(ILogger<DashboardController> logger, ApplicationDbContext context, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
        }


        [HttpGet]
        // GET: DashboardController
        public async Task<IActionResult> Dashboard()
        {

            var loggedinuserid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (loggedinuserid is null)
            {
                return RedirectToAction("AutomatedError", "Error", new { number = 01, description = "Are you logged in?", reference = "Event ID = 1, Identifier = Acc72Det" });
            }

            var loggedinuser = await _userManager.FindByIdAsync(loggedinuserid).ConfigureAwait(true);
            if (loggedinuser is null)
            {
                return RedirectToAction("AutomatedError", "Error", new { number = 03, description = "Couldn't find you.", reference = "Event ID = 2, Identifier = Something" });
            }

            var model = new QRCodeModel()
            {
                Code = []
            };
            var userroles = await _userManager.GetRolesAsync(loggedinuser).ConfigureAwait(true);
            foreach (var role in userroles)
            {
                switch (role)
                {
                    case "Teacher":
                        {
                            //Global Variables for "Teacher" case.
                            var schoolid = loggedinuser.SchoolId;

                            var courses = _context.ActiveCourseInfoModels.Where(a => a.CourseTeacherID == schoolid).ToList();
                            List<string> bells = [];

                            for (int i = 0; i < courses.Count; i++)
                            {
                                bells.Add(courses[i].CourseBellNumber);
                            }
                            //Remove any duplicates from the bells list
                            bells = new HashSet<string>(bells).ToList();
                            ViewBag.Bells = bells;

                            var chosenbellschedule = _context.ChosenBellSchedModels.First().Name;
                            List<IBellSchedule>? chosenBellSched;

                            switch (chosenbellschedule)
                            {
                                case "Daily Bell Schedule":
                                    chosenBellSched = [.. _context.DailyBellScheduleModels.Where(a => a.BellName.Contains("Bell")).OrderBy(a => a.StartTime).Cast<IBellSchedule>()];
                                    chosenBellSched.Remove(chosenBellSched.First());
                                    chosenBellSched.Remove(chosenBellSched.Last());
                                    break;
                                case "Extended Aves Bell Schedule":
                                    chosenBellSched = [.. _context.ExtendedAvesModels.Where(a => a.BellName.Contains("Bell")).OrderBy(a => a.StartTime).Cast<IBellSchedule>()];
                                    break;
                                case "Pep Rally Bell Schedule":
                                    chosenBellSched = [.. _context.PepRallyBellScheduleModels.Where(a => a.BellName.Contains("Bell")).OrderBy(a => a.StartTime).Cast<IBellSchedule>()];
                                    break;
                                case "2 Hour Delay Bell Schedule":
                                    chosenBellSched = [.. _context.TwoHrDelayBellScheduleModels.Where(a => a.BellName.Contains("Bell")).OrderBy(a => a.StartTime).Cast<IBellSchedule>()];
                                    break;
                                case "Custom Bell Schedule":
                                    chosenBellSched = [.. _context.CustomSchedules.Where(a => a.BellName.Contains("Bell")).OrderBy(a => a.StartTime).Cast<IBellSchedule>()];
                                    break;
                                default:
                                    chosenBellSched = null;
                                    _logger.LogInformation("A schedule was chosen other than the ones offered. Possible breach try.");
                                    break;
                            }

                            if(chosenBellSched == null)
                            {
                                return RedirectToAction("AutomatedError", "Error", new { number = 01, description = "being null isnt so bad, you just cant be referenced", reference = "Event ID = 1, Identifier = Dash96CTDash" });
                            }

                            List<List<string>> schedule = [];
                            foreach (var item in chosenBellSched)
                            {
                                List<string> temp = [];
                                temp.Add(item.BellName);
                                temp.Add(item.StartTime.ToString());
                                temp.Add(item.EndTime.ToString());
                                schedule.Add(temp);
                            }

                            ViewBag.Schedule = schedule;



                            //The following code gets the course schedule for the student to display on the side of teacher partial. 


                            /* Code Snippet Summary:
                             * The following code uses the Teacher ID and the Current Bells that teacher has to retrieve the Room ID.
                             * This room id is used generate the QR code displayed in the teacher dashboard.
                             */

                            /* Comment 01:
                             * It checks if the currentbell is anything else than integers between 1 and 7.
                             */
                            var currentBell = GetCurrentBell();
                            if (currentBell < 1 || currentBell > 7)
                            {
                                //do nothing
                            }
                            else
                            {
#pragma warning disable CA1305
                                var activeBellBasedCourses = _context.ActiveCourseInfoModels.Where(a => a.CourseTeacherID == schoolid).Where(a => a.CourseBellNumber.Contains(currentBell.ToString())).ToList();
#pragma warning restore CA1305
                                List<int> roomids = [];
                                foreach (var course in activeBellBasedCourses)
                                {
                                    roomids.Add(course.CourseRoomID);
                                }
                                var roomidsUnique = roomids.Distinct().ToList();
                                foreach (var roomid in roomidsUnique)
                                {
                                    if (roomid is 0 || roomid is -1)
                                    {
                                        //Do Nothing
                                    }
                                    else
                                    {
                                        model.Code.Add(_context.RoomQRCodeModels.Where(a => a.RoomId == roomid).First().Code);
                                    }
                                }
                            }
                            break;
                        }
                }
            }

            return View(model);
        }

        private int GetCurrentBell()
        {
            var chosenbellschedule = _context.ChosenBellSchedModels.First().Name;
            List<IBellSchedule>? chosenBellSched;

            switch (chosenbellschedule)
            {
                case "Daily Bell Schedule":
                    chosenBellSched = [.. _context.DailyBellScheduleModels.Where(a => a.BellName.Contains("Bell")).OrderBy(a => a.StartTime).Cast<IBellSchedule>()];
                    break;
                case "Extended Aves Bell Schedule":
                    chosenBellSched = [.. _context.ExtendedAvesModels.Where(a => a.BellName.Contains("Bell")).OrderBy(a => a.StartTime).Cast<IBellSchedule>()];
                    break;
                case "Pep Rally Bell Schedule":
                    chosenBellSched = [.. _context.PepRallyBellScheduleModels.Where(a => a.BellName.Contains("Bell")).OrderBy(a => a.StartTime).Cast<IBellSchedule>()];
                    break;
                case "2 Hour Delay Bell Schedule":
                    chosenBellSched = [.. _context.TwoHrDelayBellScheduleModels.Where(a => a.BellName.Contains("Bell")).OrderBy(a => a.StartTime).Cast<IBellSchedule>()];
                    break;
                case "Custom Bell Schedule":
                    chosenBellSched = [.. _context.CustomSchedules.Where(a => a.BellName.Contains("Bell")).OrderBy(a => a.StartTime).Cast<IBellSchedule>()];
                    break;
                default:
                    chosenBellSched = null;
                    _logger.LogInformation("A schedule was chosen other than the ones offered. Possible breach try.");
                    return -1;
            }

            if (chosenBellSched is null)
            {
                return -1;
            }

            var currentTime = DateTime.Now.TimeOfDay;
            foreach (var bell in chosenBellSched)
            {
                if (currentTime >= bell.StartTime && currentTime <= bell.EndTime)
                {
#pragma warning disable SYSLIB1045
                    Match match = Regex.Match(bell.BellName, @"Bell (\d+)");
#pragma warning restore SYSLIB1045
                    if (match.Success)
                    {
                        if (int.TryParse(match.Groups[1].Value, out int bellNumber))
                        {
                            return bellNumber;
                        }
                        else
                        {
                            return -1;
                        }
                    }
                    return -1;
                }
            }
            return -1;
        }

        //GET: ClassAttendance

        public IActionResult ClassAttendance(string id)
        {
            string passedId = id;

            var db = _context;
            return View();
            /*
             * 1. First get if it is semester 1 or semester 2 -> If semester 1, then use the semester 1 schedule model, else 
             * use semester 2 schedule model.
             * 2. Then get the courses (or course ids) that the teacher teachers for that specific bell.
             * 3. Then get each student's schedule with all the courseids in a list or something.
             * 4. Then compare the courses found from Step 2 to the courseid in the student's schedule.
             * 5. If there is a match in step 4, then add the student id to a list. If not match, move to the next one that 
             * has a match and add it to the list until the matching is finished.
             * 6. The added students would have their current daily and bell-to-bell attendance shown to the right which will 
             * be clickable. Once you click on either of them, several options would come to mark their presence or absence 
             * based on the types provided to us. Once the staff member clicks on the button (daily or b2b), it should not redirect
             * anywhere. However, once clicked it should show multiple attendance presence/absence options for the teacher to 
             * click & override. And once the teacher clicks a new option from the dropdown of presence/absence options, the
             * program should automatically show a "Save Changes" button that will (not asynchronously) update the attendance and
             * save changes to the database.
             * 7. To get the daily attendance, search in the database for the current date and their student id and if they have
             * CCP or similar course that doesn't require daily attendance for the on-spot moment/time then the field will be
             * disabled and cannot be edited.
             * 8. To get the bell-to-bell attendance, search in the database for the current date, the student id, and 
             * current bell and get the status for that student (unknown, present, absent, tardy, etc.)
             */
        }
    }

    public class QRCodeModel
    {
        public required List<string> Code { get; set; }
    }
}