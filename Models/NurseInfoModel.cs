using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace SAMS.Models
{
    public class NurseInfoModel
    {
        [Key]
        public string NurseID { get; set; } = null!;
        public string NurseFirstNameMod { get; set; } = null!;
        [AllowNull]
        public string NurseMiddleNameMod { get; set; } = null!;
        public string NurseLastNameMod { get; set; } = null!;
        public string NursePreferredNameMod { get; set; } = null!;
        public string NurseEmailMod { get; set; } = null!;
        public string NursePhoneMod { get; set; } = null!;

        //Navigation properties
        public ICollection<HallPassInfoModel>? AssignedHallPasses { get; set; }
        public ICollection<HallPassInfoModel>? AddressedHallPasses { get; set; }
        public ICollection<PassRequestInfoModel>? RequestAssignedHallPasses { get; set; }
        public ICollection<PassRequestInfoModel>? RequestAddressedHallPasses { get; set; }
    }
}
