using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace SAMS.Models
{
    public class SubTeacherModel
    {
        [Key]
        [Display(Name = ("Teacher ID"))]
        [Required]
        [ProtectedPersonalData]
        public required string SubID { get; set; }


        [Display(Name = ("First Name"))]
        [Required]
        [ProtectedPersonalData]
        public required string SubFirstNameMod { get; set; }


        [Display(Name = ("Middle Name"))]
        [ProtectedPersonalData]
        public string? SubMiddleNameMod { get; set; }


        [Display(Name = ("Last Name"))]
        [Required]
        [ProtectedPersonalData]
        public required string SubLastNameMod { get; set; }


        [Display(Name = ("Preferred Name"))]
        [ProtectedPersonalData]
        public string? SubPreferredNameMod { get; set; }


        [Display(Name = ("Email Address"))]
        [EmailAddress]
        [Required]
        [ProtectedPersonalData]
        public required string SubEmailMod { get; set; }


        [Display(Name = ("Phone Ext."))]
        [ProtectedPersonalData]
        public string? SubPhoneMod { get; set; }


//        //Navigation properties
//#pragma warning disable CA2227 // Collection properties should be read only
//        public ICollection<ActiveCourseInfoModel>? ActiveCourses { get; set; } = null!;
//        public RoomLocationInfoModel? Room { get; set; } = null!;
//        public ICollection<HallPassInfoModel>? AssignedHallPasses { get; set; } = null!;
//        public ICollection<HallPassInfoModel>? AddressedHallPasses { get; set; } = null!;
//        public ICollection<PassRequestInfoModel>? RequestAssignedHallPasses { get; set; } = null!;
//        public ICollection<PassRequestInfoModel>? RequestAddressedHallPasses { get; set; } = null!;
//#pragma warning restore CA2227 // Collection properties should be read only
    }
}
