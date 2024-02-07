using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace SAMS.Models
{
    public class CounselorModel
    {
        [Key]
        [Display(Name = ("Counselor ID"))]
        public string CounselorId { get; set; } = null!;
        [Display(Name = ("First Name"))]
        public string CounselorFirstName { get; set; } = null!;
        [Display(Name = ("Middle Name"))]
        public string CounselorMiddleName { get; set; } = null!;
        [Display(Name = ("Last Name"))]
        public string CounselorLastName { get; set; } = null!;
        [Display(Name = ("Preferred Name"))]
        public string CounselorPreferredName { get; set; } = null!;
        [Display(Name = ("Email Address"))]
        public string CounselorEmail { get; set; } = null!;
        [Display(Name = ("Phone Ext."))]
        public int CounselorPhone { get; set; } = 0!;

        //Navigation Properties
        public ICollection<StudentInfoModel>? CounselorManagedStudents { get; set; } = null!;
        public ICollection<HallPassInfoModel>? AssignedHallPasses { get; set; } = null!;
        public ICollection<HallPassInfoModel>? AddressedHallPasses { get; set; } = null!;
        public ICollection<PassRequestInfoModel>? RequestAssignedHallPasses { get; set; } = null!;
        public ICollection<PassRequestInfoModel>? RequestAddressedHallPasses { get; set; } = null!;
    }
}
