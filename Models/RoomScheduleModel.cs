using System.ComponentModel.DataAnnotations;

namespace SAMS.Models
{
    public class RoomScheduleModel
    {
        [Key]
        [Display(Name = ("Schedule ID"))]
        public int RoomScheduleID { get; set; }
        [Display(Name = ("Room ID"))]
        public int RoomId { get; set; } = 0!;
        [Display(Name = ("Teacher ID"))]
        public string TeacherID { get; set; } = null!;
        [Display(Name = ("Teaching Schedule ID"))]
        public int ScheduleID { get; set; } = 0!;

        //Navigation properties
        //public RoomLocationInfoModel? Room { get; set; } = null!;
        public TeacherInfoModel? Teacher { get; set; } = null!;
        //public TeachingScheduleModel? Schedule { get; set; } = null!;
    }
}
