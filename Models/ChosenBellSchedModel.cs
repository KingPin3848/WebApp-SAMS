using System.ComponentModel.DataAnnotations;

namespace SAMS.Models
{
    public class ChosenBellSchedModel
    {
        [Required]
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; } = null;
    }
}
