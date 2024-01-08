using System.ComponentModel.DataAnnotations;

namespace SAMS.Models
{
    public class DeveloperInfoModel
    {
        [Key]
        public int DeveloperID { get; set; } = 0!;
        public string DeveloperFirstNameMod { get; set; } = null!;
        public string DeveloperMiddleNameMod { get; set; } = null!;
        public string DeveloperLastNameMod { get; set; } = null!;
        public string DeveloperPreferredNameMod { get; set; } = null!;
        public string DeveloperEmailMod { get; set; } = null!;
    }
}
