using System.ComponentModel.DataAnnotations;

namespace SAMS.Models
{
    public class ActivationModel
    {
        [Key]
        public int CodeId { get; set; }
        public string Code { get; set; } = string.Empty;
        public int StudId { get; set; } = 0!;

        //Navigation properties
        public StudentInfoModel? Student { get; set; }
    }
}
