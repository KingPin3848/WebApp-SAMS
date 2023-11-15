using System.ComponentModel.DataAnnotations;

namespace SAMS.Models
{
    public class StudentInfoModel
    {
        protected int studentID { get; private set; }
        protected string? studentFirstNameMod { get; private set; }
        protected string? studentMiddleNameMod { get; private set; }
        protected string? studentLastNameMod { get; private set; }
        protected string? studentPreferredNameMod { get; private set; }
        protected string? studentEmailMod { get; private set; }
        protected string? studentPhoneMod { get; private set; }
        protected DateOnly? studentGradYearMod { get; private set; }
        protected string? studentCounselorNameMod { get; private set; }
        protected string? studentCounselorEmailMod { get; private set; }
        protected string? studentEANameMod { get; private set; }
        protected string? studentEAEmailMod { get; private set; }
        protected string? parentguard1NameMod { get; private set; }
        protected string? parentguard1EmailMod { get; private set; }
        protected string? parentguard2NameMod { get; private set; }
        protected string? parentguard2EmailMod { get; private set; }
        //WE NEED TO FIND OUT HOW WE CAN INCLUDE IMAGES IN THIS MODEL. EITHER WE STORE IT IN THE
        //DATABASE AND DIRECTLY ACCESS IT IN THE VIEW OR DO IT SOMEWAY OTHER.
        //OR GET IT FROM THEIR GOOGLE ACCOUNT.
    }
}
