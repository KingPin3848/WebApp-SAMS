using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace SAMS.Models
{
    public class ActiveCourseInfoModel
    {
        [Key]
        public int CourseId { get; set; } = 0!;
        public string CourseName { get; set; } = null!;
        public string CourseCode { get; set; } = null!;
        public string CourseLevel { get; set; } = null!;
        public string CourseTeacherID { get; set; } = null!;
        public int CourseRoomID { get; set; } = 0!;
        public string CourseBellNumber { get; set; } = null!;
        public string CourseLength { get; set; } = null!;

        //Navigation properties
        public TeacherInfoModel? Teacher { get; set; }
        public RoomLocationInfoModel? Room { get; set; }
        [AllowNull]
        public ICollection<CourseEnrollmentModel> CourseEnrollments { get; set; }
        public SubstituteInfoModel? Substitute { get; set; }
    }
}
