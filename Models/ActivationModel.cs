using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SAMS.Models
{
    public class ActivationModel
    {
        [Required]
        [Display(Name = "Activation Code")]
        [Key]
        public string ActivationCode { get; set; } = string.Empty;
        [Display(Name = "Enable User Experience")]
        public bool EnableUserExperience { get; set; }
    }
}
