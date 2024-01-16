using System.ComponentModel.DataAnnotations;

namespace SAMS.Models
{
    public class EASuportInfoModel
    {
        [Key]
        [Display(Name = ("EA ID"))]
        public string EaID { get; set; } = null!;
        [Display(Name = ("First Name"))]
        public string EaFirstNameMod { get; set; } = null!;
        [Display(Name = ("Middle Name"))]
        public string EaMiddleNameMod { get; set; } = null!;
        [Display(Name = ("Last Name"))]
        public string EaLastNameMod { get; set; } = null!;
        [Display(Name = ("Preferred Name"))]
        public string EaPreferredNameMod { get; set; } = null!;
        [Display(Name = ("Email Address"))]
        public string EaEmailMod { get; set; } = null!;
        [Display(Name = ("Phone"))]
        public string EaPhoneMod { get; set; } = null!;
        [Display(Name = ("Student Managed"))]
        public int? EaStudentManaged { get; set; } = 0!;

        //Navigation properties
        public ICollection<StudentInfoModel>? Students { get; set; }
    }
}
