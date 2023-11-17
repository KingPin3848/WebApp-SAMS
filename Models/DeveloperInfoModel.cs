namespace SAMS.Models
{
    public class DeveloperInfoModel
    {
        protected int? developerID { get; }
        protected string? developerFirstNameMod { get; }
        protected string? developerMiddleNameMod { get; }
        protected string? developerLastNameMod { get; }
        protected string? developerPreferredNameMod { get; private set; }
        protected string? developerEmailMod { get; }
    }
}
