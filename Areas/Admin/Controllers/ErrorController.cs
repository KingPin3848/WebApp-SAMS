using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAMS.Controllers;
using SAMS.Data;
using SAMS.Models;

namespace SAMS.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class ErrorController : Controller
    {
        private readonly ApplicationDbContext context;
        public ErrorController(ApplicationDbContext Context)
        {
            context = Context;
        }

        [Authorize(Roles = "Admin")]
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
            ReportModel report = new()
            {
                TypeOfReport = ReportModel.ErrorType.ProcessingError,
                Number = number,
                DeveloperReference = reference,
                Description = description,
                UserId = user.SchoolId,
                StatusOfReport = ReportModel.Status.SubmittedToAppropriatePersonnel,
                Severity = ReportModel.SeverityLevel.Medium
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
            ReportModel report = new()
            {
                TypeOfReport = ReportModel.ErrorType.ProcessingError,
                Number = number,
                DeveloperReference = reference,
                Description = description,
                UserId = "Anonymous. Possible Data Breach.",
                StatusOfReport = ReportModel.Status.SubmittedToAppropriatePersonnel,
                Severity = ReportModel.SeverityLevel.High
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
