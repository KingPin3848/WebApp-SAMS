using Microsoft.Extensions.Primitives;
using System.ComponentModel.DataAnnotations;

namespace SAMS.Models
{
    public class StudentLocationModel
    {
        [Key]
        public int StudentId { get; set; }
        public string? StudentName { get; set;}
        public string? StudentLocation {  get; set; }
    }
}
