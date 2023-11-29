using System.ComponentModel.DataAnnotations;

namespace SAMS.Models
{
    public class SynnLabQRNodeModel
    {
        [Key]
        public string scannerID { get; set; } = null!;
        public string synnlabRoomMod { get; set; } = null!;
        public string synnlabRoomSubject { get; set; } = null!;
        public string scannerMacAddressMod { get; set; } = null!;
        public string modelNumberMod { get; set; } = null!;
        public string scannerDeviceIPAddressMod { get; set; } = null!;
        public string scannerLabelMod { get; set; } = null!;
    }
}
