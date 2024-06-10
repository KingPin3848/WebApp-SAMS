using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SAMS.Models;

namespace SAMS.Controllers
{
    public class RoleController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager) : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;
        private readonly UserManager<ApplicationUser> _userManager = userManager;

        public async Task<IActionResult> CreateRole()
        {
            var developer = await _roleManager.CreateAsync(new IdentityRole("Developer"));
            //var hsschooladmin = await _roleManager.CreateAsync(new IdentityRole("HS School Admin"));
            //var synnlabadmin = await _roleManager.CreateAsync(new IdentityRole("Synnovation Lab Admin"));
            //var teacher = await _roleManager.CreateAsync(new IdentityRole("Teacher"));
            //var attoff = await _roleManager.CreateAsync(new IdentityRole("Attendance Office Member"));
            //var nurse = await _roleManager.CreateAsync(new IdentityRole("Nurse"));
            //var lawenf = await _roleManager.CreateAsync(new IdentityRole("Law Enforcement"));
            //var scanner = await _roleManager.CreateAsync(new IdentityRole("Synnovation Lab QR Code Scanner Management"));
            //var sub = await _roleManager.CreateAsync(new IdentityRole("Substitute Teacher"));
            //var student = await _roleManager.CreateAsync(new IdentityRole("Student"));
            //var districtadmin = await _roleManager.CreateAsync(new IdentityRole("District Admin"));

            if (developer.Succeeded)
            {
                Console.WriteLine("The Role Creation worked.");
                return RedirectToAction("Index");
            }
            else
            {
                Console.WriteLine("The role creation did not work.");
                Console.WriteLine("School Admin: " + developer.Succeeded);
            }
            return View();
        }

        public async Task<IActionResult> UserToRole()
        {
            var user = await _userManager.FindByNameAsync("vyasshivam2006@gmail.com");
            if (user != null)
            {
                var result = await _userManager.AddToRoleAsync(user, "Developer");
                if (result.Succeeded)
                {
                    Console.WriteLine("It worked.");
                    return RedirectToAction("Index");

                } else
                {
                    Console.WriteLine("It did not work.");
                }
            }
            return View();
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
