using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAMS.Areas.Shared.Controllers;
using SAMS.Data;
using SAMS.Models;

namespace SAMS.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class ErrorController(ApplicationDbContext context) : ErrorControllerBase(context)
    {
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        internal async Task<IActionResult> AutomatedError(int number, string description, string reference, ApplicationUser? user)
        {
            return await AutomatedErrorCore(number, description, reference, user).ConfigureAwait(true);
        }
    }
}
