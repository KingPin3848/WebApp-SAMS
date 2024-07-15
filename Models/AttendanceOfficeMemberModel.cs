using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace SAMS.Models
{
    public class AttendanceOfficeMemberModel
    {
        [Key]
        [Required]
        [Display(Name = ("AO Member ID"))]
        [ProtectedPersonalData]
        public required string AoMemberID { get; set; }


        [Display(Name = ("First Name"))]
        [Required]
        [ProtectedPersonalData]
        public required string AoMemberFirstNameMod { get; set; }


        [Display(Name = ("Middle Name"))]
        [ProtectedPersonalData]
        public string? AoMemberMiddleNameMod { get; set; }


        [Display(Name = ("Last Name"))]
        [Required]
        [ProtectedPersonalData]
        public required string AoMemberLastNameMod { get; set; }


        [Display(Name = ("Preferred Name"))]
        [ProtectedPersonalData]
        public string? AoMemberPreferredNameMod { get; set; }


        [Display(Name = ("Email Address"))]
        [EmailAddress]
        [Required]
        [ProtectedPersonalData]
        public required string AoMemberEmailMod { get; set; }


        [Display(Name = ("Phone Ext."))]
        [ProtectedPersonalData]
        public string? AoMemberPhoneMod { get; set; }




        //Navigation properties
#pragma warning disable CA2227 // Collection properties should be read only
        public ICollection<HallPassInfoModel>? AssignedHallPasses { get; set; } = null!;
        public ICollection<HallPassInfoModel>? AddressedHallPasses { get; set; } = null!;
        public ICollection<PassRequestInfoModel>? RequestAssignedHallPasses { get; set; } = null!;
        public ICollection<PassRequestInfoModel>? RequestAddressedHallPasses { get; set; } = null!;
#pragma warning restore CA2227 // Collection properties should be read only
    }
}
