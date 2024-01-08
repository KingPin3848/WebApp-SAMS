using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace SAMS.Models
{
    public class StudentInfoModel
    {
        [Key]
        [DisallowNull]
        public int StudentID { get; set; } = 0!;
        public string StudentFirstNameMod { get; set; } = null!;
        public string StudentMiddleNameMod { get; set; } = null!;
        public string StudentLastNameMod { get; set; } = null!;
        public string StudentPreferredNameMod { get; set; } = null!;
        public string StudentEmailMod { get; set; } = null!;
        public string StudentPhoneMod { get; set; } = null!;
        public DateTime StudentGradYearMod { get; set; }
        public int StudentCounselorID { get; set; } = 0!;
        [DisallowNull]
        public Boolean HasEASupport { get; set; }
        [AllowNull]
        public int StudentEAID { get; set; } = 0!;
        public string Parentguard1NameMod { get; set; } = null!;
        public string Parentguard1EmailMod { get; set; } = null!;
        public string Parentguard2NameMod { get; set; } = null!;
        public string Parentguard2EmailMod { get; set; } = null!;
        public int ActivationCodeId { get; set; } = 0!;


        //WE NEED TO FIND OUT HOW WE CAN INCLUDE IMAGES IN THIS MODEL. EITHER WE STORE IT IN THE
        //DATABASE AND DIRECTLY ACCESS IT IN THE VIEW OR DO IT SOMEWAY OTHER.
        //OR GET IT FROM THEIR GOOGLE ACCOUNT.

        //Navigation Properties
        public CounselorModel? Counselor { get; set; }
        public EASuportInfoModel? AssignedEASuport { get; set; }
        public ICollection<HallPassInfoModel>? HallPasses { get; set; }
        public ICollection<FastPassModel>? FastPasses { get; set; }
        public ICollection<CourseEnrollmentModel>? CourseEnrollments { get; set; }
        public ActivationModel? ActivationCodes { get; set; }
    }
}
