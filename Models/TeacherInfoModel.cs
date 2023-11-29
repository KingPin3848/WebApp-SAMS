using System.ComponentModel.DataAnnotations;

namespace SAMS.Models
{
    public class TeacherInfoModel
    {
        [Key]
        public string teacherID { get; set; } = null!;
        public string teacherFirstNameMod { get; set; } = null!;
        public string teacherMiddleNameMod { get; set; } = null!;
        public string teacherLastNameMod { get; set; } = null!;
        public string teacherPreferredNameMod { get; set; } = null!;
        public string teacherEmailMod { get; set; } = null!;
        public string teacherPhoneMod { get; set; } = null!;
    }
}
