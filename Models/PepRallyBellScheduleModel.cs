namespace SAMS.Models
{
    public class PepRallyBellScheduleModel
    {
        public string? BellName { get; }
        public TimeSpan? StartTime { get; }
        public TimeSpan? EndTime { get; }
        public TimeSpan? Duration { get; }
    }
}
