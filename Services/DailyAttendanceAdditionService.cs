using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SAMS.Data;
using SAMS.Interfaces;
using SAMS.Models;
using System.Text.RegularExpressions;

namespace SAMS.Services
{
    public class DailyAttendanceAdditionService(ILogger<DailyAttendanceAdditionService> logger, IServiceScopeFactory scopeFactory) : BackgroundService
    {
        private readonly ILogger<DailyAttendanceAdditionService> _logger = logger;
        private readonly IServiceScopeFactory _scopeFactory = scopeFactory;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await HolidayRun();

                //await GenerateAttendanceFieldsDailyAttTask();
                //await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
            }
        }

        private async Task HolidayRun()
        {
            using var scope = _scopeFactory.CreateAsyncScope();

            var _context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var holidayDates = _context.SchedulerModels.Where(a => a.Type == SchedulerModel.Types.NoSchool).Select(a => a.Date).ToList();
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

        private async Task ScheduleRunner()
        {
            using var scope = _scopeFactory.CreateAsyncScope();

            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var chosenBellSched = context.ChosenBellSchedModels.Select(a => a.Name).ToList();
            var dateTime = DateTime.Now;
            var day = dateTime.DayOfWeek;

            if (day == DayOfWeek.Monday || day == DayOfWeek.Tuesday || day == DayOfWeek.Wednesday || day == DayOfWeek.Thursday || day == DayOfWeek.Friday)
            {
                var time = dateTime.TimeOfDay;

                switch (chosenBellSched[0])
                {
                    case "Daily Bell Schedule":
                        {
                            TimeSpan dailyBellStart = new(7, 15, 00);
                            if (time >= dailyBellStart && time <= new TimeSpan(07, 20, 00))
                            {
                                await GenerateAttendanceFieldsDailyAttTask("Daily Bell Schedule");
                                break;
                            }
                            await Task.Delay(TimeSpan.FromMinutes(2.0));
                            break;
                        }

                    case "Pep Rally Bell Schedule":
                        {
                            TimeSpan peprallyStart = new(7, 15, 00);
                            if (time >= peprallyStart && time <= new TimeSpan(23, 59, 00))
                            {
                                await GenerateAttendanceFieldsDailyAttTask("Pep Rally Bell Schedule");
                                break;
                            }
                            await Task.Delay(TimeSpan.FromMinutes(2.0));
                            break;
                        }

                    case "2 Hour Delay Bell Schedule":
                        {
                            TimeSpan _2hrdelStart = new(9, 15, 00);
                            if (time >= _2hrdelStart && time <= new TimeSpan(09, 20, 00))
                            {
                                await GenerateAttendanceFieldsDailyAttTask("2 Hour Delay Bell Schedule");
                                break;
                            }
                            await Task.Delay(TimeSpan.FromMinutes(2.0));
                            break;
                        }

                    case "Extended Aves Bell Schedule":
                        {
                            TimeSpan extAvesStart = new(7, 15, 00);
                            if (time >= extAvesStart && time <= new TimeSpan(07, 20, 00))
                            {
                                await GenerateAttendanceFieldsDailyAttTask("Extended Aves Bell Schedule");
                                break;
                            }
                            await Task.Delay(TimeSpan.FromMinutes(2.0));
                            break;
                        }
                    case "Custom Bell Schedule":
                        {
                            TimeSpan customStart = context.CustomSchedules.OrderBy(a => a.StartTime).Where(a => a.BellName.Contains("Bell ")).Select(a => a.StartTime).First();
                            if (time >= customStart.Subtract(TimeSpan.FromMinutes(5.0)) && time <= customStart)
                            {
                                await GenerateAttendanceFieldsDailyAttTask("Custom Bell Schedule");
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

        private async Task GenerateAttendanceFieldsDailyAttTask(string bellsched)
        {
            using var scope = _scopeFactory.CreateAsyncScope();
            var _userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var _context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var date = DateOnly.FromDateTime(DateTime.Now);

            var students = await _userManager.GetUsersInRoleAsync("Student");
            var noncheckDailyCourses = _context.ActiveCourseInfoModels.Where(a => a.DailyAttChecked == false).ToList();

            switch (bellsched)
            {
                case "Daily Bell Schedule":
                    {
                        await NormalScheduleRunner(students, noncheckDailyCourses, date, bellsched);
                        break;
                    }
                case "Extended Aves Bell Schedule":
                    {
                        await NormalScheduleRunner(students, noncheckDailyCourses, date, bellsched);
                        break;
                    }
                case "Pep Rally Bell Schedule":
                    {
                        await NormalScheduleRunner(students, noncheckDailyCourses, date, bellsched);
                        break;
                    }
                case "2 Hour Delay Bell Schedule":
                    {
                        await NormalScheduleRunner(students, noncheckDailyCourses, date, bellsched);
                        break;
                    }
                case "Custom Bell Schedule":
                    {
                        await CustomschedulRunner(students, noncheckDailyCourses, date, bellsched);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
            await Task.Delay(TimeSpan.FromMinutes(2));
            await Task.CompletedTask;
        }

        private async Task NormalScheduleRunner(IList<ApplicationUser> users, List<ActiveCourseInfoModel> courses, DateOnly date, string bellssched)
        {
            using var scope = _scopeFactory.CreateAsyncScope();
            var _context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            foreach (var student in users)
            {
                int studentId = int.Parse(student.SchoolId!);

                for (int bell = 0; bell <= 7; bell++)
                {
                    var sem2start = _context.SchedulerModels.Where(a => a.Type == SchedulerModel.Types.Semester2).Select(a => a.Date).FirstOrDefault();
                    IStudentSchedule? studentSchedule = (DateOnly.FromDateTime(DateTime.Now.Date) >= sem2start) ? (await _context.Sem2StudSchedules.FindAsync(studentId)) : (await _context.Sem1StudSchedules.FindAsync(studentId));
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

                    if (courses.Any(course => course.CourseId == bellCourseId))
                    {
                        foreach (var item in courses)
                        {
                            _logger.LogInformation("We don't add this courseId {CourseId}.", bellCourseId);
                            // If the course for this bell is in noncheckDailyCourses, skip to the next student
                            _logger.LogInformation("Course for this bell is in the noncheckDailyCourses.");
                        }
                    }
                    else
                    {
                        var entryExists = _context.DailyAttendanceModels.Any(a =>
                            a.StudentId == studentId &&
                            a.AttendanceDate == date);

                        if (entryExists)
                        {
                            _logger.LogInformation("Entry exists.");
                            continue;
                        }
                        else
                        {
                            //Add new entry if entry doesn't exist
                            var newEntry = new DailyAttendanceModel
                            {
                                StudentId = studentId,
                                AttendanceDate = date,
                                Status = "Unknown",
                                ReasonForAbsence = "Not Applicable Yet",
                                ChosenBellSchedule = bellssched
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

                            _context.DailyAttendanceModels.Add(newEntry);
                            _context.TimestampModels.Add(timestamp);
                            await _context.SaveChangesAsync();

                        }
                    }
                }
            }
        }

        private async Task CustomschedulRunner(IList<ApplicationUser> users, List<ActiveCourseInfoModel> courses, DateOnly date, string bellssched)
        {
            using var scope = _scopeFactory.CreateAsyncScope();

            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var bellsList = context.CustomSchedules.Where(a => a.BellName.Contains("Bell")).ToList() ?? throw new NullReferenceException();
            List<int> todayBells = [];

            foreach (var bell in bellsList)
            {
#pragma warning disable SYSLIB1045 // Convert to 'GeneratedRegexAttribute'.
                Match match = Regex.Match(bell.BellName, @"Bell (\d+)");
#pragma warning restore SYSLIB1045 // Convert to 'GeneratedRegexAttribute'.
                if (match.Success)
                {
                    todayBells.Add(int.Parse(match.Groups[1].Value));
                }
            }

            foreach (var student in users)
            {
                int studentId = int.Parse(student.SchoolId!);
                foreach (var bell in todayBells)
                {
                    var sem2start = context.SchedulerModels.Where(a => a.Type == SchedulerModel.Types.Semester2).Select(a => a.Date).FirstOrDefault();
                    IStudentSchedule? studentSchedule = (DateOnly.FromDateTime(DateTime.Now.Date) >= sem2start) ? (await context.Sem2StudSchedules.FindAsync(studentId)) : (await context.Sem1StudSchedules.FindAsync(studentId));
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

                    if (courses.Any(course => course.CourseId == bellCourseId))
                    {
                        // If the course for this bell is in noncheckDailyCourses, skip to the next student
                        _logger.LogInformation("Course for this bell is in the noncheckDailyCourses.");
                    }
                    else
                    {
                        var entryExists = context.DailyAttendanceModels.Any(a =>
                            a.StudentId == studentId &&
                            a.AttendanceDate == date);

                        if (entryExists)
                        {
                            _logger.LogInformation("Entry exists.");
                            continue;
                        }
                        else
                        {
                            //Add new entry if entry doesn't exist
                            var newEntry = new DailyAttendanceModel
                            {
                                StudentId = studentId,
                                AttendanceDate = date,
                                Status = "Unknown",
                                ReasonForAbsence = "Not Applicable Yet",
                                ChosenBellSchedule = bellssched
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
                            context.Database.ExecuteSqlRaw(rawSqlString);

                            context.DailyAttendanceModels.Add(newEntry);
                            context.TimestampModels.Add(timestamp);
                            await context.SaveChangesAsync();

                        }
                    }
                }
            }
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
}
