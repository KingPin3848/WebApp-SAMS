using System.ComponentModel.DataAnnotations;

namespace SAMS.Models
{
    public class PassRequestInfoModel
    {
        [Key]
        [Display(Name = ("Pass Request ID"))]
        public int PassRequestId { get; set; } = 0!;
        [Display(Name = ("Student ID"))]
        public int StudentID { get; set; } = 0!;
        [Display(Name = ("Student Name"))]
        public string StudentNameMod { get; set; } = null!;
        [Display(Name = ("Requested By"))]
        public string HallPassAssignedBy { get; set; } = null!;
        [Display(Name = ("Requested To"))]
        public string HallPassAddressedBy { get; set; } = null!;
        [Display(Name = ("Start Time"))]
        public DateTime StartDateTime { get; set; }
        [Display(Name = ("End Time"))]
        public DateTime EndDateTime { get; set; }
        [Display(Name = ("Duration"))]
        public TimeSpan Duration { get { return Duration; } set { Duration = EndDateTime - StartDateTime; } }
        [Display(Name = ("Bell"))]
        public int BellNumber { get; set; } = 0!;
        [Display(Name = ("Start Location"))]
        public string StartLocation { get; set; } = null!;
        [Display(Name = ("End Location"))]
        public string EndLocation { get; set; } = null!;
        [Display(Name = ("Request Status"))]
        public string RequestStatus { get; set; } = null!;

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
