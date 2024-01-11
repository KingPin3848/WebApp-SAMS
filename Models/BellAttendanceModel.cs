using System.ComponentModel.DataAnnotations;

namespace SAMS.Models
{
    public class BellAttendanceModel
    {
        public string BellAttendanceId { get; set; } = null!;
        [Key]
        public int StudentId { get; set; } = 0!;
        public DateTime DateTime { get; set; }
        public string Status { get; set; } = null!;
        public string ReasonForAbsence { get; set; } = null!;
        public int BellNumId { get; set; } = 0!;
        public string CourseName { get; set; } = null!;

        //Navigation properties
        public StudentScheduleInfoModel? StudentScheduleInfoModel { get; set; }
        public StudentInfoModel? StudentInfo { get; set; }
    }
}
