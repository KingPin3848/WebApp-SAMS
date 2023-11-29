using System.ComponentModel.DataAnnotations;

namespace SAMS.Models
{
    public class AdminInfoModel
    {
        [Key]
        public string adminID { get; set; } = null!;
        protected string adminFirstNameMod { get; set; } = null!;
        protected string adminMiddleNameMod { get; set; } = null!;
        protected string adminLastNameMod { get; set; } = null!;
        protected string adminPreferredNameMod { get; set; } = null!;
        protected string adminEmailMod { get; set; } = null!;
        protected string adminPhoneMod { get; set; } = null!;
        protected string adminLabelMod { get; set; } = null!;
    }
}
