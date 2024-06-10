// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using SAMS.Controllers;

namespace SAMS.Areas.Identity.Pages.Account.Manage
{
    public class PersonalDataModel(
            UserManager<ApplicationUser> userManager,
            ILogger<PersonalDataModel> logger) : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
#pragma warning disable IDE0052 // Remove unread private members
        private readonly ILogger<PersonalDataModel> _logger = logger;
#pragma warning restore IDE0052 // Remove unread private members


        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            return Page();
        }
    }
}
