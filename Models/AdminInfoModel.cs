using System.ComponentModel.DataAnnotations;

namespace SAMS.Models
{
    public class AdminInfoModel
    {
        [Key]
        public string AdminID { get; set; } = null!;
        protected string AdminFirstNameMod { get; set; } = null!;
        protected string AdminMiddleNameMod { get; set; } = null!;
        protected string AdminLastNameMod { get; set; } = null!;
        protected string AdminPreferredNameMod { get; set; } = null!;
        protected string AdminEmailMod { get; set; } = null!;
        protected string AdminPhoneMod { get; set; } = null!;
        protected string AdminLabelMod { get; set; } = null!;

        //Navigation properties
        public ICollection<HallPassInfoModel>? AssignedHallPasses { get; set; }
        public ICollection<HallPassInfoModel>? AddressedHallPasses { get; set; }
    }
}
