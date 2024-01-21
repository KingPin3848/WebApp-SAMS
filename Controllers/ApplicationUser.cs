using Microsoft.AspNetCore.Identity;

namespace SAMS.Controllers
{
    public class ApplicationUser : IdentityUser
    {
        public string? SchoolId { get; set; } = null!;
        public string? ActivationCode {  get; set; } = null!;
        public Boolean? UserExperienceEnabled { get; set; }
    }
}
