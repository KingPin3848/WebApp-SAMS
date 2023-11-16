using System.ComponentModel.DataAnnotations;

namespace SAMS.Models
{
    public class StudentInfoModel
    {
        protected int studentID { get; }
        protected string? studentFirstNameMod { get; }
        protected string? studentMiddleNameMod { get; }
        protected string? studentLastNameMod { get; }
        protected string? studentPreferredNameMod { get; }
        protected string? studentEmailMod { get; }
        protected string? studentPhoneMod { get; }
        protected DateOnly? studentGradYearMod { get; }
        protected string? studentCounselorNameMod { get; }
        protected string? studentCounselorEmailMod { get; }
        protected string? studentEANameMod { get; }
        protected string? studentEAEmailMod { get; }
        protected string? parentguard1NameMod { get; }
        protected string? parentguard1EmailMod { get; }
        protected string? parentguard2NameMod { get; }
        protected string? parentguard2EmailMod { get; }
        //WE NEED TO FIND OUT HOW WE CAN INCLUDE IMAGES IN THIS MODEL. EITHER WE STORE IT IN THE
        //DATABASE AND DIRECTLY ACCESS IT IN THE VIEW OR DO IT SOMEWAY OTHER.
        //OR GET IT FROM THEIR GOOGLE ACCOUNT.
    }
}
