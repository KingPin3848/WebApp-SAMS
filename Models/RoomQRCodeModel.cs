using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SAMS.Models
{
    public class RoomQRCodeModel
    {
        [Key]
        public int RoomId { get; set; }
        public string Code { get; set; } = null!;

        //Navigation Properties
        public RoomLocationInfoModel? Room { get; set; } = null!;
    }
}
