namespace SAMS.Models
{
    public class DailyBellScheduleModel
    {
        public string? BellName { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan? EndTime { get; set;}
        public TimeSpan Duration
        {
            get
            {
                return (TimeSpan)(EndTime - StartTime);
            }
        }
    }
}
