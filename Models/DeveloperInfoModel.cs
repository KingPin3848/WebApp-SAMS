using System.ComponentModel.DataAnnotations;

namespace SAMS.Models
{
    public class DeveloperInfoModel
    {
        [Key]
        [Display(Name = ("Developer ID"))]
        public int DeveloperID { get; set; } = 0!;
        [Display(Name = ("First Name"))]
        public string DeveloperFirstNameMod { get; set; } = null!;
        [Display(Name = ("Middle Name"))]
        public string DeveloperMiddleNameMod { get; set; } = null!;
        [Display(Name = ("Last Name"))]
        public string DeveloperLastNameMod { get; set; } = null!;
        [Display(Name = ("Preferred Name"))]
        public string DeveloperPreferredNameMod { get; set; } = null!;
        [Display(Name = ("Email Address"))]
        public string DeveloperEmailMod { get; set; } = null!;
    }
}
