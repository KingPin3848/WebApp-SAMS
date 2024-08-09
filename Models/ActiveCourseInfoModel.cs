using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace SAMS.Models
{
    public class ActiveCourseInfoModel
    {
        [Key]
        public int CourseId { get; set; } = 0!;
        [Display(Name = ("Name"))]
        public string CourseName { get; set; } = null!;
        [Display(Name = ("Code"))]
        public string CourseCode { get; set; } = null!;
        [Display(Name = ("Level"))]
        public string CourseLevel { get; set; } = null!;
        [Display(Name = ("Teacher"))]
        public string CourseTeacherID { get; set; } = null!;
        [Display(Name = ("Room"))]
        public int CourseRoomID { get; set; } = 0!;
        [Display(Name = ("Bell Number"))]
        public string CourseBellNumber { get; set; } = null!;
        [Display(Name = ("Length"))]
        public string CourseLength { get; set; } = null!;
        [Display(Name = ("Taught on Days"))]
        public string CourseTaughtDays { get; set; } = null!;
        [Display(Name = ("Daily Attendance Applicable?"))]
        public Boolean? DailyAttChecked { get; set; }
        [Display(Name = ("Bell-to-bell Attendance Applicable"))]
        public Boolean? B2BAttChecked { get; set; }

        //Navigation properties
        public TeacherInfoModel Teacher { get; set; } = default!;
        public RoomLocationInfoModel Room { get; set; } = default!;
        public ICollection<BellAttendanceModel>? BellAttendances { get; set; } = default!;
    }
}
