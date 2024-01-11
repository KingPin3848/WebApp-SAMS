using System.ComponentModel.DataAnnotations;

namespace SAMS.Models
{
    public class RoomLocationInfoModel
    {
        [Key]
        public int RoomId { get; set; }
        public string RoomNumberMod { get; set; } = null!;
        public string WingNameMod { get; set; } = null!;
        public string RoomCodeMod { get; set; } = null!;
        public string RoomAssignedToTeacherID { get; set; } = null!;
        public int RoomScannerId { get; set; } = 0!;

        //Navigation properties
        public TeacherInfoModel? Teacher {  get; set; }
        public ICollection<ActiveCourseInfoModel>? ActiveCourseInfos { get; set; }
        public ICollection<FastPassModel>? FastPassesIssued { get; set; }
        public RoomScheduleModel? RoomSchedule { get; set; }
        public SynnLabQRNodeModel? SynnLabQRNode { get; set; }
    }
}
