using Microsoft.AspNetCore.Identity;
using SAMS.Controllers;
using SAMS.Data;
using SAMS.Models;

namespace SAMS.Services
{
    public class DailyAttendanceAdditionService : BackgroundService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationUser> _roleManager;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DailyAttendanceAdditionService> _logger;

        public DailyAttendanceAdditionService(UserManager<ApplicationUser> userManager, RoleManager<ApplicationUser> roleManager, ApplicationDbContext dbContext, ILogger<DailyAttendanceAdditionService> logger)
        {
            _context = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var holidayDates = _context.schedulerModels.Where(a => a.Type == "No School @SHS").Select(a => a.Date).ToList();
                var todayDate = DateOnly.FromDateTime(DateTime.Now.Date);

                foreach (var date in holidayDates)
                {
                    if (date == todayDate)
                    {
                        await Task.Delay(TimeSpan.FromDays(1));
                    }
                    else
                    {
                        await ScheduleRunner(stoppingToken);
                    }
                }

                //await GenerateAttendanceFieldsDailyAttTask();
                //await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
            }
        }

        private async Task ScheduleRunner(CancellationToken token)
        {
            var chosenBellSched = _context.chosenBellSchedModels.Select(a => a.Name).ToList();
            var dateTime = DateTime.Now;
            var day = dateTime.DayOfWeek;

            if (day == DayOfWeek.Monday || day == DayOfWeek.Tuesday || day == DayOfWeek.Wednesday || day == DayOfWeek.Thursday || day == DayOfWeek.Friday)
            {
                var time = dateTime.TimeOfDay;
                TimeSpan dailyBellStart = new TimeSpan(7, 15, 00);
                TimeSpan peprallyStart = new TimeSpan(7, 15, 00);
                TimeSpan _2hrdelStart = new TimeSpan(9, 15, 00);
                TimeSpan extAvesStart = new TimeSpan(7, 15, 00);

                switch (chosenBellSched[0])
                {
                    case "Daily Bell Schedule":
                    {
                        if (time >= dailyBellStart && time <= new TimeSpan(07, 20, 00))
                        {
                            await GenerateAttendanceFieldsDailyAttTask();
                            break;
                        }
                        break;
                    }

                    case "Pep Rally Bell Schedule":
                    {
                        if (time >= peprallyStart && time <= new TimeSpan(07, 20, 00))
                        {
                            await GenerateAttendanceFieldsDailyAttTask();
                            break;
                        }
                        break;
                    }

                    case "2 Hour Delay Bell Schedule":
                    {
                        if (time >= _2hrdelStart && time <= new TimeSpan(09, 20, 00))
                        {
                            await GenerateAttendanceFieldsDailyAttTask();
                            break;
                        }
                        break;
                    }

                    case "Extended Aves Bell Schedule":
                    {
                        if (time >= extAvesStart && time <= new TimeSpan(07, 20, 00))
                        {
                            await GenerateAttendanceFieldsDailyAttTask();
                            break;
                        }
                        break;
                    }
                    default:
                    {
                        await Task.Delay(TimeSpan.FromDays(1), token);
                        break;
                    }
                }
            }
            else
            {
                await Task.Delay(TimeSpan.FromDays(1), token);
            }
        }

        private async Task GenerateAttendanceFieldsDailyAttTask()
        {
            var date = DateTime.Now.Date;
            var students = await _userManager.GetUsersInRoleAsync("Student");
            var noncheckDailyCourses = _context.activeCourseInfoModels.Where(a => a.DailyAttChecked == false).ToList();

            foreach (var student in students)
            {
                var studentId = int.Parse(student.SchoolId);

                for (int bell = 0; bell <= 7; bell++)
                {
                    var sem2start = _context.schedulerModels.Where(a => a.Type == "Semester 2").Select(a => a.Date).FirstOrDefault();
                    int bellCourseId;
                    if (DateOnly.FromDateTime(DateTime.Now.Date) >= sem2start)
                    {
                        var studentSchedule = await _context.sem2StudSchedules.FindAsync(studentId);
                        bellCourseId = GetS2BellCourseId(studentSchedule, bell);
                    }
                    else
                    {
                        var studentSchedule = await _context.sem1StudSchedules.FindAsync(studentId);
                        bellCourseId = GetS1BellCourseId(studentSchedule, bell);
                    }
                    var matchingCourse = noncheckDailyCourses.Any(course => course.CourseId == bellCourseId);

                    if (matchingCourse == true)
                    {
                        // If the course for this bell is in noncheckDailyCourses, skip to the next student
                        _logger.LogInformation("Course for this bell is in the noncheckDailyCourses.");
                    }
                    else
                    {
                        var entryExists = _context.dailyAttendanceModels.Any(a =>
                            a.StudentId == studentId &&
                            a.AttendanceDate == date);

                        if (!entryExists)
                        {
                            //Add new entry if entry doesn't exist
                            var newEntry = new DailyAttendanceModel
                            {
                                StudentId = studentId,
                                AttendanceDate = date,
                                Status = "Unknown",
                                ReasonForAbsence = "NA"
                            };
                            _context.dailyAttendanceModels.Add(newEntry);
                            await _context.SaveChangesAsync();
                        }
                    }
                }
            }


            await Task.CompletedTask;
        }

        private int GetS1BellCourseId(Sem1StudSchedule studentSchedule, int bell)
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
                    throw new ArgumentOutOfRangeException(nameof(bell), "Invalid bell name provided.");
            }
        }

        private int GetS2BellCourseId(Sem2StudSchedule studentSchedule, int bell)
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
                    throw new ArgumentOutOfRangeException(nameof(bell), "Invalid bell name provided.");
            }
        }
    }
}
