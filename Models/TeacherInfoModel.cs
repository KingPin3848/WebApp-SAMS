using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace SAMS.Models
{
    public class TeacherInfoModel
    {
        [Key]
        [Display(Name = ("Teacher ID"))]
        [Required]
        [ProtectedPersonalData]
        public required string TeacherID { get; set; }


        [Display(Name = ("First Name"))]
        [Required]
        [ProtectedPersonalData]
        public required string TeacherFirstNameMod { get; set; }


        [Display(Name = ("Middle Name"))]
        [ProtectedPersonalData]
        public string? TeacherMiddleNameMod { get; set; }


        [Display(Name = ("Last Name"))]
        [Required]
        [ProtectedPersonalData]
        public required string TeacherLastNameMod { get; set; }


        [Display(Name = ("Preferred Name"))]
        [ProtectedPersonalData]
        public string? TeacherPreferredNameMod { get; set; }


        [Display(Name = ("Email Address"))]
        [EmailAddress]
        [Required]
        [ProtectedPersonalData]
        public required string TeacherEmailMod { get; set; }


        [Display(Name = ("Phone Ext."))]
        [ProtectedPersonalData]
        public string? TeacherPhoneMod { get; set; }


        [Display(Name = ("Teaches all 5 days?"))]
        [PersonalData]
        public Boolean? Teaches5Days { get; set; }

        [PersonalData]
        [Display(Name = "Assigned Room")]
        public int? RoomAssignedId { get; set; }



        //Navigation properties
#pragma warning disable CA2227 // Collection properties should be read only
        public ICollection<ActiveCourseInfoModel>? ActiveCourses { get; set; } = null!;
        public RoomLocationInfoModel? Room { get; set; } = null!;
        public ICollection<HallPassInfoModel>? AssignedHallPasses { get; set; } = null!;
        public ICollection<HallPassInfoModel>? AddressedHallPasses { get; set; } = null!;
        public ICollection<PassRequestInfoModel>? RequestAssignedHallPasses { get; set; } = null!;
        public ICollection<PassRequestInfoModel>? RequestAddressedHallPasses { get; set; } = null!;
#pragma warning restore CA2227 // Collection properties should be read only
    }
}
