using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAMS.Models
{
    public class StudentViewModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public IEnumerable<StudentInfoModel> StudentInfo { get; set; }
        public IEnumerable<BellAttendanceModel> AttendanceHistory { get; set; }
        public IEnumerable<TimestampModel> Location { get; set; }

    }
}
