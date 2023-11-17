namespace SAMS.Models
{
    public class FastPassModel
    {
        protected int? studentID { get; private set; }
        protected string? fastPassIDMod { get; private set; }
        protected string? studentNameMod { get; private set; }
        public TimeSpan? StartTime { get; private set; }
        public TimeSpan? EndTime { get; private set; }
        public TimeSpan? Duration { get; private set; }
        public int? bellNumber { get; private set; }
        protected string? startLocation { get; private set; }
        protected string? endLocation { get; private set; }
    }
}
