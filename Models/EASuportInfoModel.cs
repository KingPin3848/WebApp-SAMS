using System.ComponentModel.DataAnnotations;

namespace SAMS.Models
{
    public class EASuportInfoModel
    {
        [Key]
        public string eaID { get; set; } = null!;
        public string eaFirstNameMod { get; set; } = null!;
        public string eaMiddleNameMod { get; set; } = null!;
        public string eaLastNameMod { get; set; } = null!;
        public string eaPreferredNameMod { get; set; } = null!;
        public string eaEmailMod { get; set; } = null!;
        public string eaPhoneMod { get; set; } = null!;
        public string eaStudentManaged { get; set; } = null!;
    }
}
