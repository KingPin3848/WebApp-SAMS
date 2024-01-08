using System.ComponentModel.DataAnnotations;

namespace SAMS.Models
{
    public class CourseEnrollmentModel
    {
        [Key]
        public int EnrollmentId { get; set; } = 0!;
        public int EnrollmentStudentId { get; set; } = 0!;
        public int EnrollmentCourseId { get; set; } = 0!;
        public DateTime EnrollmentDateTime { get; set; }

        //Navigation properties
        public StudentInfoModel Student { get; set; } = null!;
        public ActiveCourseInfoModel? ActiveCourses { get; set; } = null!;
    }
}
