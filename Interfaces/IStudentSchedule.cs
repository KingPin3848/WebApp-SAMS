using System.ComponentModel.DataAnnotations;

namespace SAMS.Interfaces
{
    public interface IStudentSchedule
    {
        public int StudentID { get; set; }
        public int Bell1CourseIDMod { get; set; }
        public int Bell2MonWedCourseIDMod { get; set; }
        public int Bell2TueThurCourseIDMod { get; set; }
        public int Bell3MonWedCourseIDMod { get; set; }
        public int Bell3TueThurCourseIDMod { get; set; }
        public int Bell4MonWedCourseIDMod { get; set; }
        public int Bell4TueThurCourseIDMod { get; set; }
        public int Bell5MonWedCourseIDMod { get; set; }
        public int Bell5TueThurCourseIDMod { get; set; }
        public int Bell6MonWedCourseIDMod { get; set; }
        public int Bell6TueThurCourseIDMod { get; set; }
        public int Bell7MonWedCourseIDMod { get; set; }
        public int Bell7TueThurCourseIDMod { get; set; }
        public int FriBell2CourseIDMod { get; set; }
        public int FriBell3CourseIDMod { get; set; }
        public int FriBell4CourseIDMod { get; set; }
        public int FriBell5CourseIDMod { get; set; }
        public int FriBell6CourseIDMod { get; set; }
        public int FriBell7CourseIDMod { get; set; }
        public int AvesBellCourseIDMod { get; set; }
        public Char LunchCodeMod { get; set; }
    }
}
