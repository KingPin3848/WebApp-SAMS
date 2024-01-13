using System.ComponentModel.DataAnnotations;

namespace SAMS.Models
{
    public class ActivationModel
    {
        [Key]
        [Display(Name = "Code ID")]
        public int StudId { get; set; } = 0!;
        [Display(Name = "Activation Code")]
        public string Code { get; set; } = string.Empty;

        //Navigation properties
        public StudentInfoModel? Student { get; set; }
    }
}
