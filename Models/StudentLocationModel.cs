using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAMS.Models
{
    public class StudentLocationModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int StudentIdMod { get; set; }
        public string? StudentName { get; set;}
        public string? StudentLocation {  get; set; }

        //Navigation properties
        public StudentInfoModel Student { get; set; } = default!;
    }
}
