using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using System.ComponentModel.DataAnnotations;

namespace SAMS.Models
{
    [Keyless]
    public class StudentLocationModel
    {
        public int StudentIdMod { get; set; }
        public string? StudentName { get; set;}
        public string? StudentLocation {  get; set; }

        //Navigation properties
        //public StudentInfoModel Student { get; set; } = default!;
    }
}
