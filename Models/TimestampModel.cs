using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SAMS.Models
{
    public class TimestampModel
    {
        [Key]
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string ActionMade { get; set; } = string.Empty;
        public string MadeBy { get; set; } = string.Empty;
        public string Comments { get; set; } = string.Empty;
    }
}
