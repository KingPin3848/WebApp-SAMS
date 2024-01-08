using System.ComponentModel.DataAnnotations;

namespace SAMS.Models
{
    public class DailyAttendanceModel
    {
        [Key]
        public int AttendanceId { get; set; }
        public int StudentId { get; set; } = 0!;
        public DateTime? AttendanceDate { get; set; }
        public string Status { get; set; } = null!;
        public string ReasonForAbsence { get; set; } = null!;

        //Navigation properties
        public StudentInfoModel? Student {  get; set; }
    }
}
