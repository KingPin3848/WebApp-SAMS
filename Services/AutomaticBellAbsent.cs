using Microsoft.AspNetCore.Identity;
using SAMS.Controllers;
using SAMS.Data;
using SAMS.Interfaces;
using SAMS.Models;

namespace SAMS.Services
{
    public class AutomaticBellAbsent(ILogger<AutomaticBellAbsent> logger, IServiceScopeFactory scopeFactory) : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory = scopeFactory;
        private readonly ILogger<AutomaticBellAbsent> _logger = logger;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await HolidayRun();
                // Wait for two minutes before checking again - COOL DOWN!!!!
                await Task.Delay(TimeSpan.FromMinutes(2), stoppingToken);
            }
        }

        private async Task HolidayRun()
        {
            using var scope = _scopeFactory.CreateAsyncScope();

            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var holidayDates = context.SchedulerModels.Where(a => a.Type == "No School @SHS").Select(a => a.Date).ToList();
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
                        await MarkAbsentTask();
                    }
                }
            }
        }

        private async Task MarkAbsentTask()
        {
            using var scope = _scopeFactory.CreateAsyncScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var date = DateTime.Now.Date;
            var time = DateTime.Now.TimeOfDay;
            var students = await userManager.GetUsersInRoleAsync("Student");
            var noncheckDailyCourses = context.ActiveCourseInfoModels.Where(a => a.DailyAttChecked == false).ToList();
            var noncheckBellCourses = context.ActiveCourseInfoModels.Where(a => a.B2BAttChecked == false).ToList();
            var chosenBellSchedName = context.ChosenBellSchedModels.Select(a => a.Name).FirstOrDefault();

            List<IBellSchedule>? chosenBellSched;
            switch (chosenBellSchedName)
            {
                case "Daily Bell Schedule":
                    chosenBellSched = [.. context.DailyBellScheduleModels.Where(a => a.BellName.Contains("Bell")).OrderBy(a => a.StartTime).Cast<IBellSchedule>()];
                    await NormalRunner(students, date, time, noncheckDailyCourses, noncheckBellCourses, chosenBellSched);
                    break;
                case "Extended Aves Bell Schedule":
                    chosenBellSched = [.. context.ExtendedAvesModels.Where(a => a.BellName.Contains("Bell")).OrderBy(a => a.StartTime).Cast<IBellSchedule>()];
                    await NormalRunner(students, date, time, noncheckDailyCourses, noncheckBellCourses, chosenBellSched);
                    break;
                case "Pep Rally Bell Schedule":
                    chosenBellSched = [.. context.PepRallyBellScheduleModels.Where(a => a.BellName.Contains("Bell")).OrderBy(a => a.StartTime).Cast<IBellSchedule>()];
                    await NormalRunner(students, date, time, noncheckDailyCourses, noncheckBellCourses, chosenBellSched);
                    break;
                case "2 Hour Delay Bell Schedule":
                    chosenBellSched = [.. context.TwoHrDelayBellScheduleModels.Where(a => a.BellName.Contains("Bell")).OrderBy(a => a.StartTime).Cast<IBellSchedule>()];
                    await NormalRunner(students, date, time, noncheckDailyCourses, noncheckBellCourses, chosenBellSched);
                    break;
                case "Custom Bell Schedule":
                    chosenBellSched = [.. context.CustomSchedules.Where(a => a.BellName.Contains("Bell")).OrderBy(a => a.StartTime).Cast<IBellSchedule>()];
                    await CustomRunner(students, date, time, noncheckDailyCourses, noncheckBellCourses, chosenBellSched);
                    break;
                default:
                    chosenBellSched = null;
                    _logger.LogInformation("A schedule was chosen other than the ones offered. Possible breach try.");
                    break;
            }
        }

        private async Task NormalRunner(IList<ApplicationUser> students, DateTime date, TimeSpan time, List<ActiveCourseInfoModel> noncheckDailyCourses, List<ActiveCourseInfoModel> noncheckBellCourses, List<IBellSchedule> chosenBellSched)
        {
            using var scope = _scopeFactory.CreateAsyncScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            if (chosenBellSched == null)
            {
                _logger.LogCritical("A schedule or option was chosen other than the ones offered. Possible breach try.");
            }
            else
            {
                var currentBell = GetCurrentBell(chosenBellSched);

                foreach (var bellObject in chosenBellSched)
                {
                    IBellSchedule bell = bellObject;
                    // Check if we are 15 minutes into the current bell
                    if (time >= bell.StartTime.Add(TimeSpan.FromMinutes(15)) && time < bell.EndTime)
                    {
                        foreach (var student in students)
                        {
                            var studentId = int.Parse(student.SchoolId!);
                            var sem2start = context.SchedulerModels.Where(a => a.Type == "Semester 2").Select(a => a.Date).FirstOrDefault();
                            IStudentSchedule? studentSchedule = (DateOnly.FromDateTime(DateTime.Now.Date) >= sem2start) ? (await context.Sem2StudSchedules.FindAsync(studentId)) : (await context.Sem1StudSchedules.FindAsync(studentId));
                            var sem2started = (DateOnly.FromDateTime(DateTime.Now.Date) >= sem2start);
                            int bellCourseId;
                            if (sem2started)
                            {
                                bellCourseId = GetS2BellCourseId(studentSchedule!, bell.BellName);
                            }
                            else
                            {
                                bellCourseId = GetS1BellCourseId(studentSchedule!, bell.BellName);
                            }
                            var bellCourseName = context.ActiveCourseInfoModels.Where(a => a.CourseId == bellCourseId).Select(a => a.CourseName).First();

                            var noncheckBelltrue = noncheckBellCourses.Any(course => course.CourseId == bellCourseId);
                            var noncheckDailytrue = noncheckDailyCourses.Any(course => course.CourseId == bellCourseId);
                            // If the course for this bell is in noncheckDailyCourses, skip to the next student
                            if (noncheckBelltrue || noncheckDailytrue)
                            {
                                _logger.LogInformation("Course was found inside the noncheck course list. noncheckBell list: {noncheckBelltrue}, noncheckDaily list: {noncheckDailytrue} Cannot mark absent for that class.", noncheckBelltrue, noncheckDailytrue);
                            }
                            else
                            {
                                var entryExists = context.BellAttendanceModels.Any(a =>
                                a.StudentId == studentId &&
                                a.DateTime.Date == date &&
                                a.BellNumId == bell.BellName &&
                                a.CourseId == bellCourseId &&
                                a.Status == "Unknown");

                                // If an unknown status entry exists, mark it as Absent
                                if (entryExists)
                                {
                                    var attendanceEntry = context.BellAttendanceModels.First(a =>
                                    a.StudentId == studentId &&
                                        a.DateTime.Date == date &&
                                        a.BellNumId == bell.BellName &&
                                        a.CourseId == bellCourseId &&
                                        a.Status == "Unknown");

                                    var timeStamp = new TimestampModel
                                    {
                                        Timestamp = DateTime.Now,
                                        ActionMade = "Marked Absent Automatically for Class",
                                        MadeBy = $"Automated Class Absence Service - SAMS Program {DateTime.Now}",
                                        Comments = $"The student was marked absent automatically because the student did not check themslves into the class: {bellCourseName} within the " +
                                        "5 minutes of start of the class. Please contact the Sycamore High School Attendance Office for any further questions or concerns."
                                    };

                                    attendanceEntry.Status = "Absent";
                                    context.BellAttendanceModels.Update(attendanceEntry);
                                    context.TimestampModels.Add(timeStamp);
                                    await context.SaveChangesAsync();
                                }
                            }
                        }
                    }
                }
            }
        }

        private async Task CustomRunner(IList<ApplicationUser> students, DateTime date, TimeSpan time, List<ActiveCourseInfoModel> noncheckDailyCourses, List<ActiveCourseInfoModel> noncheckBellCourses, List<IBellSchedule> chosenBellSched)
        {
            using var scope = _scopeFactory.CreateAsyncScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            if (chosenBellSched == null)
            {
                _logger.LogCritical("A schedule or option was chosen other than the ones offered. Possible breach try.");
            }
            else
            {
                var currentBell = GetCurrentBell(chosenBellSched);

                foreach (var bellObject in chosenBellSched)
                {
                    IBellSchedule bell = bellObject;
                    // Check if we are 15 minutes into the current bell
                    if (time >= bell.StartTime.Add(TimeSpan.FromMinutes(15)) && time < bell.EndTime)
                    {
                        foreach (var student in students)
                        {
                            var studentId = int.Parse(student.SchoolId!);
                            var sem2start = context.SchedulerModels.Where(a => a.Type == "Semester 2").Select(a => a.Date).FirstOrDefault();
                            IStudentSchedule? studentSchedule = (DateOnly.FromDateTime(DateTime.Now.Date) >= sem2start) ? (await context.Sem2StudSchedules.FindAsync(studentId)) : (await context.Sem1StudSchedules.FindAsync(studentId));
                            var sem2started = (DateOnly.FromDateTime(DateTime.Now.Date) >= sem2start);
                            int bellCourseId;
                            if (sem2started)
                            {
                                bellCourseId = GetS2BellCourseId(studentSchedule!, bell.BellName);
                            }
                            else
                            {
                                bellCourseId = GetS1BellCourseId(studentSchedule!, bell.BellName);
                            }
                            var bellCourseName = context.ActiveCourseInfoModels.Where(a => a.CourseId == bellCourseId).Select(a => a.CourseName).First();

                            var noncheckBelltrue = noncheckBellCourses.Any(course => course.CourseId == bellCourseId);
                            var noncheckDailytrue = noncheckDailyCourses.Any(course => course.CourseId == bellCourseId);
                            // If the course for this bell is in noncheckDailyCourses, skip to the next student
                            if (noncheckBelltrue || noncheckDailytrue)
                            {
                                _logger.LogInformation("Course was found inside the noncheck course list. noncheckBell list: {noncheckBelltrue}, noncheckDaily list: {noncheckDailytrue} Cannot mark absent for that class.", noncheckBelltrue, noncheckDailytrue);
                            }
                            else
                            {
                                var entryExists = context.BellAttendanceModels.Any(a =>
                                a.StudentId == studentId &&
                                a.DateTime.Date == date &&
                                a.BellNumId == bell.BellName &&
                                a.CourseId == bellCourseId &&
                                a.Status == "Unknown");

                                // If an unknown status entry exists, mark it as Absent
                                if (entryExists)
                                {
                                    var attendanceEntry = context.BellAttendanceModels.First(a =>
                                    a.StudentId == studentId &&
                                        a.DateTime.Date == date &&
                                        a.BellNumId == bell.BellName &&
                                        a.CourseId == bellCourseId &&
                                        a.Status == "Unknown");

                                    var timeStamp = new TimestampModel
                                    {
                                        Timestamp = DateTime.Now,
                                        ActionMade = "Marked Absent Automatically for Class",
                                        MadeBy = $"Automated Class Absence Service - SAMS Program {DateTime.Now}",
                                        Comments = $"The student was marked absent automatically because the student did not check themslves into the class: {bellCourseName} within the " +
                                        "5 minutes of start of the class. Please contact the Sycamore High School Attendance Office for any further questions or concerns."
                                    };

                                    attendanceEntry.Status = "Absent";
                                    context.BellAttendanceModels.Update(attendanceEntry);
                                    context.TimestampModels.Add(timeStamp);
                                    await context.SaveChangesAsync();
                                }
                            }
                        }
                    }
                }
            }
        }

        private static string? GetCurrentBell(List<IBellSchedule> chosenBellSched)
        {
            var currentTime = DateTime.Now.TimeOfDay;
            foreach (var bell in chosenBellSched)
            {
                if (currentTime >= bell.StartTime && currentTime <= bell.EndTime)
                {
                    return bell.BellName;
                }
            }
            return null;
        }

        private static int GetS1BellCourseId(IStudentSchedule studentSchedule, string bellName)
        {
            switch (bellName)
            {
                case "Bell 1":
                    return studentSchedule.Bell1CourseIDMod;
                case "Bell 2":
                    if (DateTime.Now.Date.DayOfWeek == DayOfWeek.Monday || DateTime.Now.Date.DayOfWeek == DayOfWeek.Wednesday)
                    {
                        return studentSchedule.Bell2MonWedCourseIDMod;
                    }
                    else if (DateTime.Now.Date.DayOfWeek == DayOfWeek.Friday)
                    {
                        return studentSchedule.FriBell2CourseIDMod;
                    }
                    return studentSchedule.Bell2TueThurCourseIDMod;
                case "Bell 3":
                    if (DateTime.Now.Date.DayOfWeek == DayOfWeek.Monday || DateTime.Now.Date.DayOfWeek == DayOfWeek.Wednesday)
                    {
                        return studentSchedule.Bell3MonWedCourseIDMod;
                    }
                    else if (DateTime.Now.Date.DayOfWeek == DayOfWeek.Friday)
                    {
                        return studentSchedule.FriBell3CourseIDMod;
                    }
                    return studentSchedule.Bell3TueThurCourseIDMod;
                case "Bell 4":
                    if (DateTime.Now.Date.DayOfWeek == DayOfWeek.Monday || DateTime.Now.Date.DayOfWeek == DayOfWeek.Wednesday)
                    {
                        return studentSchedule.Bell4MonWedCourseIDMod;
                    }
                    else if (DateTime.Now.Date.DayOfWeek == DayOfWeek.Friday)
                    {
                        return studentSchedule.FriBell4CourseIDMod;
                    }
                    return studentSchedule.Bell4TueThurCourseIDMod;
                case "Bell 5":
                    if (DateTime.Now.Date.DayOfWeek == DayOfWeek.Monday || DateTime.Now.Date.DayOfWeek == DayOfWeek.Wednesday)
                    {
                        return studentSchedule.Bell5MonWedCourseIDMod;
                    }
                    else if (DateTime.Now.Date.DayOfWeek == DayOfWeek.Friday)
                    {
                        return studentSchedule.FriBell5CourseIDMod;
                    }
                    return studentSchedule.Bell5TueThurCourseIDMod;
                case "Bell 6":
                    if (DateTime.Now.Date.DayOfWeek == DayOfWeek.Monday || DateTime.Now.Date.DayOfWeek == DayOfWeek.Wednesday)
                    {
                        return studentSchedule.Bell6MonWedCourseIDMod;
                    }
                    else if (DateTime.Now.Date.DayOfWeek == DayOfWeek.Friday)
                    {
                        return studentSchedule.FriBell6CourseIDMod;
                    }
                    return studentSchedule.Bell6TueThurCourseIDMod;
                case "Bell 7":
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
                    throw new ArgumentOutOfRangeException(nameof(bellName), "Invalid bell name provided.");
            }
        }

        private static int GetS2BellCourseId(IStudentSchedule studentSchedule, string bellName)
        {
            switch (bellName)
            {
                case "Bell 1":
                    return studentSchedule.Bell1CourseIDMod;
                case "Bell 2":
                    if (DateTime.Now.Date.DayOfWeek == DayOfWeek.Monday || DateTime.Now.Date.DayOfWeek == DayOfWeek.Wednesday)
                    {
                        return studentSchedule.Bell2MonWedCourseIDMod;
                    }
                    else if (DateTime.Now.Date.DayOfWeek == DayOfWeek.Friday)
                    {
                        return studentSchedule.FriBell2CourseIDMod;
                    }
                    return studentSchedule.Bell2TueThurCourseIDMod;
                case "Bell 3":
                    if (DateTime.Now.Date.DayOfWeek == DayOfWeek.Monday || DateTime.Now.Date.DayOfWeek == DayOfWeek.Wednesday)
                    {
                        return studentSchedule.Bell3MonWedCourseIDMod;
                    }
                    else if (DateTime.Now.Date.DayOfWeek == DayOfWeek.Friday)
                    {
                        return studentSchedule.FriBell3CourseIDMod;
                    }
                    return studentSchedule.Bell3TueThurCourseIDMod;
                case "Bell 4":
                    if (DateTime.Now.Date.DayOfWeek == DayOfWeek.Monday || DateTime.Now.Date.DayOfWeek == DayOfWeek.Wednesday)
                    {
                        return studentSchedule.Bell4MonWedCourseIDMod;
                    }
                    else if (DateTime.Now.Date.DayOfWeek == DayOfWeek.Friday)
                    {
                        return studentSchedule.FriBell4CourseIDMod;
                    }
                    return studentSchedule.Bell4TueThurCourseIDMod;
                case "Bell 5":
                    if (DateTime.Now.Date.DayOfWeek == DayOfWeek.Monday || DateTime.Now.Date.DayOfWeek == DayOfWeek.Wednesday)
                    {
                        return studentSchedule.Bell5MonWedCourseIDMod;
                    }
                    else if (DateTime.Now.Date.DayOfWeek == DayOfWeek.Friday)
                    {
                        return studentSchedule.FriBell5CourseIDMod;
                    }
                    return studentSchedule.Bell5TueThurCourseIDMod;
                case "Bell 6":
                    if (DateTime.Now.Date.DayOfWeek == DayOfWeek.Monday || DateTime.Now.Date.DayOfWeek == DayOfWeek.Wednesday)
                    {
                        return studentSchedule.Bell6MonWedCourseIDMod;
                    }
                    else if (DateTime.Now.Date.DayOfWeek == DayOfWeek.Friday)
                    {
                        return studentSchedule.FriBell6CourseIDMod;
                    }
                    return studentSchedule.Bell6TueThurCourseIDMod;
                case "Bell 7":
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
                    throw new ArgumentOutOfRangeException(nameof(bellName), "Invalid bell name provided.");
            }
        }
    }
}
