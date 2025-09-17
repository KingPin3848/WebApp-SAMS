using Microsoft.AspNetCore.Identity;
using SAMS.Data;
using SAMS.Interfaces;
using SAMS.Models;

namespace SAMS.Services
{
    public class AutomaticAvesAbsent(IServiceScopeFactory scopeFactory, ILogger<AutomaticAvesAbsent> logger) : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory = scopeFactory;
        private readonly ILogger<AutomaticAvesAbsent> _logger = logger;

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
                        await MarkAbsentTask();
                    }
                }
            }
        }

        private async Task MarkAbsentTask()
        {
            using var scope = _scopeFactory.CreateAsyncScope();
            var _userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var _context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var students = await _userManager.GetUsersInRoleAsync("Student");
            var noncheckDailyCourses = _context.ActiveCourseInfoModels.Where(a => a.DailyAttChecked == false).ToList();
            var noncheckBellCourses = _context.ActiveCourseInfoModels.Where(a => a.B2BAttChecked == false).ToList();
            var chosenBellSchedName = _context.ChosenBellSchedModels.Select(a => a.Name).FirstOrDefault();

            List<IBellSchedule>? chosenBellSched;
            switch (chosenBellSchedName)
            {
                case "Daily Bell Schedule":
                    {
                        chosenBellSched = [.. _context.DailyBellScheduleModels.Where(a => a.BellName.Contains("Aves")).OrderBy(a => a.StartTime).Cast<IBellSchedule>()];
                        await NormalRunner(students, noncheckBellCourses, noncheckDailyCourses, chosenBellSched);
                        break;
                    }
                case "Extended Aves Bell Schedule":
                    {
                        chosenBellSched = [.. _context.ExtendedAvesModels.Where(a => a.BellName.Contains("Aves")).OrderBy(a => a.StartTime).Cast<IBellSchedule>()];
                        await NormalRunner(students, noncheckBellCourses, noncheckDailyCourses, chosenBellSched);
                        break;
                    }
                case "Custom Bell Schedule":
                    {
                        if (_context.CustomSchedules.OrderBy(a => a.StartTime).Where(a => a.BellName.Contains("Aves Bell")).Select(a => a.BellName).ToString() == "Aves Bell")
                        {
                            chosenBellSched = [.. _context.CustomSchedules.Where(a => a.BellName.Contains("Aves")).OrderBy(a => a.StartTime).Cast<IBellSchedule>()];
                            await CustomRunner(students, noncheckBellCourses, noncheckDailyCourses, chosenBellSched);
                        }
                        else
                        {
                            _logger.LogInformation("Check 1. Couldn't find aves bell in the custom bell schedule. Hence, the task will be delayed by ONE DAY.");
                            chosenBellSched = null;
                            await CustomRunner(students, noncheckBellCourses, noncheckDailyCourses, chosenBellSched);
                            break;
                        }
                        break;
                    }
                default:
                    {
                        _logger.LogInformation("A schedule other than daily, extended aves, or custom bell schedule was chosen. The Automatic Aves absence service is delayed for ONE DAY.");
                        await Task.Delay(TimeSpan.FromDays(1));
                        break;
                    }
            }
        }

        private async Task NormalRunner(IList<ApplicationUser> students, List<ActiveCourseInfoModel> noncheckBellCourses, List<ActiveCourseInfoModel> noncheckDailyCourses, List<IBellSchedule> chosenBellSched)
        {
            using var scope = _scopeFactory.CreateAsyncScope();
            var _context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var date = DateTime.Now.Date;
            var time = DateTime.Now.TimeOfDay;

            if (chosenBellSched == null)
            {
                _logger.LogInformation("Check 2. No Aves bell found in custom schedule. Hence null and couldn't be worked out.");
                await Task.Delay(TimeSpan.FromDays(1));
            }
            else
            {
                foreach (var bellObject in chosenBellSched)
                {
                    if (bellObject.BellName == "Aves Bell")
                    {
                        IBellSchedule bell = bellObject;
                        // Check if we are 5 minutes into the current bell
                        if (time >= bell.StartTime.Add(TimeSpan.FromMinutes(5)) && time < bell.EndTime)
                        {
                            foreach (var student in students)
                            {
                                int studentId = int.Parse(student.SchoolId!);

                                var sem2start = _context.SchedulerModels.Where(a => a.Type == SchedulerModel.Types.Semester2).Select(a => a.Date).FirstOrDefault();
                                IStudentSchedule? studentSchedule = (DateOnly.FromDateTime(DateTime.Now.Date) >= sem2start) ? (await _context.Sem2StudSchedules.FindAsync(studentId)) : (await _context.Sem1StudSchedules.FindAsync(studentId));
                                var sem2started = (DateOnly.FromDateTime(DateTime.Now.Date) >= sem2start);
                                int bellCourseId;
                                if (sem2started)
                                {
                                    bellCourseId = GetS2BellCourseId(studentSchedule!);
                                }
                                else
                                {
                                    bellCourseId = GetS1BellCourseId(studentSchedule!);
                                }
                                var bellCourseName = _context.ActiveCourseInfoModels.Where(a => a.CourseId == bellCourseId).Select(a => a.CourseName).First();

                                bool inNonCheckDailyCourses = false;
                                bool inNonCheckBellCourses = false;
                                // If the course for this bell is in noncheckDailyCourses or noncheckBellCourses, skip to the next student
                                if (noncheckDailyCourses.Any(course => course.CourseId == bellCourseId))
                                {
                                    inNonCheckDailyCourses = true;
                                    _logger.LogInformation("The course {courseid} {coursename} was inside the noncheckdaily course list.", bellCourseId, bellCourseName);
                                }
                                if (noncheckBellCourses.Any(course => course.CourseId == bellCourseId))
                                {
                                    inNonCheckBellCourses = true;
                                    _logger.LogInformation("The course {courseid} {coursename} was inside the noncheckbell course list.", bellCourseId, bellCourseName);
                                }
                                if ((inNonCheckDailyCourses == false) || (inNonCheckBellCourses == false))
                                {
                                    var entryExists = _context.BellAttendanceModels.Any(a =>
                                    a.StudentId == studentId &&
                                    a.DateTime.Date == date &&
                                    a.BellNumId.Contains("Aves Bell") &&
                                    a.Status == "Unknown");

                                    // If an unknown status entry exists, mark it as Absent
                                    if (entryExists)
                                    {
                                        var attendanceEntry = _context.BellAttendanceModels.First(a =>
                                            a.StudentId == studentId &&
                                            a.DateTime.Date == date &&
                                            a.BellNumId.Contains("Aves Bell") &&
                                            a.Status == "Unknown");

                                        var timeStamp = new TimestampModel
                                        {
                                            Timestamp = DateTime.Now,
                                            ActionMade = "Marked Absent Automatically for Aves Bell",
                                            MadeBy = $"Automated Aves Absence Service - SAMS Program {DateTime.Now}",
                                            Comments = $"The student was marked absent automatically because the student did not check themslves into aves bell within the " +
                                            "5 minutes of start of the class. Please contact the Sycamore High School Attendance Office for any further questions or concerns."
                                        };

                                        attendanceEntry.Status = "Absent";
                                        attendanceEntry.ReasonForAbsence = "Not confirmed. Student was marked absent automatically because the student did not check themslves into the aves bell class within the 5 minutes of the start of aves bell. Contact the SHS Attendance Office for any questions or concerns.";
                                        _context.BellAttendanceModels.Update(attendanceEntry);
                                        await _context.SaveChangesAsync();
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private async Task CustomRunner(IList<ApplicationUser> students, List<ActiveCourseInfoModel> noncheckBellCourses, List<ActiveCourseInfoModel> noncheckDailyCourses, List<IBellSchedule>? chosenBellSched)
        {
            using var scope = _scopeFactory.CreateAsyncScope();
            var _context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var date = DateTime.Now.Date;
            var time = DateTime.Now.TimeOfDay;


            if (chosenBellSched == null)
            {
                _logger.LogInformation("Check 2. No Aves bell found in custom schedule. Hence null and couldn't be worked out.");
                await Task.Delay(TimeSpan.FromDays(1));
            }
            else
            {
                foreach (var bellObject in chosenBellSched)
                {
                    if (bellObject.BellName == "Aves Bell")
                    {
                        IBellSchedule bell = bellObject;
                        // Check if we are 5 minutes into the current bell
                        if (time >= bell.StartTime.Add(TimeSpan.FromMinutes(5)) && time < bell.EndTime)
                        {
                            foreach (var student in students)
                            {
                                int studentId = int.Parse(student.SchoolId!);

                                var sem2start = _context.SchedulerModels.Where(a => a.Type == SchedulerModel.Types.Semester2).Select(a => a.Date).FirstOrDefault();
                                IStudentSchedule? studentSchedule = (DateOnly.FromDateTime(DateTime.Now.Date) >= sem2start) ? (await _context.Sem2StudSchedules.FindAsync(studentId)) : (await _context.Sem1StudSchedules.FindAsync(studentId));
                                var sem2started = (DateOnly.FromDateTime(DateTime.Now.Date) >= sem2start);
                                int bellCourseId;
                                if (sem2started)
                                {
                                    bellCourseId = GetS2BellCourseId(studentSchedule!);
                                }
                                else
                                {
                                    bellCourseId = GetS1BellCourseId(studentSchedule!);
                                }
                                var bellCourseName = _context.ActiveCourseInfoModels.Where(a => a.CourseId == bellCourseId).Select(a => a.CourseName).First();

                                // If the course for this bell is in noncheckDailyCourses or noncheckBellCourses, skip to the next student
                                if (noncheckDailyCourses.Any(course => course.CourseId == bellCourseId))
                                {
                                    _logger.LogInformation("The course {courseid} {coursename} was inside the noncheckdaily course list.", bellCourseId, bellCourseName);
                                }
                                else if (noncheckBellCourses.Any(course => course.CourseId == bellCourseId))
                                {
                                    _logger.LogInformation("The course {courseid} {coursename} was inside the noncheckbell course list.", bellCourseId, bellCourseName);
                                }
                                else
                                {
                                    var entryExists = _context.BellAttendanceModels.Any(a =>
                                    a.StudentId == studentId &&
                                    a.DateTime.Date == date &&
                                    a.BellNumId.Contains("Aves Bell") &&
                                    a.Status == "Unknown");

                                    // If an unknown status entry exists, mark it as Absent
                                    if (entryExists)
                                    {
                                        var attendanceEntry = _context.BellAttendanceModels.First(a =>
                                        a.StudentId == studentId &&
                                            a.DateTime.Date == date &&
                                            a.BellNumId.Contains("Aves Bell") &&
                                            a.Status == "Unknown");

                                        var timeStamp = new TimestampModel
                                        {
                                            Timestamp = DateTime.Now,
                                            ActionMade = "Marked Absent Automatically for Aves Bell",
                                            MadeBy = $"Automated Aves Absence Service - SAMS Program {DateTime.Now}",
                                            Comments = $"The student was marked absent automatically because the student did not check themslves into the class: {bellCourseName} within the " +
                                            "10 minutes of start of the class. Please contact the Sycamore High School Attendance Office for any further questions or concerns."
                                        };

                                        attendanceEntry.Status = "Absent";
                                        attendanceEntry.ReasonForAbsence = "Not confirmed. Student was marked absent automatically because the student did not check themslves into the aves bell class within the 5 minutes of the start of their first in-school class. Contact the SHS Attendance Office for any questions or concerns.";
                                        _context.BellAttendanceModels.Update(attendanceEntry);
                                        await _context.SaveChangesAsync();
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private static int GetS1BellCourseId(IStudentSchedule studentSchedule)
        {
            return studentSchedule.AvesBellCourseIDMod;
        }

        private static int GetS2BellCourseId(IStudentSchedule studentSchedule)
        {
            return studentSchedule.AvesBellCourseIDMod;
        }
    }
}
