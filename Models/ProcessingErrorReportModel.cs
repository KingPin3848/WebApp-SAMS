using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAMS.Models
{
    public class ProcessingErrorReportModel
    {
        [Key]
        [Display(Name = "Report Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReportId { get; set; }

        [Display(Name = "Reported Number")]
        [Required]
        public required int Number { get; set; }

        [Display(Name = "Developer Reference")]
        [Required]
        public required string DeveloperReference { get; set; }

        [Display(Name = "Description")]
        [Required]
        public required string Description { get; set; }

        [Display(Name = "Reported By")]
        [Required]
        public required string UserId { get; set; }
    }
}
