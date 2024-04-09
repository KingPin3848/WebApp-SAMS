using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SAMS.Models
{
    public class RoomLocationInfoModel
    {
        [Display(Name = ("Room Number"))]
        [Key]
        public int RoomNumberMod { get; set; }
        [Display(Name = ("Wing"))]
        public string WingNameMod { get; set; } = null!;
        [Display(Name = ("Scanner ID"))]
        public int RoomScannerId { get; set; } = 0!;

        //Navigation properties
        public TeacherInfoModel? Teacher { get; set; } = null!;
        public ICollection<ActiveCourseInfoModel>? ActiveCourseInfos { get; set; } = null!;
        public ICollection<FastPassModel>? FastPassesIssued { get; set; } = null!;
        public SynnLabQRNodeModel? SynnLabQRNode { get; set; } = null!;
        public RoomQRCodeModel? RoomQRCode { get; set; } = null!;
    }
}
