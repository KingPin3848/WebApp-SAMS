using System.ComponentModel.DataAnnotations;

namespace SAMS.Models
{
    public class RoomLocationInfoModel
    {
        [Key]
        public string roomNumberMod { get; set; } = null!;
        public string wingNameMod { get; set; } = null!;
        public string roomCodeMod { get; set; } = null!;
    }
}
