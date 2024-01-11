using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAMS.Models
{
    public class HallPassInfoModel
    {
        public int StudentID { get; set; } = 0!;
        [Key]
        public string HallPassID { get; set; } = null!;
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public TimeSpan Duration { get { return Duration; } set { Duration = EndDateTime - StartDateTime; } }
        public int BellNumber { get; set; } = 0!;
        public string HallPassAssignedByID { get; set; } = null!;
        public string HallPassAddressedByID { get; set; } = null!;
        public string StartLocation { get; set; } = null!;
        public string EndLocation { get; set; } = null!;

        //Navigation properties
        public StudentInfoModel? Student {  get; set; }
        public AdminInfoModel? AssignedByAdmin { get; set; }
        public AdminInfoModel? AddressedByAdmin { get; set; }
        public NurseInfoModel? AssignedByNurse { get; set; }
        public NurseInfoModel? AddressedByNurse { get; set; }
        public TeacherInfoModel? AssignedByTeacher { get; set; }
        public TeacherInfoModel? AddressedByTeacher { get; set; }
        public LawEnforcementInfoModel? AssignedByLawEnf { get; set; }
        public LawEnforcementInfoModel? AddressedByLawEnf { get; set; }
        public AttendanceOfficeMemberModel? AssignedByAttendanceOfficeMember { get; set; }
        public AttendanceOfficeMemberModel? AddressedByAttendanceOfficeMember { get; set; }
        public CounselorModel? AssignedByCounselor { get; set; }
        public CounselorModel? AddressedByCounselor { get; set; }


    }
}