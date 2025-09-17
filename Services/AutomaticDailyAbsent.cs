using Microsoft.AspNetCore.Identity;
using SAMS.Data;
using SAMS.Interfaces;
using SAMS.Models;

namespace SAMS.Services
{
    public class AutomaticDailyAbsent(ILogger<AutomaticDailyAbsent> logger, IServiceScopeFactory serviceScopeFactory) : BackgroundService
    {
        private readonly ILogger<AutomaticDailyAbsent> _logger = logger;
        private readonly IServiceScopeFactory _scopeFactory = serviceScopeFactory;

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
                        await MarkAbsentDaily();
                    }
                }
            }
        }

        private async Task MarkAbsentDaily()
        {
            using var scope = _scopeFactory.CreateAsyncScope();
            var _userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var _context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var date = DateOnly.FromDateTime(DateTime.Now.Date);
            var time = DateTime.Now.TimeOfDay;


            var students = await _userManager.GetUsersInRoleAsync("Student");
            var noncheckDailyCourses = _context.ActiveCourseInfoModels.Where(a => a.DailyAttChecked == false).ToList();
            var chosenBellSchedName = _context.ChosenBellSchedModels.Select(a => a.Name).FirstOrDefault();

            List<IBellSchedule> chosenBellSched = [];
            switch (chosenBellSchedName)
            {
                case "Daily Bell Schedule":
                    {
                        chosenBellSched = [.. _context.DailyBellScheduleModels.OrderBy(a => a.StartTime).Where(a => a.BellName.Contains("Bell")).Cast<IBellSchedule>()];
                        await NormalRunner(students, noncheckDailyCourses, chosenBellSched, date, time);
                        break;
                    }
                case "Extended Aves Bell Schedule":
                    {
                        chosenBellSched = [.. _context.ExtendedAvesModels.OrderBy(a => a.StartTime).Where(a => a.BellName.Contains("Bell")).Cast<IBellSchedule>()];
                        await NormalRunner(students, noncheckDailyCourses, chosenBellSched, date, time);
                        break;
                    }
                case "Pep Rally Bell Schedule":
                    {
                        chosenBellSched = [.. _context.PepRallyBellScheduleModels.OrderBy(a => a.StartTime).Where(a => a.BellName.Contains("Bell")).Cast<IBellSchedule>()];
                        await NormalRunner(students, noncheckDailyCourses, chosenBellSched, date, time);
                        break;
                    }
                case "2 Hour Delay Bell Schedule":
                    {
                        chosenBellSched = [.. _context.TwoHrDelayBellScheduleModels.OrderBy(a => a.StartTime).Where(a => a.BellName.Contains("Bell")).Cast<IBellSchedule>()];
                        await NormalRunner(students, noncheckDailyCourses, chosenBellSched, date, time);
                        break;
                    }
                case "Custom Bell Schedule":
                    {
                        chosenBellSched = [.. _context.CustomSchedules.OrderBy(a => a.StartTime).Where(a => a.BellName.Contains("Bell")).Cast<IBellSchedule>()];
                        await CustomRunner(students, noncheckDailyCourses, chosenBellSched, date, time);
                        break;
                    }
                default:
                    {
                        throw new ArgumentException("Invalid bell schedule name provided.");
                    }
            }
        }

        private async Task CustomRunner(IList<ApplicationUser> students, List<ActiveCourseInfoModel> noncheckDailyCourses, List<IBellSchedule> chosenBellSched, DateOnly date, TimeSpan currenttime)
        {
            using var scope = _scopeFactory.CreateAsyncScope();

            var _context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            foreach (var bellObject in chosenBellSched)
            {
                dynamic bell = bellObject;

                if (bell.BellName.Contains("Transition"))
                {
                    _logger.LogInformation("Student is not marked absent during transition period.");
                }
                else
                {


                    // Check if we are 5 minutes into the current bell
                    if ((currenttime >= bell.StartTime.Add(TimeSpan.FromMinutes(5))) && (currenttime < bell.EndTime))
                    {
                        foreach (var student in students)
                        {
                            var studentId = int.Parse(student.SchoolId!);
                            var sem2start = _context.SchedulerModels.Where(a => a.Type == SchedulerModel.Types.Semester2).Select(a => a.Date).FirstOrDefault();
                            int bellCourseId;
                            if (DateOnly.FromDateTime(DateTime.Now.Date) >= sem2start)
                            {
                                var studentSchedule = await _context.Sem2StudSchedules.FindAsync(studentId);
                                bellCourseId = GetS2BellCourseId(studentSchedule, bell.BellName);
                            }
                            else
                            {
                                var studentSchedule = await _context.Sem1StudSchedules.FindAsync(studentId);
                                bellCourseId = GetS1BellCourseId(studentSchedule, bell.BellName);
                            }

                            // If the course for this bell is in noncheckDailyCourses, skip to the next student
                            if (noncheckDailyCourses.Any(course => course.CourseId == bellCourseId))
                            {
                                _logger.LogInformation("The course is inside noncheckdaily course list. Automatic Absence Service doesn't mark students absent for {bellCourseId}", bellCourseId);
                            }
                            else
                            {
                                var entryExists = _context.DailyAttendanceModels.Any(a =>
                                a.StudentId == studentId &&
                                a.AttendanceDate == date &&
                                a.Status == "Unknown");

                                // If an unknown status entry exists, mark it as Absent
                                if (entryExists)
                                {
                                    var attendanceEntry = _context.DailyAttendanceModels.First(a =>
                                        a.StudentId == studentId &&
                                        a.AttendanceDate == date &&
                                        a.Status == "Unknown");
                                    attendanceEntry.Status = "Absent";
                                    attendanceEntry.ReasonForAbsence = "Not confirmed. Student was marked absent automatically because the student did not check themslves into the school within the 15 minutes of the start of their first in-school class. Contact the SHS Attendance Office for any questions or concerns.";

                                    var timeStampEntry = new TimestampModel
                                    {
                                        Timestamp = DateTime.Now,
                                        ActionMade = "Student Marked Absent Automatically",
                                        MadeBy = $"Automated Building Absence Service - SAMS Program {DateTime.Now}",
                                        Comments = $"{studentId} was marked absent automatically because the student did not check themslves into the school and class within the 15 minutes of start of the class." +
                                        " Please contact the Sycamore High School Attendance Office for any further questions or concerns."
                                    };
                                    _context.TimestampModels.Add(timeStampEntry);
                                    _context.DailyAttendanceModels.Update(attendanceEntry);
                                    await _context.SaveChangesAsync();
                                }
                                else
                                {
                                    _logger.LogInformation("Couldn't find entry for {studentId}.", studentId);
                                }
                            }
                        }
                    }
                }
            }
        }

        private async Task NormalRunner(IList<ApplicationUser> students, List<ActiveCourseInfoModel> noncheckDailyCourses, List<IBellSchedule> chosenBellSchedName, DateOnly date, TimeSpan time)
        {
            using var scope = _scopeFactory.CreateAsyncScope();
            var _context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            foreach (var bellObject in chosenBellSchedName)
            {
                IBellSchedule bell = bellObject;

                // Check if we are 15 minutes into the current bell
                if (time >= bell.StartTime.Add(TimeSpan.FromMinutes(15)) && time < bell.EndTime)
                {
                    foreach (var student in students)
                    {
                        var studentId = int.Parse(student.SchoolId!);
                        var sem2start = _context.SchedulerModels.Where(a => a.Type == SchedulerModel.Types.Semester2).Select(a => a.Date).FirstOrDefault();
                        int bellCourseId;
                        if (DateOnly.FromDateTime(DateTime.Now.Date) >= sem2start)
                        {
                            var studentSchedule = await _context.Sem2StudSchedules.FindAsync(studentId);
                            bellCourseId = GetS2BellCourseId(studentSchedule!, bell.BellName);
                        }
                        else
                        {
                            var studentSchedule = await _context.Sem1StudSchedules.FindAsync(studentId);
                            bellCourseId = GetS1BellCourseId(studentSchedule!, bell.BellName);
                        }

                        // If the course for this bell is in noncheckDailyCourses, skip to the next student
                        if (noncheckDailyCourses.Any(course => course.CourseId == bellCourseId))
                        {
                            continue;
                        }

                        var entryExists = _context.DailyAttendanceModels.Any(a =>
                            a.StudentId == studentId &&
                            a.AttendanceDate == date &&
                            a.Status == "Unknown");

                        // If an unknown status entry exists, mark it as Absent
                        if (entryExists)
                        {
                            var attendanceEntry = _context.DailyAttendanceModels.First(a =>
                                a.StudentId == studentId &&
                                a.AttendanceDate == date &&
                                a.Status == "Unknown");
                            attendanceEntry.Status = "Absent";
                            attendanceEntry.ReasonForAbsence = "Not confirmed. Student was marked absent automatically because the student did not check themslves into the school within the 15 minutes of the start of their first in-school class. Contact the SHS Attendance Office for any questions or concerns.";

                            var timeStampEntry = new TimestampModel
                            {
                                Timestamp = DateTime.Now,
                                ActionMade = "Student Marked Absent Automatically",
                                MadeBy = $"Automated Building Absence Service - SAMS Program {DateTime.Now}",
                                Comments = "The student was marked absent automatically because the student did not check themslves into the school and class within the 15 minutes of start of the class." +
                                " Please contact the Sycamore High School Attendance Office for any further questions or concerns."
                            };
                            _context.TimestampModels.Add(timeStampEntry);
                            _context.DailyAttendanceModels.Update(attendanceEntry);
                            await _context.SaveChangesAsync();
                        }
                    }
                }
            }
        }

        private static int GetS1BellCourseId(Sem1StudSchedule studentSchedule, string bellName)
        {
            switch (bellName)
            {
                case "Bell 1":
                    return studentSchedule.Bell1CourseIDMod;
                case "Bell 2":
                    {
                        if (DateTime.Now.Date.DayOfWeek == DayOfWeek.Monday || DateTime.Now.Date.DayOfWeek == DayOfWeek.Wednesday)
                        {
                            return studentSchedule.Bell2MonWedCourseIDMod;
                        }
                        else if (DateTime.Now.Date.DayOfWeek == DayOfWeek.Friday)
                        {
                            return studentSchedule.FriBell2CourseIDMod;
                        }
                        return studentSchedule.Bell2TueThurCourseIDMod;
                    }
                case "Bell 3":
                    {
                        if (DateTime.Now.Date.DayOfWeek == DayOfWeek.Monday || DateTime.Now.Date.DayOfWeek == DayOfWeek.Wednesday)
                        {
                            return studentSchedule.Bell3MonWedCourseIDMod;
                        }
                        else if (DateTime.Now.Date.DayOfWeek == DayOfWeek.Friday)
                        {
                            return studentSchedule.FriBell3CourseIDMod;
                        }
                        return studentSchedule.Bell3TueThurCourseIDMod;
                    }
                case "Bell 4":
                    {
                        if (DateTime.Now.Date.DayOfWeek == DayOfWeek.Monday || DateTime.Now.Date.DayOfWeek == DayOfWeek.Wednesday)
                        {
                            return studentSchedule.Bell4MonWedCourseIDMod;
                        }
                        else if (DateTime.Now.Date.DayOfWeek == DayOfWeek.Friday)
                        {
                            return studentSchedule.FriBell4CourseIDMod;
                        }
                        return studentSchedule.Bell4TueThurCourseIDMod;
                    }
                case "Bell 5":
                    {
                        if (DateTime.Now.Date.DayOfWeek == DayOfWeek.Monday || DateTime.Now.Date.DayOfWeek == DayOfWeek.Wednesday)
                        {
                            return studentSchedule.Bell5MonWedCourseIDMod;
                        }
                        else if (DateTime.Now.Date.DayOfWeek == DayOfWeek.Friday)
                        {
                            return studentSchedule.FriBell5CourseIDMod;
                        }
                        return studentSchedule.Bell5TueThurCourseIDMod;
                    }
                case "Bell 6":
                    {
                        if (DateTime.Now.Date.DayOfWeek == DayOfWeek.Monday || DateTime.Now.Date.DayOfWeek == DayOfWeek.Wednesday)
                        {
                            return studentSchedule.Bell6MonWedCourseIDMod;
                        }
                        else if (DateTime.Now.Date.DayOfWeek == DayOfWeek.Friday)
                        {
                            return studentSchedule.FriBell6CourseIDMod;
                        }
                        return studentSchedule.Bell6TueThurCourseIDMod;
                    }
                case "Bell 7":
                    {
                        if (DateTime.Now.Date.DayOfWeek == DayOfWeek.Monday || DateTime.Now.Date.DayOfWeek == DayOfWeek.Wednesday)
                        {
                            return studentSchedule.Bell7MonWedCourseIDMod;
                        }
                        else if (DateTime.Now.Date.DayOfWeek == DayOfWeek.Friday)
                        {
                            return studentSchedule.FriBell7CourseIDMod;
                        }
                        return studentSchedule.Bell7TueThurCourseIDMod;
                    }
                default:
                    throw new ArgumentOutOfRangeException(nameof(bellName), "Invalid bell name provided.");
            }
        }

        private static int GetS2BellCourseId(Sem2StudSchedule studentSchedule, string bellName)
        {
            switch (bellName)
            {
                case "Bell 1":
                    return studentSchedule.Bell1CourseIDMod;
                case "Bell 2":
                    {
                        if (DateTime.Now.Date.DayOfWeek == DayOfWeek.Monday || DateTime.Now.Date.DayOfWeek == DayOfWeek.Wednesday)
                        {
                            return studentSchedule.Bell2MonWedCourseIDMod;
                        }
                        else if (DateTime.Now.Date.DayOfWeek == DayOfWeek.Friday)
                        {
                            return studentSchedule.FriBell2CourseIDMod;
                        }
                        return studentSchedule.Bell2TueThurCourseIDMod;
                    }
                case "Bell 3":
                    {
                        if (DateTime.Now.Date.DayOfWeek == DayOfWeek.Monday || DateTime.Now.Date.DayOfWeek == DayOfWeek.Wednesday)
                        {
                            return studentSchedule.Bell3MonWedCourseIDMod;
                        }
                        else if (DateTime.Now.Date.DayOfWeek == DayOfWeek.Friday)
                        {
                            return studentSchedule.FriBell3CourseIDMod;
                        }
                        return studentSchedule.Bell3TueThurCourseIDMod;
                    }
                case "Bell 4":
                    {
                        if (DateTime.Now.Date.DayOfWeek == DayOfWeek.Monday || DateTime.Now.Date.DayOfWeek == DayOfWeek.Wednesday)
                        {
                            return studentSchedule.Bell4MonWedCourseIDMod;
                        }
                        else if (DateTime.Now.Date.DayOfWeek == DayOfWeek.Friday)
                        {
                            return studentSchedule.FriBell4CourseIDMod;
                        }
                        return studentSchedule.Bell4TueThurCourseIDMod;
                    }
                case "Bell 5":
                    {
                        if (DateTime.Now.Date.DayOfWeek == DayOfWeek.Monday || DateTime.Now.Date.DayOfWeek == DayOfWeek.Wednesday)
                        {
                            return studentSchedule.Bell5MonWedCourseIDMod;
                        }
                        else if (DateTime.Now.Date.DayOfWeek == DayOfWeek.Friday)
                        {
                            return studentSchedule.FriBell5CourseIDMod;
                        }
                        return studentSchedule.Bell5TueThurCourseIDMod;
                    }
                case "Bell 6":
                    {
                        if (DateTime.Now.Date.DayOfWeek == DayOfWeek.Monday || DateTime.Now.Date.DayOfWeek == DayOfWeek.Wednesday)
                        {
                            return studentSchedule.Bell6MonWedCourseIDMod;
                        }
                        else if (DateTime.Now.Date.DayOfWeek == DayOfWeek.Friday)
                        {
                            return studentSchedule.FriBell6CourseIDMod;
                        }
                        return studentSchedule.Bell6TueThurCourseIDMod;
                    }
                case "Bell 7":
                    {
                        if (DateTime.Now.Date.DayOfWeek == DayOfWeek.Monday || DateTime.Now.Date.DayOfWeek == DayOfWeek.Wednesday)
                        {
                            return studentSchedule.Bell7MonWedCourseIDMod;
                        }
                        else if (DateTime.Now.Date.DayOfWeek == DayOfWeek.Friday)
                        {
                            return studentSchedule.FriBell7CourseIDMod;
                        }
                        return studentSchedule.Bell7TueThurCourseIDMod;
                    }
                default:
                    throw new ArgumentOutOfRangeException(nameof(bellName), "Invalid bell name provided.");
            }
        }

        //public object GetBellSchedule(string chosenBellSchedName)
        //{
        //    using var scope = _scopeFactory.CreateAsyncScope();
        //    var _context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        //    List<object> chosenBellSched;
        //    switch (chosenBellSchedName)
        //    {
        //        case "Daily Bell Schedule":
        //            {
        //                chosenBellSched = [.. _context.dailyBellScheduleModels.OrderBy(a => a.StartTime).Cast<object>()];
        //                break;
        //            }
        //        case "Extended Aves Bell Schedule":
        //            {
        //                chosenBellSched = [.. _context.extendedAvesModels.OrderBy(a => a.StartTime).Cast<object>()];
        //                break;
        //            }
        //        case "Pep Rally Bell Schedule":
        //            {
        //                chosenBellSched = [.. _context.pepRallyBellScheduleModels.OrderBy(a => a.StartTime).Cast<object>()];
        //                break;
        //            }
        //        case "2 Hour Delay Bell Schedule":
        //            {
        //                chosenBellSched = [.. _context.twoHrDelayBellScheduleModels.OrderBy(a => a.StartTime).Cast<object>()];
        //                break;
        //            }
        //        case "Custom Bell Schedule":
        //            {
        //                chosenBellSched = [.. _context.CustomSchedules.OrderBy(a => a.StartTime).Cast<object>()];
        //                break;
        //            }
        //        default:
        //            {
        //                throw new ArgumentException("Invalid bell schedule name provided.");
        //            }
        //    }
        //    return chosenBellSched;
        //}

    }
}
