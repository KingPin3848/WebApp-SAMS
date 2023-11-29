using System.ComponentModel.DataAnnotations;

namespace SAMS.Models
{
    public class AttendanceOfficeMemberModel
    {
        [Key]
        public string aoMemberID { get; set; } = null!;
        public string aoMemberFirstNameMod { get; set; } = null!;
        public string aoMemberMiddleNameMod { get; set; } = null!;
        public string aoMemberLastNameMod { get; set; } = null!;
        public string aoMemberPreferredNameMod { get; set; } = null!;
        public string aoMemberEmailMod { get; set; } = null!;
        public string aoMemberPhoneMod { get; set; } = null!;
    }
}
