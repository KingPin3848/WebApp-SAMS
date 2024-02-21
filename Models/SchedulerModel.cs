using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SAMS.Models
{
    [Keyless]
    public class SchedulerModel
    {
        [Display(Name = "Name of event")]
        public string NameOfEvent { get; set; } = null!;
        [Display(Name = "Date of event")]
        public DateOnly Date {  get; set; }
        [Display(Name = "Type of event")]
        public string Type { get; set; } = null!;
    }
}
