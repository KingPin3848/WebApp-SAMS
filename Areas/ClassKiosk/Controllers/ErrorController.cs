using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAMS.Areas.Shared.Controllers;
using SAMS.Data;
using SAMS.Models;

namespace SAMS.Areas.ClassKiosk.Controllers
{
    [Authorize(Roles = "Synnovation Lab QR Code Scanner Management")]
    [Area("ClassKiosk")]
    public class ErrorController(ApplicationDbContext context) : ErrorControllerBase(context)
    {
        [Authorize(Roles = "Synnovation Lab QR Code Scanner Management")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AutomatedError(int number, string description, string reference, ApplicationUser? user)
        {
            return await AutomatedErrorCore(number, description, reference, user).ConfigureAwait(true);
        }
    }
}
