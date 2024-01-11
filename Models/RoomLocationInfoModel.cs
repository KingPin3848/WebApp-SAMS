using System.ComponentModel.DataAnnotations;

namespace SAMS.Models
{
    public class RoomLocationInfoModel
    {
        [Key]
        [Display(Name = ("Room ID"))]
        public int RoomId { get; set; }
        [Display(Name = ("Room Number"))]
        public string RoomNumberMod { get; set; } = null!;
        [Display(Name = ("Wing"))]
        public string WingNameMod { get; set; } = null!;
        [Display(Name = ("Room Code"))]
        public string RoomCodeMod { get; set; } = null!;
        [Display(Name = ("Assigned To"))]
        public string RoomAssignedToTeacherID { get; set; } = null!;
        [Display(Name = ("Scanner ID"))]
        public int RoomScannerId { get; set; } = 0!;

        //Navigation properties
        public TeacherInfoModel? Teacher {  get; set; }
        public ICollection<ActiveCourseInfoModel>? ActiveCourseInfos { get; set; }
        public ICollection<FastPassModel>? FastPassesIssued { get; set; }
        public RoomScheduleModel? RoomSchedule { get; set; }
        public SynnLabQRNodeModel? SynnLabQRNode { get; set; }
    }
}
