using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace SAMS.Models
{
    public class AdminInfoModel
    {
        [Key]
        [Required]
        [Display(Name = ("Admin ID"))]
        public required string AdminID { get; set; }


        [Display(Name = ("First Name"))]
        [Required]
        [ProtectedPersonalData]
        public required string AdminFirstNameMod { get; set; }


        [Display(Name = ("Middle Name"))]
        [ProtectedPersonalData]
        public string? AdminMiddleNameMod { get; set; }


        [Display(Name = ("Last Name"))]
        [Required]
        [ProtectedPersonalData]
        public required string AdminLastNameMod { get; set; }


        [Display(Name = ("Preferred Name"))]
        [ProtectedPersonalData]
        public string? AdminPreferredNameMod { get; set; }


        [Display(Name = ("Email Address"))]
        [EmailAddress]
        [Required]
        [ProtectedPersonalData]
        public required string AdminEmailMod { get; set; }


        [Display(Name = ("Phone Ext."))]
        [ProtectedPersonalData]
        public string? AdminPhoneMod { get; set; }


        [Display(Name = ("Label"))]
        [Required]
        public required string AdminLabelMod { get; set; } = null!;





        //Navigation properties
#pragma warning disable CA2227 // Collection properties should be read only
        public ICollection<HallPassInfoModel>? AssignedHallPasses { get; set; } = null!;
        public ICollection<HallPassInfoModel>? AddressedHallPasses { get; set; } = null!;
        public ICollection<PassRequestInfoModel>? RequestAssignedHallPasses { get; set; } = null!;
        public ICollection<PassRequestInfoModel>? RequestAddressedHallPasses { get; set; } = null!;
#pragma warning restore CA2227 // Collection properties should be read only
    }
}
