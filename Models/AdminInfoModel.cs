using System.ComponentModel.DataAnnotations;

namespace SAMS.Models
{
    public class AdminInfoModel
    {
        [Key]
        [Display(Name = ("Admin ID"))]
        public string AdminID { get; set; } = null!;
        [Display(Name = ("First Name"))]
        protected string AdminFirstNameMod { get; set; } = null!;
        [Display(Name = ("Middle Name"))]
        protected string AdminMiddleNameMod { get; set; } = null!;
        [Display(Name = ("Last Name"))]
        protected string AdminLastNameMod { get; set; } = null!;
        [Display(Name = ("Preferred Name"))]
        protected string AdminPreferredNameMod { get; set; } = null!;
        [Display(Name = ("Email Address"))]
        protected string AdminEmailMod { get; set; } = null!;
        [Display(Name = ("Phone Ext."))]
        protected string AdminPhoneMod { get; set; } = null!;
        [Display(Name = ("Label"))]
        protected string AdminLabelMod { get; set; } = null!;

        //Navigation properties
        public ICollection<HallPassInfoModel>? AssignedHallPasses { get; set; }
        public ICollection<HallPassInfoModel>? AddressedHallPasses { get; set; }
        public ICollection<PassRequestInfoModel>? RequestAssignedHallPasses { get; set; }
        public ICollection<PassRequestInfoModel>? RequestAddressedHallPasses { get; set; }
    }
}
