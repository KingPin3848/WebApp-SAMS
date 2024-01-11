using System.ComponentModel.DataAnnotations;

namespace SAMS.Models
{
    public class TeacherInfoModel
    {
        [Key]
        [Display(Name = ("Teacher ID"))]
        public string TeacherID { get; set; } = null!;
        [Display(Name = ("First Name"))]
        public string TeacherFirstNameMod { get; set; } = null!;
        [Display(Name = ("Middle Name"))]
        public string TeacherMiddleNameMod { get; set; } = null!;
        [Display(Name = ("Last Name"))]
        public string TeacherLastNameMod { get; set; } = null!;
        [Display(Name = ("Preferred Name"))]
        public string TeacherPreferredNameMod { get; set; } = null!;
        [Display(Name = ("Email Address"))]
        public string TeacherEmailMod { get; set; } = null!;
        [Display(Name = ("Phone Ext."))]
        public string TeacherPhoneMod { get; set; } = null!;
        [Display(Name = ("Teaches all 5 days?"))]
        public Boolean Teaches5Days { get; set; }
        [Display(Name = ("Teaching Schedule ID"))]
        public int TeachingScheduleID { get; set; } = 0!;


        //Navigation properties
        public ICollection<ActiveCourseInfoModel>? ActiveCourses { get; set; }
        public SubstituteInfoModel? SubTeachers { get; set; }
        public RoomLocationInfoModel? Room {  get; set; }
        public ICollection<RoomScheduleModel>? RoomSchedules { get; set; }
        public TeachingScheduleModel? TeachingSchedule { get; set; }
        public ICollection<HallPassInfoModel>? AssignedHallPasses { get; set; }
        public ICollection<HallPassInfoModel>? AddressedHallPasses { get; set; }
        public ICollection<PassRequestInfoModel>? RequestAssignedHallPasses { get; set; }
        public ICollection<PassRequestInfoModel>? RequestAddressedHallPasses { get; set; }
    }
}
