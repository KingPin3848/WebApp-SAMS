using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace SAMS.Models
{
    public class CounselorModel
    {
        [Key]
        [Display(Name = ("Counselor ID"))]
        [Required]
        [ProtectedPersonalData]
        public required string CounselorId { get; set; }


        [Display(Name = ("First Name"))]
        [Required]
        [ProtectedPersonalData]
        public required string CounselorFirstName { get; set; }


        [Display(Name = ("Middle Name"))]
        [ProtectedPersonalData]
        public string? CounselorMiddleName { get; set; }


        [Display(Name = ("Last Name"))]
        [Required]
        [ProtectedPersonalData]
        public required string CounselorLastName { get; set; }


        [Display(Name = ("Preferred Name"))]
        [ProtectedPersonalData]
        public string? CounselorPreferredName { get; set; }


        [Display(Name = ("Email Address"))]
        [EmailAddress]
        [ProtectedPersonalData]
        public required string CounselorEmail { get; set; }


        [Display(Name = ("Phone Ext."))]
        [ProtectedPersonalData]
        public string? CounselorPhone { get; set; }




        //Navigation Properties
#pragma warning disable CA2227 // Collection properties should be read only
        public ICollection<StudentInfoModel>? CounselorManagedStudents { get; set; } = null!;
        public ICollection<HallPassInfoModel>? AssignedHallPasses { get; set; } = null!;
        public ICollection<HallPassInfoModel>? AddressedHallPasses { get; set; } = null!;
        public ICollection<PassRequestInfoModel>? RequestAssignedHallPasses { get; set; } = null!;
        public ICollection<PassRequestInfoModel>? RequestAddressedHallPasses { get; set; } = null!;
#pragma warning restore CA2227 // Collection properties should be read only
    }
}
