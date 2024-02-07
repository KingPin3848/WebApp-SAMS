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
        public StudentInfoModel? Student { get; set; } = null!;
        public AdminInfoModel? AssignedByAdmin { get; set; } = null!;
        public AdminInfoModel? AddressedByAdmin { get; set; } = null!;
        public NurseInfoModel? AssignedByNurse { get; set; } = null!;
        public NurseInfoModel? AddressedByNurse { get; set; } = null!;
        public TeacherInfoModel? AssignedByTeacher { get; set; } = null!;
        public TeacherInfoModel? AddressedByTeacher { get; set; } = null!;
        public LawEnforcementInfoModel? AssignedByLawEnf { get; set; } = null!;
        public LawEnforcementInfoModel? AddressedByLawEnf { get; set; } = null!;
        public AttendanceOfficeMemberModel? AssignedByAttendanceOfficeMember { get; set; } = null!;
        public AttendanceOfficeMemberModel? AddressedByAttendanceOfficeMember { get; set; } = null!;
        public CounselorModel? AssignedByCounselor { get; set; } = null!;
        public CounselorModel? AddressedByCounselor { get; set; } = null!;


    }
}