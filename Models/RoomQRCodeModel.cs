using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SAMS.Models
{
    public class RoomQRCodeModel
    {
        [Key]
        public required int RoomId { get; set; }
        public required string Code { get; set; }

        //Navigation Properties
        public RoomLocationInfoModel? Room { get; set; } = null!;
    }
}
