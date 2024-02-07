using System.ComponentModel.DataAnnotations;

namespace SAMS.Models
{
    public class SynnLabQRNodeModel
    {
        [Key]
        [Display(Name = ("Scanner ID"))]
        public string ScannerID { get; set; } = null!;
        [Display(Name = ("Room ID"))]
        public int SynnlabRoomIDMod { get; set; } = 0!;
        [Display(Name = ("MAC Address"))]
        public string ScannerMacAddressMod { get; set; } = null!;
        [Display(Name = ("Model Number"))]
        public string ModelNumberMod { get; set; } = null!;
        [Display(Name = ("IP Address"))]
        public string ScannerDeviceIPAddressMod { get; set; } = null!;
        [Display(Name = ("Label"))]
        public string ScannerLabelMod { get; set; } = null!;

        //Navigation properties
        public RoomLocationInfoModel? Room { get; set; } = null!;
    }
}
