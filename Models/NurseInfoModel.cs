using System.ComponentModel.DataAnnotations;

namespace SAMS.Models
{
    public class NurseInfoModel
    {
        [Key]
        public string nurseID { get; set; } = null!;
        public string nurseFirstNameMod { get; set; } = null!;
        public string nurseMiddleNameMod { get; set; } = null!;
        public string nurseLastNameMod { get; set; } = null!;
        public string nursePreferredNameMod { get; set; } = null!;
        public string nurseEmailMod { get; set; } = null!;
        public string nursePhoneMod { get; set; } = null!;
    }
}
