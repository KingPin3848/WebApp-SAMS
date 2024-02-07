using System.ComponentModel.DataAnnotations;

namespace SAMS.Models
{
    public class LawEnforcementInfoModel
    {
        [Key]
        [Display(Name = ("Law Enforcement ID"))]
        public string LawenfID { get; set; } = null!;
        [Display(Name = ("First Name"))]
        public string LaweFirstNameMod { get; set; } = null!;
        [Display(Name = ("Middle Name"))]
        public string LaweMiddleNameMod { get; set; } = null!;
        [Display(Name = ("Last Name"))]
        public string LaweLastNameMod { get; set; } = null!;
        [Display(Name = ("Preferred Name"))]
        public string LawePreferredNameMod { get; set; } = null!;
        [Display(Name = ("Email Address"))]
        public string LaweEmailMod { get; set; } = null!;
        [Display(Name = ("Phone (Personal/Ext.)"))]
        public string LawePhoneMod { get; set; } = null!;

        //Navigation properties
        public ICollection<HallPassInfoModel>? AssignedHallPasses { get; set; } = null!;
        public ICollection<HallPassInfoModel>? AddressedHallPasses { get; set; } = null!;
        public ICollection<PassRequestInfoModel>? RequestAssignedHallPasses { get; set; } = null!;
        public ICollection<PassRequestInfoModel>? RequestAddressedHallPasses { get; set; } = null!;
    }
}
