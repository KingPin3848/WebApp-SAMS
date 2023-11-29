using System.ComponentModel.DataAnnotations;

namespace SAMS.Models
{
    public class PassRequestInfoModel
    {
        [Key]
        public int passRequestId { get; set; } = 0!;
        public int studentID { get; set; } = 0!;
        public string studentNameMod { get; set; } = null!;
        public string hallPassAssignedBy { get; set; } = null!;
        public string hallPassAddressee { get; set; } = null!;
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public TimeSpan Duration { get; set; }
        public int bellNumber { get; set; } = 0!;
        public string startLocation { get; set; } = null!;
        public string endLocation { get; set; } = null!;
        public string requestStatus { get; set; } = null!;
    }
}
