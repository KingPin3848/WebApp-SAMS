using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SAMS.Controllers;
using SAMS.Data;

namespace SAMS.Areas.Teacher.Controllers
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

        // GET: DashboardController
        public async Task<IActionResult> Dashboard()
        {

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var courses = _context.activeCourseInfoModels.Where(a => a.CourseTeacherID == user.SchoolId).ToList();
            List<string> bells = new List<string>();

            for (int i = 0; i < courses.Count; i++)
            {
                bells.Add(courses[i].CourseBellNumber);
            }

            //Remove any duplicates from the bells list
            bells = new HashSet<string>(bells).ToList();
            ViewBag.Bells = bells;

            return View();
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

}