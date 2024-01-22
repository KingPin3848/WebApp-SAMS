using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SAMS.Models
{
    public class StudentScheduleInfoModel
    {
        [Key]
        [Display(Name = ("Student ID"))]
        public int StudentID { get; set; } = 0!;
        [Display(Name = ("Bell 1 Course Code"))]
        public int Bell1EnrollmentCodeMod { get; set; } = 0!;
        [Display(Name = ("Bell 2 Course Code"))]
        public int Bell2EnrollmentCodeMod { get; set; } = 0!;
        [Display(Name = ("Bell 3 Course Code"))]
        public int Bell3EnrollmentCodeMod { get; set; } = 0!;
        [Display(Name = ("Bell 4 Course Code"))]
        public int Bell4EnrollmentCodeMod { get; set; } = 0!;
        [Display(Name = ("Bell 5 Course Code"))]
        public int Bell5EnrollmentCodeMod { get; set; } = 0!;
        [Display(Name = ("Bell 6 Course Code"))]
        public int Bell6EnrollmentCodeMod { get; set; } = 0!;
        [Display(Name = ("Bell 7 Course Code"))]
        public int Bell7EnrollmentCodeMod { get; set; } = 0!;
        [Display(Name = ("Aves Bell Room"))]
        public int AvesBellRoomCodeMod { get; set; }
        [Display(Name = ("Lunch Code"))]
        public Char LunchCodeMod { get; set; }

        //Navigation properties
        public StudentInfoModel? Student {  get; set; }
        public ICollection<CourseEnrollmentModel>? CourseEnrollments { get; set; }
        public ICollection<BellAttendanceModel>? BellAttendance { get; set; }
        public ICollection<FastPassModel>? FastPasses { get; set; }
    }
}
