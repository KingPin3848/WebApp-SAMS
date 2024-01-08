using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace SAMS.Models
{
    public class ActiveCourseInfoModel
    {
        [Key]
        public string CourseId { get; set; } = null!;
        public string CourseName { get; set; } = null!;
        public string CourseCode { get; set; } = null!;
        public string CourseLevel { get; set; } = null!;
        public int CourseTeacherID { get; set; } = 0!;
        public int CourseRoomID { get; set; } = 0!;
        public string CourseBellNumber { get; set; } = null!;
        public string CourseLength { get; set; } = null!;

        //Navigation properties
        public TeacherInfoModel? Teacher { get; set; }
        public RoomLocationInfoModel? Room { get; set; }
        [AllowNull]
        public ICollection<CourseEnrollmentModel> CourseEnrollments { get; set; }
    }
}
