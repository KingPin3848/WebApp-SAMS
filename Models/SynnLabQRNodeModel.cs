using System.ComponentModel.DataAnnotations;

namespace SAMS.Models
{
    public class SynnLabQRNodeModel
    {
        [Key]
        public string ScannerID { get; set; } = null!;
        public int SynnlabRoomIDMod { get; set; } = 0!;
        public string ScannerMacAddressMod { get; set; } = null!;
        public string ModelNumberMod { get; set; } = null!;
        public string ScannerDeviceIPAddressMod { get; set; } = null!;
        public string ScannerLabelMod { get; set; } = null!;

        //Navigation properties
        public RoomLocationInfoModel? Room {  get; set; }
    }
}
