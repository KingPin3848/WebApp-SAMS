using System.ComponentModel.DataAnnotations;

namespace SAMS.Models
{
    public class DailyAttendanceModel
    {
        [Key]
        [Display(Name = ("Att. ID"))]
        public int AttendanceId { get; set; }
        [Display(Name = ("Student ID"))]
        public int StudentId { get; set; } = 0!;
        [Display(Name = ("Date"))]
        public DateOnly? AttendanceDate { get; set; }
        [Display(Name = ("Status"))]
        public string Status { get; set; } = null!;
        [Display(Name = ("Reason for Absence"))]
        public string ReasonForAbsence { get; set; } = null!;
        [Display(Name = ("Bell Schedule for the day"))]
        public string ChosenBellSchedule { get; set; } = null!;

        //Navigation properties
        public StudentInfoModel? Student { get; set; } = null!;
    }
}
