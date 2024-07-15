using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace SAMS.Models
{
    public class EASuportInfoModel
    {
        [Key]
        [Display(Name = ("EA ID"))]
        [Required]
        [ProtectedPersonalData]
        public required string EaID { get; set; }


        [Display(Name = ("First Name"))]
        [Required]
        [ProtectedPersonalData]
        public required string EaFirstNameMod { get; set; }


        [Display(Name = ("Middle Name"))]
        [ProtectedPersonalData]
        public string? EaMiddleNameMod { get; set; }


        [Display(Name = ("Last Name"))]
        [Required]
        [ProtectedPersonalData]
        public required string EaLastNameMod { get; set; }


        [Display(Name = ("Preferred Name"))]
        [ProtectedPersonalData]
        public string? EaPreferredNameMod { get; set; }


        [Display(Name = ("Email Address"))]
        [EmailAddress]
        public required string EaEmailMod { get; set; }


        [Display(Name = ("Phone"))]
        [ProtectedPersonalData]
        public string? EaPhoneMod { get; set; }


        [Display(Name = ("Student Managed"))]
        public int? EaStudentManaged { get; set; } = 0!;



        //Navigation properties
#pragma warning disable CA2227 // Collection properties should be read only
        public ICollection<StudentInfoModel>? Students { get; set; } = null!;
#pragma warning restore CA2227 // Collection properties should be read only
    }
}
