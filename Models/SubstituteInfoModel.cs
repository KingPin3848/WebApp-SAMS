using System.ComponentModel.DataAnnotations;

namespace SAMS.Models
{
    public class SubstituteInfoModel
    {
        [Key]
        public string SubID { get; set; } = null!;
        public string SubFirstNameMod { get; set; } = null!;
        public string SubMiddleNameMod { get; set; } = null!;
        public string SubLastNameMod { get; set; } = null!;
        public string SubPreferredNameMod { get; set; } = null!;
        public string SubEmailMod { get; set; } = null!;
        public string SubPhoneMod { get; set; } = null!;
        public string ManagedTeacherIdMod { get; set; } = null!;
        public DateTime ScheduledDate { get; set; }

        //Navigation properties
        public TeacherInfoModel? TeacherManaged {  get; set; }

    }
}
