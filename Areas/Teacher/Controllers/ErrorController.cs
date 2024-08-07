using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAMS.Controllers;
using SAMS.Data;
using SAMS.Models;

namespace SAMS.Areas.Teacher.Controllers
{
    [Authorize(Roles = "Teacher")]
    [Area("Teacher")]
    public class ErrorController : Controller
    {
        private readonly ApplicationDbContext context;
        public ErrorController(ApplicationDbContext Context)
        {
            context = Context;
        }

        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> AutomatedError(int number, string description, string reference, ApplicationUser? user)
        {
            if (user == null)
            {
                var reported = await UnauthorizedReportError(number, description, reference).ConfigureAwait(true);
                ViewData["Error"] = reported;
                ViewData["ErrorNumber"] = number;
                ViewData["ErrorDescription"] = description;
                ViewData["DeveloperReference"] = reference;
            }
            if (user.Id is null || string.IsNullOrEmpty(user.Id))
            {
                var reported = await UnauthorizedReportError(number, description, reference).ConfigureAwait(true);
                ViewData["Error"] = reported;
                ViewData["ErrorNumber"] = number;
                ViewData["ErrorDescription"] = description;
                ViewData["DeveloperReference"] = reference;
            }
            else
            {
                var reported = await Report(number, description, reference, user).ConfigureAwait(true);
                ViewData["Error"] = reported;
                ViewData["ErrorNumber"] = number;
                ViewData["ErrorDescription"] = description;
                ViewData["DeveloperReference"] = reference;
            }

            return View("Error");
        }

        private async Task<bool> Report(int number, string description, string reference, ApplicationUser user)
        {
            ProcessingErrorReportModel report = new()
            {
                Number = number,
                Description = description,
                DeveloperReference = reference,
                UserId = user.SchoolId
            };
            context.ErrorProcessingModel.Add(report);
            var result = await context.SaveChangesAsync().ConfigureAwait(true);
            if (result > 0)
            {
                //Changes saved successfully
                return true;
            }
            else
            {
                return false;
            }
        }

        private async Task<bool> UnauthorizedReportError(int number, string description, string reference)
        {
            ProcessingErrorReportModel report = new()
            {
                Number = number,
                Description = description,
                DeveloperReference = reference,
                UserId = "Anonymous. Possible Data Breach."
            };
            context.ErrorProcessingModel.Add(report);
            var result = await context.SaveChangesAsync().ConfigureAwait(true);
            if (result > 0)
            {
                //Changes saved successfully
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}