using System.ComponentModel.DataAnnotations;

namespace SAMS.Models
{
    public class HandheldScannerNodeModel
    {
        [Key]
        [Display(Name = ("Scanner ID"))]
        public int ScannerID { get; set; } = default!;

        [Display(Name = ("Room ID"))]
        public int RoomIDMod { get; set; } = default!;

        [Display(Name = ("Serial Number"))]
        public int SerialNumberMod { get; set; } = default!;

        //Navigation properties
        public RoomLocationInfoModel? Room { get; set; } = null!;
    }
}
