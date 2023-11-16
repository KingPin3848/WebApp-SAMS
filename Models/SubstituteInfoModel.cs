namespace SAMS.Models
{
    public class SubstituteInfoModel
    {
        protected string? subID { get; }
        protected string? subFirstNameMod { get; }
        protected string? subMiddleNameMod { get; }
        protected string? subLastNameMod { get; }
        protected string? subPreferredNameMod { get; }
        protected string? subEmailMod { get; }
        protected string? subPhoneMod { get; }
        protected string? managedTeacherMod { get; private set; }
    }
}
