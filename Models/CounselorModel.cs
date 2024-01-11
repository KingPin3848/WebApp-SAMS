using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace SAMS.Models
{
    public class CounselorModel
    {
        [Key]
        public string CounselorId { get; set; } = null!;
        public string CounselorFirstName { get; set; } = null!;
        public string CounselorMiddleName { get; set; } = null!;
        public string CounselorLastName { get; set; } = null!;
        public string CounselorPreferredName { get; set; } = null!;
        public string CounselorEmail { get; set; } = null!;
        public int CounselorPhone { get; set; } = 0!;

        //Navigation Properties
        public ICollection<StudentInfoModel>? CounselorManagedStudents { get; set; }
        public ICollection<HallPassInfoModel>? AssignedHallPasses { get; set; }
        public ICollection<HallPassInfoModel>? AddressedHallPasses { get; set; }
        public ICollection<PassRequestInfoModel>? RequestAssignedHallPasses { get; set; }
        public ICollection<PassRequestInfoModel>? RequestAddressedHallPasses { get; set; }
    }
}
