namespace SAMS.Interfaces
{
    public interface IBellSchedule
    {
        public string BellName { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public TimeSpan Duration { get; set; }
    }
}
