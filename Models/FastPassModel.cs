using System.ComponentModel.DataAnnotations;

namespace SAMS.Models
{
    public class FastPassModel
    {
        [Key]
        [Display(Name = ("FastPass ID"))]
        public string FastPassIDMod { get; set; } = null!;
        [Display(Name = ("Student ID"))]
        public int StudentID { get; set; } = 0!;
        [Display(Name = ("Student Name"))]
        public string StudentNameMod { get; set; } = null!;
        [Display(Name = ("Issuance Time"))]
        public DateTime StartDateTime { get; set; }
        [Display(Name = ("Expiration Time"))]
        public DateTime EndDateTime { get; set; }
        [Display(Name = ("FastPass Duration"))]
        private TimeSpan _Duration;
        public TimeSpan Duration
        {
            get
            {
                return _Duration;
            }
            set
            {
                _Duration = EndDateTime - StartDateTime;
            }
        }
        [Display(Name = ("Bell"))]
        public int BellNumber { get; set; } = 0!;
        [Display(Name = ("Start Location"))]
        public int StartLocationID { get; set; } = 0!;
        [Display(Name = ("End Location"))]
        public int EndLocationID { get; set; } = 0!;
        [Display(Name = ("Course"))]
        public int CourseIDFromStudentSchedule { get; set; } = 0!;



        //Navigation properties
        public StudentInfoModel? Student { get; set; } = null!;

        //REVIEW THIS ONE ABOUT ROOM LOCATION INFO MODEL AND IF THIS SHOULD BE AN ICOLLECTION OR A SINGULAR
        public RoomLocationInfoModel? Room { get; set; } = null!;
        public Sem1StudSchedule? Sem1StudSchedule { get; set; } = null!;
        public Sem2StudSchedule? Sem2StudSchedule { get; set; } = null!;
    }
}
