namespace SAMS.Models
{
    public class HallPassInfoModel
    {
        protected int? studentID { get; private set; }
        protected string? HallPassID { get; private set; }
        protected string? studentNameMod { get; private set; }
        public TimeSpan? StartTime { get; protected set; }
        public TimeSpan? EndTime { get; protected set; }
        public TimeSpan? Duration { get; protected set; }
        public int? bellNumber { get; protected set; }
        protected string? hallPassAssignedBy { get; private set; }
        protected string? hallPassAddressee { get; private set; }
        public string? startLocation { get; private set; }
        public string? endLocation { get; set; }
    }
}