using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SAMS.Models
{
    public class StudentScheduleInfoModel
    {
        [Key]
        public int StudentID { get; set; } = 0!;
        public string Bell1EnrollmentCodeMod { get; set; } = null!;
        public string Bell2EnrollmentCodeMod { get; set; } = null!;
        public string Bell3EnrollmentCodeMod { get; set; } = null!;
        public string Bell4EnrollmentCodeMod { get; set; } = null!;
        public string Bell5EnrollmentCodeMod { get; set; } = null!;
        public string Bell6EnrollmentCodeMod { get; set; } = null!;
        public string Bell7EnrollmentCodeMod { get; set; } = null!;
        public int AvesBellRoomCodeMod { get; set; }
        public Char LunchCodeMod { get; set; }

        //Navigation properties
        public StudentInfoModel? Student {  get; set; }
        public CourseEnrollmentModel? CourseEnrollment { get; set; }
    }
}
