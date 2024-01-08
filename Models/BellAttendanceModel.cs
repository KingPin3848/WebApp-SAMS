using System.ComponentModel.DataAnnotations;

namespace SAMS.Models
{
    public class BellAttendanceModel
    {
        [Key]
        public string BellAttendanceId { get; set; } = null!;
        public int StudentId { get; set; } = 0!;
        public int EnrollmentId { get; set; } = 0!;
        public DateTime Date { get; set; }
        public string Status { get; set; } = null!;
        public string ReasonForAbsence { get; set; } = null!;
        public int BellNumId { get; set; } = 0!;

        //Navigation properties
        public CourseEnrollmentModel? CourseEnrollment { get; set; }
        public StudentInfoModel? StudentInfo { get; set; }
    }
}
