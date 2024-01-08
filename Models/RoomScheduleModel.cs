using System.ComponentModel.DataAnnotations;

namespace SAMS.Models
{
    public class RoomScheduleModel
    {
        [Key]
        public int RoomScheduleID { get; set; }
        public int RoomId { get; set; } = 0!;
        public string TeacherID { get; set; } = null!;
        public int ScheduleID { get; set; } = 0!;

        //Navigation properties
        public RoomLocationInfoModel? Room {  get; set; }
        public TeacherInfoModel? Teacher { get; set; }
        public TeachingScheduleModel? Schedule { get; set; }
    }
}
