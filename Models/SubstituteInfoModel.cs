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
        public int ManagedClassIdMod { get; set; } = 0!;

        //Navigation properties
        public ActiveCourseInfoModel? ClassManaged {  get; set; }

    }
}
