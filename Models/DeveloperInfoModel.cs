using System.ComponentModel.DataAnnotations;

namespace SAMS.Models
{
    public class DeveloperInfoModel
    {
        [Key]
        public int developerID { get; set; } = 0!;
        public string developerFirstNameMod { get; set; } = null!;
        public string developerMiddleNameMod { get; set; } = null!;
        public string developerLastNameMod { get; set; } = null!;
        public string developerPreferredNameMod { get; set; } = null!;
        public string developerEmailMod { get; set; } = null!;
    }
}
