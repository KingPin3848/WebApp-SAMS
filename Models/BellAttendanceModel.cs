using System.ComponentModel.DataAnnotations;

namespace SAMS.Models
{
    public class BellAttendanceModel
    {
        [Key]
        [Display(Name = ("Att. ID"))]
        public int BellAttendanceId { get; set; } = 0!;
        [Display(Name = ("Student ID"))]
        public int StudentId { get; set; } = 0!;
        [Display(Name = ("Date/Time"))]
        public DateTime DateTime { get; set; }
        [Display(Name = ("Status"))]
        public string Status { get; set; } = null!;
        [Display(Name = ("Reason For Absence"))]
        public string ReasonForAbsence { get; set; } = null!;
        [Display(Name = ("Bell Number"))]
        public string BellNumId { get; set; } = null!;
        [Display(Name = ("Course Id"))]
        public int CourseId { get; set; } = 0!;
        [Display(Name = ("Bell Schedule for the day"))]
        public string ChosenBellSchedule { get; set; } = null!;

        //Navigation properties
        public ActiveCourseInfoModel ActiveCourses { get; set; } = default!;
        public StudentInfoModel StudentInfo { get; set; } = default!;
    }
}
