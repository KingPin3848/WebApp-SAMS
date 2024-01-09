using System.ComponentModel.DataAnnotations;

namespace SAMS.Models
{
    public class EASuportInfoModel
    {
        [Key]
        public string EaID { get; set; } = null!;
        public string EaFirstNameMod { get; set; } = null!;
        public string EaMiddleNameMod { get; set; } = null!;
        public string EaLastNameMod { get; set; } = null!;
        public string EaPreferredNameMod { get; set; } = null!;
        public string EaEmailMod { get; set; } = null!;
        public string EaPhoneMod { get; set; } = null!;
        public int EaStudentManaged { get; set; } = 0!;

        //Navigation properties
        public StudentInfoModel? Student { get; set; }
    }
}
