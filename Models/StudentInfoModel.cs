using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace SAMS.Models
{
    public class StudentInfoModel
    {
        [Key]
        [Required]
        [Display(Name = ("Student ID"))]
        [ProtectedPersonalData]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int StudentID { get; set; } = 0!;


        [Display(Name = ("First Name"))]
        [Required]
        [ProtectedPersonalData]
        public required string StudentFirstNameMod { get; set; }


        [Display(Name = ("Middle Name"))]
        [ProtectedPersonalData]
        public string? StudentMiddleNameMod { get; set; }


        [Display(Name = ("Last Name"))]
        [Required]
        [ProtectedPersonalData]
        public required string StudentLastNameMod { get; set; }


        [Display(Name = ("Preferred Name"))]
        [ProtectedPersonalData]
        public string? StudentPreferredNameMod { get; set; }


        [Display(Name = ("Email Address"))]
        [EmailAddress]
        [Required]
        [ProtectedPersonalData]
        public required string StudentEmailMod { get; set; }


        [Display(Name = ("Phone Number"))]
        [Phone]
        [ProtectedPersonalData]
        public string? StudentPhoneMod { get; set; }


        [Display(Name = ("Graduation Year"))]
        [Required]
        [ProtectedPersonalData]
        public required DateTime StudentGradYearMod { get; set; }


        [Display(Name = ("Counselor"))]
        [Required]
        [PersonalData]
        public required string StudentCounselorID { get; set; }


        [Display(Name = ("EA Support?"))]
        [ProtectedPersonalData]
        public Boolean? HasEASupport { get; set; }


        [Display(Name = ("EA ID"))]
        [ProtectedPersonalData]
        public string? StudentEAID { get; set; }


        [Display(Name = ("Parent/Guardian 1 Name"))]
        [Required]
        [ProtectedPersonalData]
        public required string Parentguard1NameMod { get; set; }


        [Display(Name = ("Parent/Guardian 1 Email Address"))]
        [EmailAddress]
        [Required]
        [ProtectedPersonalData]
        public required string Parentguard1EmailMod { get; set; }


        [Display(Name = ("Parent/Guardian 2 Name"))]
        [ProtectedPersonalData]
        public string? Parentguard2NameMod { get; set; }


        [Display(Name = ("Parent/Guardian 2 Email Address"))]
        [EmailAddress]
        [ProtectedPersonalData]
        public string? Parentguard2EmailMod { get; set; }


        [Display(Name = ("Assigned Activation Code"))]
        public int ActivationCode { get; set; } = 0!;



        //WE NEED TO FIND OUT HOW WE CAN INCLUDE IMAGES IN THIS MODEL. EITHER WE STORE IT IN THE
        //DATABASE AND DIRECTLY ACCESS IT IN THE VIEW OR DO IT SOMEWAY OTHER.
        //OR GET IT FROM THEIR GOOGLE ACCOUNT.

        //Navigation Properties
#pragma warning disable CA2227 // Collection properties should be read only
        public CounselorModel Counselor { get; set; } = default!;
        public EASuportInfoModel? AssignedEASuport { get; set; } = null!;
        public ICollection<HallPassInfoModel> HallPasses { get; set; } = default!;
        public ICollection<FastPassModel> FastPasses { get; set; } = default!;
        public ICollection<BellAttendanceModel> BellAttendances { get; set; } = default!;
        public ICollection<DailyAttendanceModel> DailyAttendances { get; set; } = default!;
        public EASuportInfoModel? EASuport { get; set; } = null!;
        public ICollection<PassRequestInfoModel> PassRequestsForStudent { get; set; } = default!;
        public Sem1StudSchedule Sem1StudSchedule { get; set; } = default!;
        public Sem2StudSchedule Sem2StudSchedule { get; set; } = default!;
        public StudentLocationModel StudentLocation { get; set; } = default!;
#pragma warning restore CA2227 // Collection properties should be read only
    }
}
