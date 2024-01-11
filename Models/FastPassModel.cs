using System.ComponentModel.DataAnnotations;

namespace SAMS.Models
{
    public class FastPassModel
    {
        [Key]
        public string FastPassIDMod { get; set; } = null!;
        public int StudentID { get; set; } = 0!;
        public string StudentNameMod { get; set; } = null!;
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public TimeSpan Duration { get { return Duration; } set { Duration = EndDateTime - StartDateTime; } }
        public int BellNumber { get; set; } = 0!;
        public int StartLocationID { get; set; } = 0!;
        public int EndLocationID { get; set; } = 0!;
        public int CourseIDFromStudentSchedule { get; set; } = 0!;



        //Navigation properties
        public StudentInfoModel? Student {  get; set; }

        //REVIEW THIS ONE ABOUT ROOM LOCATION INFO MODEL AND IF THIS SHOULD BE AN ICOLLECTION OR A SINGULAR
        public RoomLocationInfoModel? Room { get; set; }
        public StudentScheduleInfoModel? StudentSchedule { get; set; }
    }
}
