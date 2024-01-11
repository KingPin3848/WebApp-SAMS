using System.ComponentModel.DataAnnotations;

namespace SAMS.Models
{
    public class SubstituteInfoModel
    {
        [Key]
        [Display(Name = ("Substitute Teacher ID"))]
        public string SubID { get; set; } = null!;
        [Display(Name = ("First Name"))]
        public string SubFirstNameMod { get; set; } = null!;
        [Display(Name = ("Middle Name"))]
        public string SubMiddleNameMod { get; set; } = null!;
        [Display(Name = ("Last Name"))]
        public string SubLastNameMod { get; set; } = null!;
        [Display(Name = ("Preferred Name"))]
        public string SubPreferredNameMod { get; set; } = null!;
        [Display(Name = ("Email Address"))]
        public string SubEmailMod { get; set; } = null!;
        [Display(Name = ("Phone"))]
        public string SubPhoneMod { get; set; } = null!;
        [Display(Name = ("Teacher Managed"))]
        public string ManagedTeacherIdMod { get; set; } = null!;
        [Display(Name = ("Date/Time"))]
        public DateTime ScheduledDate { get; set; }

        //Navigation properties
        public TeacherInfoModel? TeacherManaged {  get; set; }

    }
}
