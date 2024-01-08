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
        public string RoomAssignedToTeacher1ID { get; set; } = null!;

        //Navigation properties
        public TeacherInfoModel? Teacher {  get; set; }
        public ICollection<ActiveCourseInfoModel>? ActiveCourseInfos { get; set; }
    }
}
