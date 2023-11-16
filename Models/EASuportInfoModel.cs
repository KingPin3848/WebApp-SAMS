namespace SAMS.Models
{
    public class EASuportInfoModel
    {
        protected string? eaID { get; }
        protected string? eaFirstNameMod { get; }
        protected string? eaMiddleNameMod { get; }
        protected string? eaLastNameMod { get; }
        protected string? eaPreferredNameMod { get; }
        protected string? eaEmailMod { get; }
        protected string? eaPhoneMod { get; }
        public string? eaStudentManaged { get; private set; }
    }
}
