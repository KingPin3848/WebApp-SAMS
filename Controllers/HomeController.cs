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
            string camResultController = ScannedCode;
            string camResultTimestampController = ScannedCodeTimestamp;
            string theschoolidController = issuedSchoolId;

            // Parsing the timestamp since it's a string
            if (DateTime.TryParse(camResultTimestampController, out DateTime parsedTimestamp))
            {
                // Use the parsedTimestamp as needed
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var schoolIDdb = user.SchoolId;
            if (schoolIDdb == null)
            {
                return NotFound($"Unable to find the school id with the user with ID '{_userManager.GetUserId(User)}'.");
            }
            else
            {
                if (schoolIDdb == theschoolidController)
                {
                    int bellScheduleEnabled = 0;
                }
            }

            return RedirectToAction("Index");
        }
    }
}