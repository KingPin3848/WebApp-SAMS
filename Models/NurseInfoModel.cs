using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace SAMS.Models
{
    public class NurseInfoModel
    {
        [Key]
        [Display(Name = ("Nurse ID"))]
        [Required]
        [ProtectedPersonalData]
        public required string NurseID { get; set; }


        [Display(Name = ("First Name"))]
        [Required]
        [ProtectedPersonalData]
        public required string NurseFirstNameMod { get; set; }

        
        [Display(Name = ("Middle Name"))]
        [ProtectedPersonalData]
        public string? NurseMiddleNameMod { get; set; }


        [Display(Name = ("Last Name"))]
        [Required]
        [ProtectedPersonalData]
        public required string NurseLastNameMod { get; set; }


        [Display(Name = ("Preferred Name"))]
        [ProtectedPersonalData]
        public string? NursePreferredNameMod { get; set; }


        [Display(Name = ("Email Address"))]
        [EmailAddress]
        [Required]
        [ProtectedPersonalData]
        public required string NurseEmailMod { get; set; }


        [Display(Name = ("Phone Ext."))]
        [ProtectedPersonalData]
        public string? NursePhoneMod { get; set; }



        //Navigation properties
#pragma warning disable CA2227 // Collection properties should be read only
        public ICollection<HallPassInfoModel>? AssignedHallPasses { get; set; } = null!;
        public ICollection<HallPassInfoModel>? AddressedHallPasses { get; set; } = null!;
        public ICollection<PassRequestInfoModel>? RequestAssignedHallPasses { get; set; } = null!;
        public ICollection<PassRequestInfoModel>? RequestAddressedHallPasses { get; set; } = null!;
#pragma warning restore CA2227 // Collection properties should be read only
    }
}
