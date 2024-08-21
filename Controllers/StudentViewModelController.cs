using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SAMS.Data;
using System.Text.RegularExpressions;

namespace SAMS.Controllers
{
    public class StudentViewModelController(ApplicationDbContext context) : Controller
    {

        private readonly ApplicationDbContext _context = context;

        public IActionResult StudentInfo(int id)
        {
            var student = _context.StudentInfoModels.Where(a => a.StudentID == id).Include(s => s.Counselor).Include(s => s.BellAttendances).Include(s => s.DailyAttendances).Include(s => s.Sem1StudSchedule).Include(s => s.Sem2StudSchedule).Include(s => s.HallPasses).First();
            ViewBag.studentId = id;
            ViewBag.studentName = $"{student.StudentFirstNameMod} {student.StudentLastNameMod}";
            ViewBag.studentEmail = student.StudentEmailMod;
            ViewBag.counselorName = $"{student.Counselor!.CounselorFirstName} {student.Counselor.CounselorLastName}";
            ViewBag.counselorEmail = student.Counselor.CounselorEmail;

            //Semester 1 Lists
            List<int> Sem1CourseIDsMonWed = [student.Sem1StudSchedule.Bell1CourseIDMod, student.Sem1StudSchedule.Bell2MonWedCourseIDMod, student.Sem1StudSchedule.AvesBellCourseIDMod, student.Sem1StudSchedule.Bell3MonWedCourseIDMod, student.Sem1StudSchedule.Bell4MonWedCourseIDMod, student.Sem1StudSchedule.Bell5MonWedCourseIDMod, student.Sem1StudSchedule.LunchCodeMod, student.Sem1StudSchedule.Bell6MonWedCourseIDMod, student.Sem1StudSchedule.Bell7MonWedCourseIDMod];

            List<int> Sem1CourseIDsTuesThurs = [student.Sem1StudSchedule.Bell1CourseIDMod, student.Sem1StudSchedule.Bell2TueThurCourseIDMod, student.Sem1StudSchedule.AvesBellCourseIDMod, student.Sem1StudSchedule.Bell3TueThurCourseIDMod, student.Sem1StudSchedule.Bell4TueThurCourseIDMod, student.Sem1StudSchedule.Bell5TueThurCourseIDMod, student.Sem1StudSchedule.LunchCodeMod, student.Sem1StudSchedule.Bell6TueThurCourseIDMod, student.Sem1StudSchedule.Bell7TueThurCourseIDMod];

            List<int> Sem1CourseIDsFriday = [student.Sem1StudSchedule.FriBell2CourseIDMod, student.Sem1StudSchedule.FriBell3CourseIDMod, student.Sem1StudSchedule.FriBell4CourseIDMod, student.Sem1StudSchedule.FriBell5CourseIDMod, student.Sem1StudSchedule.FriBell6CourseIDMod, student.Sem1StudSchedule.FriBell7CourseIDMod];

            //Semester 2 Lists
            List<int> Sem2CourseIDsMonWed = [student.Sem2StudSchedule.Bell1CourseIDMod, student.Sem2StudSchedule.Bell2MonWedCourseIDMod, student.Sem2StudSchedule.AvesBellCourseIDMod, student.Sem2StudSchedule.Bell3MonWedCourseIDMod, student.Sem2StudSchedule.Bell4MonWedCourseIDMod, student.Sem2StudSchedule.Bell5MonWedCourseIDMod, student.Sem2StudSchedule.LunchCodeMod, student.Sem2StudSchedule.Bell6MonWedCourseIDMod, student.Sem2StudSchedule.Bell7MonWedCourseIDMod];

            List<int> Sem2CourseIDsTuesThurs = [student.Sem2StudSchedule.Bell1CourseIDMod, student.Sem2StudSchedule.Bell2TueThurCourseIDMod, student.Sem2StudSchedule.AvesBellCourseIDMod, student.Sem2StudSchedule.Bell3TueThurCourseIDMod, student.Sem2StudSchedule.Bell4TueThurCourseIDMod, student.Sem2StudSchedule.Bell5TueThurCourseIDMod, student.Sem2StudSchedule.LunchCodeMod, student.Sem2StudSchedule.Bell6TueThurCourseIDMod, student.Sem2StudSchedule.Bell7TueThurCourseIDMod];

            List<int> Sem2CourseIDsFriday = [student.Sem2StudSchedule.FriBell2CourseIDMod, student.Sem2StudSchedule.FriBell3CourseIDMod, student.Sem2StudSchedule.FriBell4CourseIDMod, student.Sem2StudSchedule.FriBell5CourseIDMod, student.Sem2StudSchedule.FriBell6CourseIDMod, student.Sem2StudSchedule.FriBell7CourseIDMod];

            //CourseName Lists
            List<string> Sem1CourseNamesMonWed = [];

            List<string> Sem1CourseNamesTuesThurs = [];

            List<string> Sem1CourseNamesFriday = [];

            List<string> Sem2CourseNamesMonWed = [];

            List<string> Sem2CourseNamesTuesThurs = [];

            List<string> Sem2CourseNamesFriday = [];

            foreach (var item in Sem1CourseIDsMonWed)
            {
                Sem1CourseNamesMonWed.Add(_context.ActiveCourseInfoModels.Where(a => a.CourseId == item).Select(b => b.CourseName).FirstOrDefault() ?? string.Empty);
            }

            foreach (var item in Sem1CourseIDsTuesThurs)
            {
                Sem1CourseNamesTuesThurs.Add(_context.ActiveCourseInfoModels.Where(a => a.CourseId == item).Select(b => b.CourseName).FirstOrDefault() ?? string.Empty);
            }

            foreach (var item in Sem1CourseIDsFriday)
            {
                Sem1CourseNamesFriday.Add(_context.ActiveCourseInfoModels.Where(a => a.CourseId == item).Select(b => b.CourseName).FirstOrDefault() ?? string.Empty);
            }

            foreach (var item in Sem2CourseIDsMonWed)
            {
                Sem2CourseNamesMonWed.Add(_context.ActiveCourseInfoModels.Where(a => a.CourseId == item).Select(b => b.CourseName).FirstOrDefault() ?? string.Empty);
            }

            foreach (var item in Sem2CourseIDsTuesThurs)
            {
                Sem2CourseNamesTuesThurs.Add(_context.ActiveCourseInfoModels.Where(a => a.CourseId == item).Select(b => b.CourseName).FirstOrDefault() ?? string.Empty);
            }

            foreach (var item in Sem2CourseIDsFriday)
            {
                Sem2CourseNamesFriday.Add(_context.ActiveCourseInfoModels.Where(a => a.CourseId == item).Select(b => b.CourseName).FirstOrDefault() ?? string.Empty);
            }

            ViewBag.Sem1MonWed = Sem1CourseNamesMonWed;
            ViewBag.Sem1TuesThurs = Sem1CourseNamesTuesThurs;
            ViewBag.Sem1Fri = Sem1CourseNamesFriday;
            ViewBag.Sem2MonWed = Sem2CourseNamesMonWed;
            ViewBag.Sem2TuesThurs = Sem2CourseNamesTuesThurs;
            ViewBag.Sem2Fri = Sem2CourseNamesFriday;

            return View("StudentInfo", student);
        }

