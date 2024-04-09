using SAMS.Interfaces;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SAMS.Models
{
    public class Sem2StudSchedule : IStudentSchedule
    {
        [Key]
        [Display(Name = ("Student ID"))]
        public int StudentID { get; set; } = 0!;
        [Display(Name = ("Bell 1 Course Code"))]
        public int Bell1CourseIDMod { get; set; }
        [Display(Name = ("Bell 2 Mon/Wed Course Code"))]
        public int Bell2MonWedCourseIDMod { get; set; }
        [Display(Name = ("Bell 2 Tue/Thurs Course Code"))]
        public int Bell2TueThurCourseIDMod { get; set; }
        [Display(Name = ("Bell 3 Mon/Wed Course Code"))]
        public int Bell3MonWedCourseIDMod { get; set; }
        [Display(Name = ("Bell 3 Tue/Thur Course Code"))]
        public int Bell3TueThurCourseIDMod { get; set; }
        [Display(Name = ("Bell 4 Mon/Wed Course Code"))]
        public int Bell4MonWedCourseIDMod { get; set; }
        [Display(Name = ("Bell 4 Tue/Thur Course Code"))]
        public int Bell4TueThurCourseIDMod { get; set; }
        [Display(Name = ("Bell 5 Mon/Wed Course Code"))]
        public int Bell5MonWedCourseIDMod { get; set; }
        [Display(Name = ("Bell 5 Tue/Thur Course Code"))]
        public int Bell5TueThurCourseIDMod { get; set; }
        [Display(Name = ("Bell 6 Mon/Wed Course Code"))]
        public int Bell6MonWedCourseIDMod { get; set; }
        [Display(Name = ("Bell 6 Tue/Thur Course Code"))]
        public int Bell6TueThurCourseIDMod { get; set; }
        [Display(Name = ("Bell 7 Mon/Wed Course Code"))]
        public int Bell7MonWedCourseIDMod { get; set; }
        [Display(Name = ("Bell 7 Tue/Thur Course Code"))]
        public int Bell7TueThurCourseIDMod { get; set; }
        [Display(Name = "Friday Bell 2 Course Code")]
        public int FriBell2CourseIDMod { get; set; }
        [Display(Name = "Friday Bell 3 Course Code")]
        public int FriBell3CourseIDMod { get; set; }
        [Display(Name = "Friday Bell 4 Course Code")]
        public int FriBell4CourseIDMod { get; set; }
        [Display(Name = "Friday Bell 5 Course Code")]
        public int FriBell5CourseIDMod { get; set; }
        [Display(Name = "Friday Bell 6 Course Code")]
        public int FriBell6CourseIDMod { get; set; }
        [Display(Name = "Friday Bell 7 Course Code")]
        public int FriBell7CourseIDMod { get; set; }
        [Display(Name = ("Aves Bell Course Code"))]
        public int AvesBellCourseIDMod { get; set; }
        [Display(Name = ("Lunch Code"))]
        public Char LunchCodeMod { get; set; }

            //Navigation properties
            public StudentInfoModel? Student { get; set; } = null!;
            public ICollection<BellAttendanceModel>? BellAttendance { get; set; } = null!;
            public ICollection<FastPassModel>? FastPasses { get; set; } = null!;
    }
}
