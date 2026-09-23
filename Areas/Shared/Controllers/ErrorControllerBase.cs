using Microsoft.AspNetCore.Mvc;
using SAMS.Data;
using SAMS.Models;

namespace SAMS.Areas.Shared.Controllers
{
	public abstract class ErrorControllerBase(ApplicationDbContext context) : Controller
	{
		private readonly ApplicationDbContext _context = context;

		protected async Task<IActionResult> AutomatedErrorCore(int number, string description, string reference, ApplicationUser? user)
		{
			var sanitizedDescription = string.IsNullOrWhiteSpace(description)
				? "No description provided."
				: description.Trim();

			var sanitizedReference = string.IsNullOrWhiteSpace(reference)
				? "UNSPECIFIED_REFERENCE"
				: reference.Trim();

			var normalizedErrorCode = BuildErrorCode(number, sanitizedReference);

			var reported = await SaveReportAsync(number, normalizedErrorCode, sanitizedDescription, user).ConfigureAwait(true);

			ViewData["Error"] = reported;
			ViewData["ErrorNumber"] = number;
			ViewData["ErrorCode"] = normalizedErrorCode;
			ViewData["ErrorDescription"] = sanitizedDescription;
			ViewData["DeveloperReference"] = sanitizedReference;

			return View("Error");
		}

		private async Task<bool> SaveReportAsync(int number, string reference, string description, ApplicationUser? user)
		{
			var isAuthorizedUser = user is not null && !string.IsNullOrWhiteSpace(user.Id);

			var report = new ReportModel
			{
				TypeOfReport = ReportModel.ErrorType.ProcessingError,
				Number = number,
				DeveloperReference = reference,
				Description = description,
				UserId = isAuthorizedUser ? (user!.SchoolId ?? "UnknownUser") : "Anonymous. Possible Data Breach.",
				StatusOfReport = ReportModel.Status.SubmittedToAppropriatePersonnel,
				Severity = isAuthorizedUser ? ReportModel.SeverityLevel.Medium : ReportModel.SeverityLevel.High
			};

			_context.ErrorProcessingModel.Add(report);
			var result = await _context.SaveChangesAsync().ConfigureAwait(true);
			return result > 0;
		}

		private static string BuildErrorCode(int number, string reference)
		{
			var referenceToken = new string(reference
				.Where(char.IsLetterOrDigit)
				.Take(12)
				.ToArray())
				.ToUpperInvariant();

			if (string.IsNullOrWhiteSpace(referenceToken))
			{
				referenceToken = "GENERAL";
			}

			return $"SAMS-ERR-{number:D4}-{referenceToken}";
		}
	}
}
