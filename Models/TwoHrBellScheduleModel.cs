using SAMS.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace SAMS.Models
{
    public class TwoHrDelayBellScheduleModel : IBellSchedule
    {
        [Display(Name = ("Bell"))]
        public string BellName { get; set; } = null!;
        [Key]
        [Display(Name = ("Start Time"))]
        public TimeSpan StartTime { get; set; }
        [Display(Name = ("End Time"))]
        public TimeSpan EndTime { get; set; }
        [Display(Name = ("Duration"))]
        public TimeSpan Duration { get; set; }
    }
}
