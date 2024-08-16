using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAMS.Models
{

    public class ReportModel
    {
        [Key]
        [Display(Name = "Report Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReportId { get; set; }

        public enum ErrorType
        {
            [Display(Name = "Attendance Status Error")]
            AttendanceStatusError,

            [Display(Name = "Attendance Scanning Error ")]
            AttendanceScanningError,

            [Display(Name = "Hall-Pass Error")]
            HallPassError,

            [Display(Name = "Student Location Error")]
            StudentLocationError,

            [Display(Name = "Processing Error - Oh eyeballs!")]
            ProcessingError,

            [Display(Name = "Bug")]
            Bug,

            [Display(Name = "System Feedback")]
            SystemFeedback
        }
        [Required]
        [Display(Name = "Type")]
        public required ErrorType TypeOfReport { get; set; }

        [Display(Name = "Reported Number")]
        public int Number { get; set; }

        [Display(Name = "Developer Reference")]
        public string DeveloperReference { get; set; } = string.Empty;

        [Display(Name = "Description")]
        [Required]
        public required string Description { get; set; }

        [Display(Name = "Reported By")]
        [Required]
        public required string UserId { get; set; }

        public enum Status
        {
            [Display(Name = "Submitted to Appropriate Personnel")]
            SubmittedToAppropriatePersonnel,
            [Display(Name = "Under Review")]
            UnderReview,
            [Display(Name = "Reviewed and Additional Information Required")]
            ReviewedAndAdditionalInformationRequired,
            [Display(Name = "Resolved and Closed")]
            Resolved
        }

        [Required]
        [Display(Name = "Status")]
        public required Status StatusOfReport { get; set; }

        public enum SeverityLevel
        {
            Low,
            Medium,
            High
        }
        [Required]
        [Display(Name = "Severity Level")]
        public required SeverityLevel Severity { get; set; }

    }
}
