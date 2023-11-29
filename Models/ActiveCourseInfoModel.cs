using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SAMS.Models
{
    public class ActiveCourseInfoModel
    {
        [Key]
        public string courseId { get; set; } = null!;
        public string courseName { get; set; } = null!;
        public string courseCode { get; set; } = null!;
        public string courseLevel { get; set; } = null!;
        public string courseTeacherName { get; set; } = null!;
        public string courseTeacherEmailAddress { get; set; } = null!;
        public int roomNumber { get; set; } = 0!;
        public string wingName { get; set; } = null!;
        public string courseBellNumber { get; set; } = null!;
    }
}
