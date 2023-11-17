namespace SAMS.Models
{
    public class PassRequestInfoModel
    {
        protected int? studentID { get; private set; }
        protected string? studentNameMod { get; private set; protected set; }
        protected string? hallPassAssignedBy { get; private set; }
        protected string? hallPassAddressee { get; private set; }
        public TimeSpan? StartTime { get; protected set; }
        public TimeSpan? EndTime { get; protected set; }
        public TimeSpan? Duration { get; protected set; }
        public int? bellNumber { get; protected set; }
        protected string? startLocation { get; private set; }
        protected string? endLocation { get; private set; }
        protected string? requestStatus { get; private set; }
    }
}
