using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SAMS.Data;
using SAMS.Interfaces;
using SAMS.Models;
using System.ComponentModel.DataAnnotations;

namespace SAMS.Areas.Teacher.Controllers
{
    [Area("Teacher")]
    [Authorize(Roles = "Teacher")]
    public class TeacherRoster(IServiceScopeFactory serviceScopeFactory) : Controller
    {
        private readonly IServiceScopeFactory ScopeFactory = serviceScopeFactory;

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Roster(int bell)
        {
            using var scope = ScopeFactory.CreateAsyncScope();
            var _context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var _userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            var user = await _userManager.GetUserAsync(User).ConfigureAwait(true);
            if (user == null)
            {
                return NotFound();
            }

            var activeCourses = _context.ActiveCourseInfoModels.Where(a => a.CourseTeacherID == user.SchoolId).ToList();

            var studentUsers = await _userManager.GetUsersInRoleAsync("Student").ConfigureAwait(true);

            List<OutputModel> studentsInBell = [];

            foreach (var studentUser in studentUsers)
            {
                if (int.TryParse(studentUser.SchoolId, out int studentID))
                {
                    //Do nothing
                }
                else
                {
                    return NotFound();
                }

                var sem2start = _context.SchedulerModels.Where(a => a.Type == SchedulerModel.Types.Semester2).Select(a => a.Date).FirstOrDefault();
                IStudentSchedule? studentSchedule = (DateOnly.FromDateTime(DateTime.Now.Date) >= sem2start) ? (await _context.Sem2StudSchedules.FindAsync(studentID).ConfigureAwait(true)) : (await _context.Sem1StudSchedules.FindAsync(studentID).ConfigureAwait(true));
                var sem2started = (DateOnly.FromDateTime(DateTime.Now.Date) >= sem2start);
                int bellCourseId;

                if (sem2started)
                {
                    bellCourseId = GetS2BellCourseId(studentSchedule!, bell);
                }
                else
                {
                    bellCourseId = GetS1BellCourseId(studentSchedule!, bell);
                }

                foreach (var course in activeCourses)
                {
                    if (course.CourseId == bellCourseId)
                    {
                        var wholestudent = _context.StudentInfoModels.Where(a => a.StudentID == studentID).First();
                        DateOnly date = DateOnly.FromDateTime(DateTime.Now.Date);
                        var details = new OutputModel()
                        {
                            studentId = studentID,
                            studentFirstAndLastName = $"{wholestudent.StudentFirstNameMod} {wholestudent.StudentLastNameMod}",
                            studentEmail = wholestudent.StudentEmailMod,
                            DailyAttStatus = _context.DailyAttendanceModels.Where(a => a.AttendanceDate == date).First().Status,
                            BellAttStatus = _context.BellAttendanceModels.Where(a => a.DateTime.Date == DateTime.Now.Date).Where(b => b.BellNumId == $"Bell {bell}").First().Status
                        };
                        studentsInBell.Add(details);
                    }
                }
            }
            return View(studentsInBell);
        }

        private static int GetS1BellCourseId(IStudentSchedule studentSchedule, int bell)
        {
            switch (bell)
            {
                case 0:
                    return studentSchedule.AvesBellCourseIDMod;
                case 1:
                    return studentSchedule.Bell1CourseIDMod;
                case 2:
                    if (DateTime.Now.Date.DayOfWeek == DayOfWeek.Monday || DateTime.Now.Date.DayOfWeek == DayOfWeek.Wednesday)
                    {
                        return studentSchedule.Bell2MonWedCourseIDMod;
                    }
                    else if (DateTime.Now.Date.DayOfWeek == DayOfWeek.Friday)
                    {
                        return studentSchedule.FriBell2CourseIDMod;
                    }
                    return studentSchedule.Bell2TueThurCourseIDMod;
                case 3:
                    if (DateTime.Now.Date.DayOfWeek == DayOfWeek.Monday || DateTime.Now.Date.DayOfWeek == DayOfWeek.Wednesday)
                    {
                        return studentSchedule.Bell3MonWedCourseIDMod;
                    }
                    else if (DateTime.Now.Date.DayOfWeek == DayOfWeek.Friday)
                    {
                        return studentSchedule.FriBell3CourseIDMod;
                    }
                    return studentSchedule.Bell3TueThurCourseIDMod;
                case 4:
                    if (DateTime.Now.Date.DayOfWeek == DayOfWeek.Monday || DateTime.Now.Date.DayOfWeek == DayOfWeek.Wednesday)
                    {
                        return studentSchedule.Bell4MonWedCourseIDMod;
                    }
                    else if (DateTime.Now.Date.DayOfWeek == DayOfWeek.Friday)
                    {
                        return studentSchedule.FriBell4CourseIDMod;
                    }
                    return studentSchedule.Bell4TueThurCourseIDMod;
                case 5:
                    if (DateTime.Now.Date.DayOfWeek == DayOfWeek.Monday || DateTime.Now.Date.DayOfWeek == DayOfWeek.Wednesday)
                    {
                        return studentSchedule.Bell5MonWedCourseIDMod;
                    }
                    else if (DateTime.Now.Date.DayOfWeek == DayOfWeek.Friday)
                    {
                        return studentSchedule.FriBell5CourseIDMod;
                    }
                    return studentSchedule.Bell5TueThurCourseIDMod;
                case 6:
                    if (DateTime.Now.Date.DayOfWeek == DayOfWeek.Monday || DateTime.Now.Date.DayOfWeek == DayOfWeek.Wednesday)
                    {
                        return studentSchedule.Bell6MonWedCourseIDMod;
                    }
                    else if (DateTime.Now.Date.DayOfWeek == DayOfWeek.Friday)
                    {
                        return studentSchedule.FriBell6CourseIDMod;
                    }
                    return studentSchedule.Bell6TueThurCourseIDMod;
                case 7:
                    if (DateTime.Now.Date.DayOfWeek == DayOfWeek.Monday || DateTime.Now.Date.DayOfWeek == DayOfWeek.Wednesday)
                    {
                        return studentSchedule.Bell7MonWedCourseIDMod;
                    }
                    else if (DateTime.Now.Date.DayOfWeek == DayOfWeek.Friday)
                    {
                        return studentSchedule.FriBell7CourseIDMod;
                    }
                    return studentSchedule.Bell7TueThurCourseIDMod;
                default:
                    {
                        throw new ArgumentOutOfRangeException(nameof(bell), "Invalid bell name provided.");
                    }
            }
        }

