namespace SAMS.Models
{
    public class TwoHrDelayBellScheduleModel
    {
        public string? BellName { get; }
        public TimeSpan? StartTime { get; }
        public TimeSpan? EndTime { get; }
        public TimeSpan? Duration { get; }
    }
}
