using System.ComponentModel.DataAnnotations;

namespace SAMS.Models
{
    public class HallPassInfoModel
    {
        public int studentID { get; set; } = 0!;
        [Key]
        public string HallPassID { get; set; } = null!;
        public string studentNameMod { get; set; } = null!;
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public TimeSpan Duration { get; set; }
        public int bellNumber { get; set; } = 0!;
        public string hallPassAssignedBy { get; set; } = null!;
        public string hallPassAddressee { get; set; } = null!;
        public string startLocation { get; set; } = null!;
        public string endLocation { get; set; } = null!;
    }
}