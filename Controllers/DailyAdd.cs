using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SAMS.Data;
using SAMS.Models;
using SAMS.Services;

namespace SAMS.Controllers
{
    public class DailyAdd(ILogger<DailyAdd> logger, IServiceScopeFactory scopeFactory) : Controller
    {
        private readonly ILogger<DailyAdd> _logger = logger;
        private readonly IServiceScopeFactory _scopeFactory = scopeFactory;

        [HttpGet]
        public IActionResult Index()
        {
            HolidayRun();
            return View();
        }

        private void HolidayRun()
        {
            using var scope = _scopeFactory.CreateAsyncScope();
            var _userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var _context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var _roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var holidayDates = _context.SchedulerModels.Where(a => a.Type == SchedulerModel.Types.NoSchool).Select(a => a.Date).ToList();
            var todayDate = DateOnly.FromDateTime(DateTime.Now.Date);

            if (holidayDates == null)
            {
                _logger.LogWarning("Holidays is null and the task if delayed by 1 DAY. Done by the if statement in holidayRun");
                //Task.Delay(TimeSpan.FromDays(1));
            }
            else
            {
                foreach (var date in holidayDates)
                {
                    if (date == todayDate)
                    {
                        _logger.LogWarning("Today is a holiday and the task is delayed by 1 DAY. Done by the if statement in holidayRun");
                        //await Task.Delay(TimeSpan.FromDays(1));
                    }
                    else
                    {
                        ScheduleRunner();
                    }
                }
            }

        }

        private void ScheduleRunner()
        {
            using var scope = _scopeFactory.CreateAsyncScope();

            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var chosenBellSched = context.ChosenBellSchedModels.Select(a => a.Name).First();
            var dateTime = DateTime.Now;
            var day = dateTime.DayOfWeek;

            if (day == DayOfWeek.Monday || day == DayOfWeek.Tuesday || day == DayOfWeek.Wednesday || day == DayOfWeek.Thursday || day == DayOfWeek.Friday)
            {
                var time = dateTime.TimeOfDay;
                TimeSpan dailyBellStart = new(7, 15, 00);
                TimeSpan peprallyStart = new(7, 15, 00);
                TimeSpan _2hrdelStart = new(9, 15, 00);
                TimeSpan extAvesStart = new(7, 15, 00);

                switch (chosenBellSched)
                {
                    case "Daily Bell Schedule":
                        {
                            if (time >= dailyBellStart && time <= new TimeSpan(23, 59, 00))
                            {
                                GenerateAttendanceFieldsDailyAttTask();
                                break;
                            }
                            break;
                        }

                    case "Pep Rally Bell Schedule":
                        {
                            if (time >= peprallyStart && time <= new TimeSpan(07, 20, 00))
                            {
                                GenerateAttendanceFieldsDailyAttTask();
                                break;
                            }
                            break;
                        }

                    case "2 Hour Delay Bell Schedule":
                        {
                            if (time >= _2hrdelStart && time <= new TimeSpan(09, 20, 00))
                            {
                                GenerateAttendanceFieldsDailyAttTask();
                                break;
                            }
                            break;
                        }

                    case "Extended Aves Bell Schedule":
                        {
                            if (time >= extAvesStart && time <= new TimeSpan(07, 20, 00))
                            {
                                GenerateAttendanceFieldsDailyAttTask();
                                break;
                            }
                            break;
                        }
                    default:
                        {
                            _logger.LogWarning("The task is supposed to be delayed for 1 DAY. Done by default case in ScheduleRunner");
                            //await Task.Delay(TimeSpan.FromDays(1), token);
                            break;
                        }
                }
            }
            else
            {
                _logger.LogWarning("The task is going to be delayed for 1 DAY. Done the by the else statement @line 126 in ScheduleRunner.");
                //await Task.Delay(TimeSpan.FromDays(1), token);
            }
        }

        private async void GenerateAttendanceFieldsDailyAttTask()
        {
            using var scope = _scopeFactory.CreateAsyncScope();
            var _userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var _context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var _roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var date = DateOnly.FromDateTime(DateTime.Now.Date);

            var students = await _userManager.GetUsersInRoleAsync("Student");
            var noncheckDailyCourses = _context.ActiveCourseInfoModels.Where(a => a.DailyAttChecked == false).ToList();

            foreach (var student in students)
            {
                int studentId = int.Parse(student.SchoolId);

                for (int bell = 0; bell <= 7; bell++)
                {
                    var sem2start = _context.SchedulerModels.Where(a => a.Type == SchedulerModel.Types.Semester2).Select(a => a.Date).FirstOrDefault();
                    int bellCourseId;
                    if (DateOnly.FromDateTime(DateTime.Now.Date) >= sem2start)
                    {
                        var studentSchedule = await _context.Sem2StudSchedules.FindAsync(studentId);
                        bellCourseId = GetS2BellCourseId(studentSchedule!, bell);
                    }
                    else
                    {
                        var studentSchedule = await _context.Sem1StudSchedules.FindAsync(studentId);
                        bellCourseId = GetS1BellCourseId(studentSchedule!, bell);
                    }
                    var matchingCourse = noncheckDailyCourses.Any(course => course.CourseId == bellCourseId);

                    if (matchingCourse == true)
                    {
                        // If the course for this bell is in noncheckDailyCourses, skip to the next student
                        _logger.LogInformation("Course for this bell is in the noncheckDailyCourses.");
                    }
                    else
                    {
                        var entryExists = _context.DailyAttendanceModels.Any(a =>
                            a.StudentId == studentId &&
                            a.AttendanceDate == date);

                        if (!entryExists)
                        {
                            var chosenBellSched = _context.ChosenBellSchedModels.Select(a => a.Name).ToList();
                            //Add new entry if entry doesn't exist
                            var newEntry = new DailyAttendanceModel
                            {
                                StudentId = studentId,
                                AttendanceDate = date,
                                Status = "Unknown",
                                ReasonForAbsence = "NA",
                                ChosenBellSchedule = chosenBellSched.ToString()
                            };
                            _context.DailyAttendanceModels.Add(newEntry);
                            await _context.SaveChangesAsync();
                        }
                    }
                }
            }
            //await Task.CompletedTask;
        }

        private static int GetS1BellCourseId(Sem1StudSchedule studentSchedule, int bell)
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

        private static int GetS2BellCourseId(Sem2StudSchedule studentSchedule, int bell)
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
}
