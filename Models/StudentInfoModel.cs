using System.ComponentModel.DataAnnotations;

namespace SAMS.Models
{
    public class StudentInfoModel
    {
        [Key]
        public int studentID { get; set; } = 0!;
        public string studentFirstNameMod { get; set; } = null!;
        public string studentMiddleNameMod { get; set; } = null!;
        public string studentLastNameMod { get; set; } = null!;
        public string studentPreferredNameMod { get; set; } = null!;
        public string studentEmailMod { get; set; } = null!;
        public string studentPhoneMod { get; set; } = null!;
        public DateOnly studentGradYearMod { get; }
        public string studentCounselorNameMod { get; set; } = null!;
        public string studentCounselorEmailMod { get; set; } = null!;
        public string studentEANameMod { get; set; } = null!;
        public string studentEAEmailMod { get; set; } = null!;
        public string parentguard1NameMod { get; set; } = null!;
        public string parentguard1EmailMod { get; set; } = null!;
        public string parentguard2NameMod { get; set; } = null!;
        public string parentguard2EmailMod { get; set; } = null!;
        //WE NEED TO FIND OUT HOW WE CAN INCLUDE IMAGES IN THIS MODEL. EITHER WE STORE IT IN THE
        //DATABASE AND DIRECTLY ACCESS IT IN THE VIEW OR DO IT SOMEWAY OTHER.
        //OR GET IT FROM THEIR GOOGLE ACCOUNT.
    }
}
