using System.ComponentModel.DataAnnotations;

namespace SAMS.Models
{
    public class LawEnforcementInfoModel
    {
        [Key]
        public string lawenfID { get; set; } = null!;
        public string laweFirstNameMod { get; set; } = null!;
        public string laweMiddleNameMod { get; set; } = null!;
        public string laweLastNameMod { get; set; } = null!;
        public string lawePreferredNameMod { get; set; } = null!;
        public string laweEmailMod { get; set; } = null!;
        public string lawePhoneMod { get; set; } = null!;
    }
}
