using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SAMS.Models;

namespace SAMS.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        public RoleController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> CreateRole()
        {
            var developer = await _roleManager.CreateAsync(new IdentityRole("Developer"));
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
