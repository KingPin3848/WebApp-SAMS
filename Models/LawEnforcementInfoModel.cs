using System.ComponentModel.DataAnnotations;

namespace SAMS.Models
{
    public class LawEnforcementInfoModel
    {
        [Key]
        public string LawenfID { get; set; } = null!;
        public string LaweFirstNameMod { get; set; } = null!;
        public string LaweMiddleNameMod { get; set; } = null!;
        public string LaweLastNameMod { get; set; } = null!;
        public string LawePreferredNameMod { get; set; } = null!;
        public string LaweEmailMod { get; set; } = null!;
        public string LawePhoneMod { get; set; } = null!;

        //Navigation properties
        public ICollection<HallPassInfoModel>? AssignedHallPasses { get; set; }
        public ICollection<HallPassInfoModel>? AddressedHallPasses { get; set; }
    }
}
