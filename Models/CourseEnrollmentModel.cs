using System.ComponentModel.DataAnnotations;

namespace SAMS.Models
{
    public class CourseEnrollmentModel
    {
        [Key]
        [Display(Name = ("Enrollment ID"))]
        public int EnrollmentId { get; set; } = 0!;
        [Display(Name = ("Student ID"))]
        public int EnrollmentStudentId { get; set; } = 0!;
        [Display(Name = ("Course Code"))]
        public int EnrollmentCourseId { get; set; } = 0!;
        [Display(Name = ("Date/Time"))]
        public DateTime EnrollmentDateTime { get; set; }

        //Navigation properties
        public StudentInfoModel Student { get; set; } = null!;
        public ActiveCourseInfoModel? ActiveCourses { get; set; } = null!;
        public StudentScheduleInfoModel? StudentSchedules { get; set;}
    }
}
