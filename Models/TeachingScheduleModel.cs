using System.ComponentModel.DataAnnotations;

namespace SAMS.Models
{
    public class TeachingScheduleModel
    {
        [Key]
        [Display(Name = ("Schedule ID"))]
        public int ScheduleID { get; set; } = 0!;
        [Display(Name = ("Teacher ID"))]
        public string TeacherID { get; set; } = null!;
        [Display(Name = ("Teaching Days"))]
        public string DaysOfWeek { get; set; } = null!;

        
        //Navigation properties
        public TeacherInfoModel? Teacher { get; set; } = null!;
        //public RoomScheduleModel? RoomSchedule { get; set; } = null!;
    }
}
