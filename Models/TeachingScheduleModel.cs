using System.ComponentModel.DataAnnotations;

namespace SAMS.Models
{
    public class TeachingScheduleModel
    {
        [Key]
        public int ScheduleID { get; set; } = 0!;
        public string TeacherID { get; set; } = null!;
        public string DaysOfWeek { get; set; } = null!;

        
        //Navigation properties
        public TeacherInfoModel? Teacher { get; set; }
        public RoomScheduleModel? RoomSchedule { get; set; }
    }
}
