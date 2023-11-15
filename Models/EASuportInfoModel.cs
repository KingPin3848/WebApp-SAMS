namespace SAMS.Models
{
    public class EASuportInfoModel
    {
        protected string? eaID { get; private set; }
        protected string? eaFirstNameMod { get; set; }
        protected string? eaMiddleNameMod { get; set; }
        protected string? eaLastNameMod { get; set; }
        protected string? eaPreferredNameMod { get; set; }
        protected string? eaEmailMod { get; set; }
        protected string? eaPhoneMod { get; set; }
        public string? eaStudentManaged { get; set; }
    }
}
