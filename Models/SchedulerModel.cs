using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SAMS.Models
{
    public class SchedulerModel
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Name of event")]
        public string NameOfEvent { get; set; } = null!;
        [Display(Name = "Date of event")]
        public DateOnly Date {  get; set; }

        public enum Types
        {
            [Display(Name = "Semester 1 Start")]
            Semester1,
            [Display(Name = "Semester 2 Start")]
            Semester2,
            [Display(Name = "No School for students @SHS")]
            NoSchool
        }
        [Display(Name = "Type of event")]
        public Types Type { get; set; }
    }
}
