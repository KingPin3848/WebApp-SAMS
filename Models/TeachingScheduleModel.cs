namespace SAMS.Models
{
    public class TeachingScheduleModel
    {
        public int ScheduleID { get; set; }
        public List<string> DaysOfWeek { get; set; } = null!;
        
        //Navigation properties
        public ICollection<TeacherInfoModel>? Teachers { get; set; }
    }
}
