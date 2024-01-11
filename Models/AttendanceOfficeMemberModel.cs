using System.ComponentModel.DataAnnotations;

namespace SAMS.Models
{
    public class AttendanceOfficeMemberModel
    {
        [Key]
        [Display(Name = ("Member ID"))]
        public string AoMemberID { get; set; } = null!;
        [Display(Name = ("First Name"))]
        public string AoMemberFirstNameMod { get; set; } = null!;
        [Display(Name = ("Middle Name"))]
        public string AoMemberMiddleNameMod { get; set; } = null!;
        [Display(Name = ("Last Name"))]
        public string AoMemberLastNameMod { get; set; } = null!;
        [Display(Name = ("Preferred Name"))]
        public string AoMemberPreferredNameMod { get; set; } = null!;
        [Display(Name = ("Email Address"))]
        public string AoMemberEmailMod { get; set; } = null!;
        [Display(Name = ("Phone Ext."))]
        public string AoMemberPhoneMod { get; set; } = null!;

        //Navigation properties
        public ICollection<HallPassInfoModel>? AssignedHallPasses { get; set; }
        public ICollection<HallPassInfoModel>? AddressedHallPasses { get; set; }
        public ICollection<PassRequestInfoModel>? RequestAssignedHallPasses { get; set; }
        public ICollection<PassRequestInfoModel>? RequestAddressedHallPasses { get; set; }
    }
}
