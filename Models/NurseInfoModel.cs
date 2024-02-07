using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace SAMS.Models
{
    public class NurseInfoModel
    {
        [Key]
        [Display(Name = ("Nurse ID"))]
        public string NurseID { get; set; } = null!;
        [Display(Name = ("First Name"))]
        public string NurseFirstNameMod { get; set; } = null!;
        [AllowNull]
        [Display(Name = ("Middle Name"))]
        public string NurseMiddleNameMod { get; set; } = null!;
        [Display(Name = ("Last Name"))]
        public string NurseLastNameMod { get; set; } = null!;
        [Display(Name = ("Preferred Name"))]
        public string NursePreferredNameMod { get; set; } = null!;
        [Display(Name = ("Email Address"))]
        public string NurseEmailMod { get; set; } = null!;
        [Display(Name = ("Phone Ext."))]
        public string NursePhoneMod { get; set; } = null!;

        //Navigation properties
        public ICollection<HallPassInfoModel>? AssignedHallPasses { get; set; } = null!;
        public ICollection<HallPassInfoModel>? AddressedHallPasses { get; set; } = null!;
        public ICollection<PassRequestInfoModel>? RequestAssignedHallPasses { get; set; } = null!;
        public ICollection<PassRequestInfoModel>? RequestAddressedHallPasses { get; set; } = null!;
    }
}
