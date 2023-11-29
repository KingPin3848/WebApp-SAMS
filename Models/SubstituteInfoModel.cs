using System.ComponentModel.DataAnnotations;

namespace SAMS.Models
{
    public class SubstituteInfoModel
    {
        [Key]
        public string subID { get; set; } = null!;
        public string subFirstNameMod { get; set; } = null!;
        public string subMiddleNameMod { get; set; } = null!;
        public string subLastNameMod { get; set; } = null!;
        public string subPreferredNameMod { get; set; } = null!;
        public string subEmailMod { get; set; } = null!;
        public string subPhoneMod { get; set; } = null!;
        public string managedTeacherMod { get; set; } = null!;
    }
}