        private static int GetS2BellCourseId(IStudentSchedule studentSchedule, int bell)
        {
            switch (bell)
            {
                case 0:
                    return studentSchedule.AvesBellCourseIDMod;
                case 1:
                    return studentSchedule.Bell1CourseIDMod;
                case 2:
                    if (DateTime.Now.Date.DayOfWeek == DayOfWeek.Monday || DateTime.Now.Date.DayOfWeek == DayOfWeek.Wednesday)
                    {
                        return studentSchedule.Bell2MonWedCourseIDMod;
                    }
                    else if (DateTime.Now.Date.DayOfWeek == DayOfWeek.Friday)
                    {
                        return studentSchedule.FriBell2CourseIDMod;
                    }
                    return studentSchedule.Bell2TueThurCourseIDMod;
                case 3:
                    if (DateTime.Now.Date.DayOfWeek == DayOfWeek.Monday || DateTime.Now.Date.DayOfWeek == DayOfWeek.Wednesday)
                    {
                        return studentSchedule.Bell3MonWedCourseIDMod;
                    }
                    else if (DateTime.Now.Date.DayOfWeek == DayOfWeek.Friday)
                    {
                        return studentSchedule.FriBell3CourseIDMod;
                    }
                    return studentSchedule.Bell3TueThurCourseIDMod;
                case 4:
                    if (DateTime.Now.Date.DayOfWeek == DayOfWeek.Monday || DateTime.Now.Date.DayOfWeek == DayOfWeek.Wednesday)
                    {
                        return studentSchedule.Bell4MonWedCourseIDMod;
                    }
                    else if (DateTime.Now.Date.DayOfWeek == DayOfWeek.Friday)
                    {
                        return studentSchedule.FriBell4CourseIDMod;
                    }
                    return studentSchedule.Bell4TueThurCourseIDMod;
                case 5:
                    if (DateTime.Now.Date.DayOfWeek == DayOfWeek.Monday || DateTime.Now.Date.DayOfWeek == DayOfWeek.Wednesday)
                    {
                        return studentSchedule.Bell5MonWedCourseIDMod;
                    }
                    else if (DateTime.Now.Date.DayOfWeek == DayOfWeek.Friday)
                    {
                        return studentSchedule.FriBell5CourseIDMod;
                    }
                    return studentSchedule.Bell5TueThurCourseIDMod;
                case 6:
                    if (DateTime.Now.Date.DayOfWeek == DayOfWeek.Monday || DateTime.Now.Date.DayOfWeek == DayOfWeek.Wednesday)
                    {
                        return studentSchedule.Bell6MonWedCourseIDMod;
                    }
                    else if (DateTime.Now.Date.DayOfWeek == DayOfWeek.Friday)
                    {
                        return studentSchedule.FriBell6CourseIDMod;
                    }
                    return studentSchedule.Bell6TueThurCourseIDMod;
                case 7:
                    if (DateTime.Now.Date.DayOfWeek == DayOfWeek.Monday || DateTime.Now.Date.DayOfWeek == DayOfWeek.Wednesday)
                    {
                        return studentSchedule.Bell7MonWedCourseIDMod;
                    }
                    else if (DateTime.Now.Date.DayOfWeek == DayOfWeek.Friday)
                    {
                        return studentSchedule.FriBell7CourseIDMod;
                    }
                    return studentSchedule.Bell7TueThurCourseIDMod;
                default:
                    {
                        throw new ArgumentOutOfRangeException(nameof(bell), "Invalid bell name provided.");
                    }
            }
        }
    }

    public class OutputModel()
    {
        [Display(Name = "Student ID")]
        public required int studentId { get; set; } = 0;
        [Display(Name = "Name (F/L)")]
        public required string studentFirstAndLastName { get; set; } = string.Empty;
        [Display(Name = "School Email Address")]
        public required string studentEmail { get; set; } = string.Empty;
        [Display(Name = "Daily Attendance Status")]
        public required string DailyAttStatus { get; set; } = string.Empty;
        [Display(Name = "Bell Attendance Status")]
        public required string BellAttStatus { get; set; } = string.Empty;
    }
}
