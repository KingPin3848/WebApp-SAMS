using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SAMS.Data;
using System.Collections;
using System.Reflection.Metadata;
using System.Security.Claims;

namespace SAMS.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = "Admin")]
    public class ReportController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReportController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        [RequireHttps]
        public async Task<IActionResult> Index()
        {
            var loggedinuserid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (loggedinuserid is null)
            {
                return RedirectToAction("AutomatedError", "Error", new { number = 01, description = "Are you logged in?", reference = "Event ID = 1, Identifier = Acc72Det" });
            }

            var loggedinuser = await _userManager.FindByIdAsync(loggedinuserid).ConfigureAwait(true);
            if (loggedinuser is null)
            {
                return RedirectToAction("AutomatedError", "Error", new { number = 01, description = "Are you an alien? We couldn't authenticate you.", reference = "Event ID = 1, Identifier = Acc78Det" });
            }

            ViewData["Self-Created"] = _context.ErrorProcessingModel.Where(a => a.UserId == loggedinuser.UserName).ToList();
            ViewData["All Reports"] = _context.ErrorProcessingModel.ToListAsync();
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> MyCases()
        {
            var loggedinuserid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (loggedinuserid is null)
            {
                return RedirectToAction("AutomatedError", "Error", new { number = 01, description = "Are you logged in?", reference = "Event ID = 1, Identifier = Acc72Det" });
            }

            var loggedinuser = await _userManager.FindByIdAsync(loggedinuserid).ConfigureAwait(true);
            if (loggedinuser is null)
            {
                return RedirectToAction("AutomatedError", "Error", new { number = 01, description = "Are you an alien? We couldn't authenticate you.", reference = "Event ID = 1, Identifier = Acc78Det" });
            }

            return Json(_context.ErrorProcessingModel.Where(a => a.UserId == loggedinuser.UserName).ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> AllCases()
        {
            var loggedinuserid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (loggedinuserid is null)
            {
                return RedirectToAction("AutomatedError", "Error", new { number = 01, description = "Are you logged in?", reference = "Event ID = 1, Identifier = Acc72Det" });
            }

            var loggedinuser = await _userManager.FindByIdAsync(loggedinuserid).ConfigureAwait(true);
            if (loggedinuser is null)
            {
                return RedirectToAction("AutomatedError", "Error", new { number = 01, description = "Are you an alien? We couldn't authenticate you.", reference = "Event ID = 1, Identifier = Acc78Det" });
            }

            return Json(_context.ErrorProcessingModel.ToListAsync());
        }

        [HttpGet]
        [RequireHttps]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reportModel = await _context.ErrorProcessingModel
                .FirstOrDefaultAsync(m => m.ReportId == id).ConfigureAwait(true);
            if (reportModel == null)
            {
                return NotFound();
            }

            return View(reportModel);
        }

        [HttpGet]
        [RequireHttps]
        public async Task<IActionResult> Create()
        {
            var loggedinuserid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (loggedinuserid is null)
            {
                return RedirectToAction("AutomatedError", "Error", new { number = 01, description = "Are you logged in?", reference = "Event ID = 1, Identifier = Acc72Det" });
            }

            var loggedinuser = await _userManager.FindByIdAsync(loggedinuserid).ConfigureAwait(true);
            if (loggedinuser is null)
            {
                return RedirectToAction("AutomatedError", "Error", new { number = 01, description = "Are you an alien? We couldn't authenticate you.", reference = "Event ID = 1, Identifier = Acc78Det" });
            }

            ViewData["User"] = loggedinuser.UserName;
            ViewData["Types"] = new SelectList(_context.ErrorProcessingModel, "TypeOfReport", "TypeOfReport");
            return View();
        }
    }
}
