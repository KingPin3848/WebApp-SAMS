using Microsoft.AspNetCore.Identity;
using SAMS.Models;
using System.ComponentModel.DataAnnotations;

namespace SAMS.Controllers
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [Display(Name = "School Id")]
        public string? SchoolId { get; set; } = null!;
        [Required]
        [StringLength(32, ErrorMessage = "The Unique Code must be at least {2} and at max {1} characters long.", MinimumLength = 32)]
        [Display(Name = "Unique Code")]
        public string? ActivationCode {  get; set; } = null!;
        [Display(Name = "Enable User Experience")]
        public Boolean? UserExperienceEnabled { get; set; }
        [Required]
        [Display(Name = "Role(s)")]
        public IList<string>? Role { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "School Issued Email Address")]
        public override string? Email { get => base.Email; set => base.Email = value; }
    }
}
