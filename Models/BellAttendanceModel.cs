using System.ComponentModel.DataAnnotations;

namespace SAMS.Models
{
    public class BellAttendanceModel
    {
        [Display(Name = ("Att. ID"))]
        public int BellAttendanceId { get; set; } = 0!;
        [Key]
        [Display(Name = ("Student ID"))]
        public int StudentId { get; set; } = 0!;
        [Display(Name = ("Date/Time"))]
        public DateTime DateTime { get; set; }
        [Display(Name = ("Status"))]
        public string Status { get; set; } = null!;
        [Display(Name = ("Reason For Absence"))]
        public string ReasonForAbsence { get; set; } = null!;
        [Display(Name = ("Bell Number"))]
        public int BellNumId { get; set; } = 0!;
        [Display(Name = ("Course Name"))]
        public int ScheduleId { get; set; } = 0!;

        //Navigation properties
        public StudentScheduleInfoModel? StudentScheduleInfoModel { get; set; }
        public StudentInfoModel? StudentInfo { get; set; }
    }
}
