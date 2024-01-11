using System.ComponentModel.DataAnnotations;

namespace SAMS.Models
{
    public class AttendanceOfficeMemberModel
    {
        [Key]
        public string AoMemberID { get; set; } = null!;
        public string AoMemberFirstNameMod { get; set; } = null!;
        public string AoMemberMiddleNameMod { get; set; } = null!;
        public string AoMemberLastNameMod { get; set; } = null!;
        public string AoMemberPreferredNameMod { get; set; } = null!;
        public string AoMemberEmailMod { get; set; } = null!;
        public string AoMemberPhoneMod { get; set; } = null!;

        //Navigation properties
        public ICollection<HallPassInfoModel>? AssignedHallPasses { get; set; }
        public ICollection<HallPassInfoModel>? AddressedHallPasses { get; set; }
        public ICollection<PassRequestInfoModel>? RequestAssignedHallPasses { get; set; }
        public ICollection<PassRequestInfoModel>? RequestAddressedHallPasses { get; set; }
    }
}
