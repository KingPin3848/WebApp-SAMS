using System.ComponentModel.DataAnnotations;

namespace SAMS.Models
{
    public class AdminInfoModel
    {
        [Key]
        [Display(Name = ("Admin ID"))]
        public string AdminID { get; set; } = null!;
        [Display(Name = ("First Name"))]
        public string AdminFirstNameMod { get; set; } = null!;
        [Display(Name = ("Middle Name"))]
        public string AdminMiddleNameMod { get; set; } = null!;
        [Display(Name = ("Last Name"))]
        public string AdminLastNameMod { get; set; } = null!;
        [Display(Name = ("Preferred Name"))]
        public string AdminPreferredNameMod { get; set; } = null!;
        [Display(Name = ("Email Address"))]
        public string AdminEmailMod { get; set; } = null!;
        [Display(Name = ("Phone Ext."))]
        public string AdminPhoneMod { get; set; } = null!;
        [Display(Name = ("Label"))]
        public string AdminLabelMod { get; set; } = null!;

        //Navigation properties
        public ICollection<HallPassInfoModel>? AssignedHallPasses { get; set; }
        public ICollection<HallPassInfoModel>? AddressedHallPasses { get; set; }
        public ICollection<PassRequestInfoModel>? RequestAssignedHallPasses { get; set; }
        public ICollection<PassRequestInfoModel>? RequestAddressedHallPasses { get; set; }
    }
}