        public IActionResult StudentDailyAttendanceSearch(DateOnly Date, int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var query = _context.DailyAttendanceModels.Where(a => a.StudentId == id).Where(a => a.AttendanceDate == Date).ToList();

            List<List<string>> something = [];
            foreach (var item in query)
            {
                List<string> something2 = [];
                something2.Add(item.Status);
                something2.Add(item.ReasonForAbsence);
                something2.Add(item.ChosenBellSchedule);
                something2.Add(item.AttendanceId.ToString());
                something.Add(something2);
            }
            return Json(something);

        }

        public IActionResult StudentBellAttendanceSearch(DateTime Date, int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var query = _context.BellAttendanceModels.Where(a => a.StudentId == id).Where(a => a.DateTime.Date == Date.Date).ToList();
            List<List<string>> something = [];
            foreach (var item in query)
            {
                List<string> something2 = [];
                something2.Add(item.BellNumId);
                something2.Add(item.Status);
                var courseID = item.CourseId;
                var courseName = _context.ActiveCourseInfoModels.Where(a => a.CourseId == courseID).First().CourseName;
                something2.Add(courseName);
                something2.Add(item.ReasonForAbsence);
                something2.Add(item.ChosenBellSchedule);
                something2.Add(item.BellAttendanceId.ToString());
                something.Add(something2);
            }
            return Json(something);
        }

        public IActionResult StudentLocation(DateTime Date, int id)
        {
            if (id == null)
            {
                return NotFound();
            }

#pragma warning disable CA1305 // Specify IFormatProvider
            var query = _context.TimestampModels.Where(a => a.ActionMade == "Student Location Update").Where(b => b.Comments.Contains(id.ToString())).Where(c => c.Timestamp.Date == Date.Date).ToList();
#pragma warning restore CA1305 // Specify IFormatProvider

            List<string> studID = [];
            List<string> oldLocation = [];
            List<string> newLocation = [];
            List<string> timestamp = [];
            List<List<string>> results = [];


            string pattern = @"Student\s+(.*?)\s+scanned.*?from\s+(.*?)\s+to\s+(.*)";

            foreach (var item in query)
            {
                Match match = Regex.Match(item.Comments, pattern);

                if (match.Success)
                {
#pragma warning disable CA1305 // Specify IFormatProvider
                    if (match.Groups[1].Value != id.ToString())
                    {
                        //do nothing
                    }
                    else
                    {
                        List<string> temp = [];

                        temp.Add(match.Groups[2].Value);
                        temp.Add(match.Groups[3].Value);
#pragma warning disable CA1305 // Specify IFormatProvider
                        temp.Add(item.Timestamp.ToString());
#pragma warning restore CA1305 // Specify IFormatProvider

                        results.Add(temp);
                    }
#pragma warning restore CA1305 // Specify IFormatProvider
                }
            }


            return Json(results);
        }

        [HttpPost]
        public IActionResult UpdateBellAttendanceStatus(int id, string status, string reason)
        {
            var query = _context.BellAttendanceModels.FirstOrDefault(a => a.BellAttendanceId == id);
            if (query == null)
            {
                return NotFound();
            }
            query.Status = status;
            query.ReasonForAbsence = reason;
            _context.SaveChanges();

            return Json(status);
        }

        [HttpPost]
        public IActionResult UpdateDailyAttendanceStatus(int id, string status, string reason)
        {
            var query = _context.DailyAttendanceModels.FirstOrDefault(a => a.AttendanceId == id);
            if (query == null)
            {
                return NotFound();
            }
            query.Status = status;
            query.ReasonForAbsence = reason;
            _context.SaveChanges();

            return Json(status);
        }
    }
}