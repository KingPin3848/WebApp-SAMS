using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAMS.Models
{
    public class HallPassInfoModel
    {
        [Display(Name = ("Student ID"))]
        public int StudentID { get; set; } = 0!;
        [Key]
        [Display(Name = ("Hall Pass ID"))]
        public string HallPassID { get; set; } = null!;
        [Display(Name = ("Issued"))]
        public DateTime StartDateTime { get; set; }
        [Display(Name = ("Expiration"))]
        public DateTime EndDateTime { get; set; }
        [Display(Name = ("Duration"))]
        public TimeSpan Duration { get { return Duration; } set { Duration = EndDateTime - StartDateTime; } }
        [Display(Name = ("Bell"))]
        public int BellNumber { get; set; } = 0!;
        [Display(Name = ("Assigned By"))]
        public string HallPassAssignedByID { get; set; } = null!;
        [Display(Name = ("Addressed To"))]
        public string HallPassAddressedByID { get; set; } = null!;
        [Display(Name = ("Start Location"))]
        public string StartLocation { get; set; } = null!;
        [Display(Name = ("End Location"))]
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