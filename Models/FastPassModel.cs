using System.ComponentModel.DataAnnotations;

namespace SAMS.Models
{
    public class FastPassModel
    {
        public int studentID { get; set; } = 0!;
        [Key]
        public string fastPassIDMod { get; set; } = null!;
        public string studentNameMod { get; set; } = null!;
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public TimeSpan Duration { get; set; }
        public int bellNumber { get; set; } = 0!;
        public string startLocation { get; set; } = null!;
        public string endLocation { get; set; } = null!;
    }
}
