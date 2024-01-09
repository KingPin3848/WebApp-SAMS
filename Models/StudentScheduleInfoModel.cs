using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SAMS.Models
{
    public class StudentScheduleInfoModel
    {
        [Key]
        public int StudentID { get; set; } = 0!;
        public int Bell1EnrollmentCodeMod { get; set; } = 0!;
        public int Bell2EnrollmentCodeMod { get; set; } = 0!;
        public int Bell3EnrollmentCodeMod { get; set; } = 0!;
        public int Bell4EnrollmentCodeMod { get; set; } = 0!;
        public int Bell5EnrollmentCodeMod { get; set; } = 0!;
        public int Bell6EnrollmentCodeMod { get; set; } = 0!;
        public int Bell7EnrollmentCodeMod { get; set; } = 0!;
        public int AvesBellRoomCodeMod { get; set; }
        public Char LunchCodeMod { get; set; }

        //Navigation properties
        public StudentInfoModel? Student {  get; set; }
        public CourseEnrollmentModel? CourseEnrollment { get; set; }
        public BellAttendanceModel? BellAttendance { get; set; }
    }
}
