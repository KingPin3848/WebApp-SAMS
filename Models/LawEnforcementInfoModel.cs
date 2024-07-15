using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace SAMS.Models
{
    public class LawEnforcementInfoModel
    {
        [Key]
        [Display(Name = ("Law Enforcement ID"))]
        [Required]
        [ProtectedPersonalData]
        public required string LawenfID { get; set; }


        [Display(Name = ("First Name"))]
        [Required]
        [ProtectedPersonalData]
        public required string LaweFirstNameMod { get; set; }


        [Display(Name = ("Middle Name"))]
        [ProtectedPersonalData]
        public string? LaweMiddleNameMod { get; set; }


        [Display(Name = ("Last Name"))]
        [Required]
        [ProtectedPersonalData]
        public required string LaweLastNameMod { get; set; }


        [Display(Name = ("Preferred Name"))]
        [ProtectedPersonalData]
        public string? LawePreferredNameMod { get; set; }


        [Display(Name = ("Email Address"))]
        [EmailAddress]
        [Required]
        [ProtectedPersonalData]
        public required string LaweEmailMod { get; set; }


        [Display(Name = ("Phone (Personal/Ext.)"))]
        public string? LawePhoneMod { get; set; }




        //Navigation properties
#pragma warning disable CA2227 // Collection properties should be read only
        public ICollection<HallPassInfoModel>? AssignedHallPasses { get; set; } = null!;
        public ICollection<HallPassInfoModel>? AddressedHallPasses { get; set; } = null!;
        public ICollection<PassRequestInfoModel>? RequestAssignedHallPasses { get; set; } = null!;
        public ICollection<PassRequestInfoModel>? RequestAddressedHallPasses { get; set; } = null!;
#pragma warning restore CA2227 // Collection properties should be read only
    }
}
