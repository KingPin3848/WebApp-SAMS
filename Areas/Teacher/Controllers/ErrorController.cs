using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAMS.Areas.Shared.Controllers;
using SAMS.Data;
using SAMS.Models;

namespace SAMS.Areas.Teacher.Controllers
{
    [Authorize(Roles = "Teacher")]
    [Area("Teacher")]
    public class ErrorController(ApplicationDbContext context) : ErrorControllerBase(context)
    {
        [Authorize(Roles = "Teacher")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AutomatedError(int number, string description, string reference, ApplicationUser? user)
        {
            return await AutomatedErrorCore(number, description, reference, user).ConfigureAwait(true);
        }
    }
}
