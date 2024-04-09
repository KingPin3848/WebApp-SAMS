using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using SAMS.Controllers;
using SAMS.Data;
using SAMS.Interfaces;
using SAMS.Models;

namespace SAMS.Services
{
    public class DailyAttendanceAdditionService : BackgroundService
    {
        private readonly ILogger<DailyAttendanceAdditionService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public DailyAttendanceAdditionService(ILogger<DailyAttendanceAdditionService> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await holidayRun();

                //await GenerateAttendanceFieldsDailyAttTask();
                //await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
            }
        }

        private async Task holidayRun()
        {
            using (var scope = _scopeFactory.CreateAsyncScope())
            {
                var _context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                var holidayDates = _context.schedulerModels.Where(a => a.Type == "No School @SHS").Select(a => a.Date).ToList();
                var todayDate = DateOnly.FromDateTime(DateTime.Now.Date);

                if (holidayDates == null)
                {
                    _logger.LogWarning("Holidays is null and the task if delayed by 1 DAY. Done by the if statement in holidayRun");
                    await Task.Delay(TimeSpan.FromDays(1));
                }
                else
                {
                    foreach (var date in holidayDates)
                    {
                        if (date == todayDate)
                        {
                            _logger.LogWarning("Today is a holiday and the task is delayed by 1 DAY. Done by the if statement in holidayRun");
                            await Task.Delay(TimeSpan.FromDays(1));
                        }
                        else
                        {
                            await ScheduleRunner();
                        }
                    }
                }
            }
        }

        private async Task ScheduleRunner()
        {
            using (var scope = _scopeFactory.CreateAsyncScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                var chosenBellSched = context.chosenBellSchedModels.Select(a => a.Name).ToList();
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
                                await Task.Delay(TimeSpan.FromMinutes(2.0));
                                break;
                            }

                        case "Pep Rally Bell Schedule":
                            {
                                if (time >= peprallyStart && time <= new TimeSpan(23, 59, 00))
                                {
                                    await GenerateAttendanceFieldsDailyAttTask();
                                    break;
                                }
                                await Task.Delay(TimeSpan.FromMinutes(2.0));
                                break;
                            }

                        case "2 Hour Delay Bell Schedule":
                            {
                                if (time >= _2hrdelStart && time <= new TimeSpan(09, 20, 00))
                                {
                                    await GenerateAttendanceFieldsDailyAttTask();
                                    break;
                                }
                                await Task.Delay(TimeSpan.FromMinutes(2.0));
                                break;
                            }

                        case "Extended Aves Bell Schedule":
                            {
                                if (time >= extAvesStart && time <= new TimeSpan(07, 20, 00))
                                {
                                    await GenerateAttendanceFieldsDailyAttTask();
                                    break;
                                }
                                await Task.Delay(TimeSpan.FromMinutes(2.0));
                                break;
                            }

                        default:
                            {
                                _logger.LogInformation("The task is supposed to be delayed for 1 DAY. Done by default case in ScheduleRunner");
                                await Task.Delay(TimeSpan.FromDays(1));
                                break;
                            }
                    }
                }
                else
                {
                    _logger.LogWarning("The task is going to be delayed for 1 DAY. Done the by the else statement @line 136 in ScheduleRunner.");
                    await Task.Delay(TimeSpan.FromDays(1));
                }
            }
        }

        private async Task GenerateAttendanceFieldsDailyAttTask()
        {
            using (var scope = _scopeFactory.CreateAsyncScope())
            {
                var _userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var _context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                var date = DateOnly.FromDateTime(DateTime.Now);

                var students = await _userManager.GetUsersInRoleAsync("Student");
                var noncheckDailyCourses = _context.activeCourseInfoModels.Where(a => a.DailyAttChecked == false).ToList();

                foreach (var student in students)
                {
                    int studentId = int.Parse(student.SchoolId!);

                    for (int bell = 0; bell <= 7; bell++)
                    {
                        var sem2start = _context.schedulerModels.Where(a => a.Type == "Semester 2").Select(a => a.Date).FirstOrDefault();
                        IStudentSchedule? studentSchedule = (DateOnly.FromDateTime(DateTime.Now.Date) >= sem2start) ? (await _context.sem2StudSchedules.FindAsync(studentId)) : (await _context.sem1StudSchedules.FindAsync(studentId));
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

                        if (noncheckDailyCourses.Any(course => course.CourseId == bellCourseId))
                        {
                            // If the course for this bell is in noncheckDailyCourses, skip to the next student
                            _logger.LogInformation("Course for this bell is in the noncheckDailyCourses.");
                        }
                        else
                        {
                            var entryExists = _context.dailyAttendanceModels.Any(a =>
                                a.StudentId == studentId &&
                                a.AttendanceDate == date);

                            if (entryExists)
                            {
                                _logger.LogInformation("Entry exists.");
                                continue;
                            }
                            else
                            {
                                var chosenBellSched = _context.chosenBellSchedModels.Select(a => a.Name).ToList();
                                //Add new entry if entry doesn't exist
                                var newEntry = new DailyAttendanceModel
                                {
                                    StudentId = studentId,
                                    AttendanceDate = date,
                                    Status = "Unknown",
                                    ReasonForAbsence = "Not Applicable Yet",
                                    ChosenBellSchedule = chosenBellSched[0]!
                                };

                                var timestamp = new TimestampModel
                                {
                                    Timestamp = DateTime.Now,
                                    ActionMade = $"Daily attendance default added for student ID {studentId} for Date: {DateTime.Now}",
                                    MadeBy = "Daily Att. Addition Service",
                                    Comments = ""
                                };

                                //Restart numbering
                                var rawSqlString = "DBCC CHECKIDENT ('dailyAttendanceModels', RESEED, 0);";
                                _context.Database.ExecuteSqlRaw(rawSqlString);

                                _context.dailyAttendanceModels.Add(newEntry);
                                _context.timestampModels.Add(timestamp);
                                await _context.SaveChangesAsync();

                            }
                        }
                    }
                }
            }
            await Task.Delay(TimeSpan.FromMinutes(5));
            await Task.CompletedTask;
        }

        private int GetS1BellCourseId(IStudentSchedule studentSchedule, int bell)
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

        private int GetS2BellCourseId(IStudentSchedule studentSchedule, int bell)
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
