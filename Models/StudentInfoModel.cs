using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SAMS.Models
{
    public class StudentInfoModel
    {
        [Key]
        [DisallowNull]
        [Display(Name = ("Student ID"))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int StudentID { get; set; } = 0!;
        [Display(Name = ("First Name"))]
        public string StudentFirstNameMod { get; set; } = null!;
        [Display(Name = ("Middle Name"))]
        public string StudentMiddleNameMod { get; set; } = null!;
        [Display(Name = ("Last Name"))]
        public string StudentLastNameMod { get; set; } = null!;
        [Display(Name = ("Preferred Name"))]
        public string StudentPreferredNameMod { get; set; } = null!;
        [Display(Name = ("Email Address"))]
        public string StudentEmailMod { get; set; } = null!;
        [Display(Name = ("Phone Number"))]
        public string StudentPhoneMod { get; set; } = null!;
        [Display(Name = ("Graduation Year"))]
        public DateTime StudentGradYearMod { get; set; }
        [Display(Name = ("Counselor"))]
        public string StudentCounselorID { get; set; } = null!;
        [DisallowNull]
        [Display(Name = ("EA Support?"))]
        public Boolean HasEASupport { get; set; }
        [Display(Name = ("EA ID"))]
        public string? StudentEAID { get; set; } = null!;
        [Display(Name = ("Parent/Guardian 1 Name"))]
        public string Parentguard1NameMod { get; set; } = null!;
        [Display(Name = ("Parent/Guardian 1 Email Address"))]
        public string Parentguard1EmailMod { get; set; } = null!;
        [Display(Name = ("Parent/Guardian 2 Name"))]
        public string Parentguard2NameMod { get; set; } = null!;
        [Display(Name = ("Parent/Guardian 2 Email Address"))]
        public string Parentguard2EmailMod { get; set; } = null!;
        [Display(Name = ("Assigned Activation Code"))]
        public int ActivationCode { get; set; } = 0!;


        //WE NEED TO FIND OUT HOW WE CAN INCLUDE IMAGES IN THIS MODEL. EITHER WE STORE IT IN THE
        //DATABASE AND DIRECTLY ACCESS IT IN THE VIEW OR DO IT SOMEWAY OTHER.
        //OR GET IT FROM THEIR GOOGLE ACCOUNT.

        //Navigation Properties
        public CounselorModel? Counselor { get; set; } = null!;
        public EASuportInfoModel? AssignedEASuport { get; set; } = null!;
        public ICollection<HallPassInfoModel>? HallPasses { get; set; } = null!;
        public ICollection<FastPassModel>? FastPasses { get; set; } = null!;
        public ICollection<CourseEnrollmentModel>? CourseEnrollments { get; set; } = null!;
        public ICollection<BellAttendanceModel>? BellAttendances { get; set; } = null!;
        public ICollection<DailyAttendanceModel>? DailyAttendances { get; set; } = null!;
        public EASuportInfoModel? EASuport { get; set; } = null!;
        public ICollection<PassRequestInfoModel>? PassRequestsForStudent { get; set; } = null!;
        public StudentScheduleInfoModel? StudentSchedule { get; set; } = null!;
    }
}
