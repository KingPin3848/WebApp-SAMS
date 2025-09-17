using Microsoft.AspNetCore.Identity;
using SAMS.Models;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;

namespace SAMS
{
    internal class ApplicationUser : IdentityUser
    {
        [Required]
        [Display(Name = "School Id")]
        [ProtectedPersonalData]
        public string SchoolId { get; set; } = null!;

        [Required]
        [StringLength(32, ErrorMessage = "The Unique Code must be at least {2} and at max {1} characters long.", MinimumLength = 32)]
        [Display(Name = "Unique Code")]
        [ProtectedPersonalData]
        public string ActivationCode {  get; set; } = null!;
        
        [Display(Name = "Enable User Experience")]
        [ProtectedPersonalData]
        public bool? UserExperienceEnabled { get; set; }

        [Required]
        [Display(Name = "Role(s)")]
        [ProtectedPersonalData]
        public IList<string> Role { get; set; }
        
        [Required]
        [EmailAddress]
        [Display(Name = "School Email Address")]
        [ProtectedPersonalData]
        public override required string? Email { get; set; }

        [Required]
        [Range(0000, 9999)]
        [ProtectedPersonalData]
        public int StudentPin { get; private set; }

        [Required]
        [ProtectedPersonalData]
        public required string FullName { get; set; }
    }
}
