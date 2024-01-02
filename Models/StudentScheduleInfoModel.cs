using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SAMS.Models
{
    public class StudentScheduleInfoModel
    {
        [Key]
        [Display(Name ="Student ID")]
        public int studentID { get; set; } = 0!;
        [Display(Name ="Student First Name")]
        public string studentFirstNameMod { get; set; } = null!;
        [Display(Name ="Student Middle Name")]
        public string studentMiddleNameMod { get; set; } = null!;
        [Display(Name ="Student Last Name")]
        public string studentLastNameMod { get; set; } = null!;
        public string bell1CourseCodeMod { get; set; } = null!;
        public string bell2CourseCodeMod { get; set; } = null!;
        public string bell3CourseCodeMod { get; set; } = null!;
        public string bell4CourseCodeMod { get; set; } = null!;
        public string bell5CourseCodeMod { get; set; } = null!;
        public string bell6CourseCodeMod { get; set; } = null!;
        public string bell7CourseCodeMod { get; set; } = null!;
        public string avesBellRoomCodeMod { get; set; } = null!;
        public string lunchCodeMod { get; set; } = null!;
    }
}
