using Microsoft.AspNetCore.Identity;
using SAMS.Data;
using SAMS.Interfaces;
using SAMS.Models;
using System.Text.RegularExpressions;

namespace SAMS.Services
{
    public class Bell2BellAdditionService(ILogger<Bell2BellAdditionService> logger, IServiceScopeFactory scopeFactory) : BackgroundService
    {
        private readonly ILogger<Bell2BellAdditionService> _logger = logger;
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
                _logger.LogWarning("Holidays is null and the task is delayed by 1 DAY. Done by the if statement in holidayRun");
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
                                await BellToBellAttendanceAsync("Daily Bell Schedule");
                                break;
                            }
                            break;
                        }

                    case "Pep Rally Bell Schedule":
                        {
                            TimeSpan peprallyStart = new(7, 15, 00);
                            if (time >= peprallyStart && time <= new TimeSpan(07, 20, 00))
                            {
                                await BellToBellAttendanceAsync("Pep Rally Bell Schedule");
                            }
                            break;
                        }

                    case "2 Hour Delay Bell Schedule":
                        {
                            TimeSpan _2hrdelStart = new(9, 15, 00);
                            if (time >= _2hrdelStart && time <= new TimeSpan(09, 20, 00))
                            {
                                await BellToBellAttendanceAsync("2 Hour Delay Bell Schedule");
                            }
                            break;
                        }

                    case "Extended Aves Bell Schedule":
                        {
                            TimeSpan extAvesStart = new(7, 15, 00);
                            if (time >= extAvesStart && time <= new TimeSpan(07, 20, 00))
                            {
                                await BellToBellAttendanceAsync("Extended Aves Bell Schedule");
                            }
                            break;
                        }
                    case "Custom Bell Schedule":
                        {
                            var customStart = context.CustomSchedules.OrderBy(a => a.StartTime).Where(a => a.BellName == "Bell ").First();
                            if (time >= customStart.StartTime && time <= customStart.EndTime)
                            {
                                await BellToBellAttendanceAsync("Custom Bell Schedule");
                            }
                            break;
                        }
                    default:
                        {
                            _logger.LogInformation("The task is supposed to be delayed for 5 MINUTES. Done by default case in ScheduleRunner");
                            await Task.Delay(TimeSpan.FromMinutes(5));
                            //await Task.Delay(TimeSpan.FromMinutes(5), token);
                            break;
                        }
                }
            }
            else
            {
                await Task.Delay(TimeSpan.FromDays(1));
            }
            //await Task.CompletedTask;
        }

        private async Task BellToBellAttendanceAsync(string bellsChed)
        {
            using var scope = _scopeFactory.CreateAsyncScope();

            var _userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var _context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var students = await _userManager.GetUsersInRoleAsync("Student");
            var noncheckBellCourses = _context.ActiveCourseInfoModels.Where(a => a.B2BAttChecked == false).ToList();


            switch (bellsChed)
            {
                case "Daily Bell Schedule":
                    {
                        await NormalRunner(bellsChed, students, noncheckBellCourses);
                        break;
                    }
                case "Pep Rally Bell Schedule":
                    {
                        await NormalRunner(bellsChed, students, noncheckBellCourses);
                        break;
                    }

                case "2 Hour Delay Bell Schedule":
                    {
                        await NormalRunner(bellsChed, students, noncheckBellCourses);
                        break;
                    }

                case "Extended Aves Bell Schedule":
                    {
                        await NormalRunner(bellsChed, students, noncheckBellCourses);
                        break;
                    }
                case "Custom Bell Schedule":
                    {
                        await CustomRunner(bellsChed, students, noncheckBellCourses);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        private async Task NormalRunner(string bellsChed, IList<ApplicationUser> students, List<ActiveCourseInfoModel> noncheckBellCourses)
        {
            using var scope = _scopeFactory.CreateAsyncScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            foreach (var student in students)
            {
                var studentId = int.Parse(student.SchoolId!);

                var sem2start = context.SchedulerModels.Where(a => a.Type == SchedulerModel.Types.Semester2).Select(a => a.Date).FirstOrDefault();
                IStudentSchedule? studentSchedule = (DateOnly.FromDateTime(DateTime.Now.Date) >= sem2start) ? (await context.Sem2StudSchedules.FindAsync(studentId)) : (await context.Sem1StudSchedules.FindAsync(studentId));
                var sem2started = (DateOnly.FromDateTime(DateTime.Now.Date) >= sem2start);
                int bellCourseId;

                for (int bell = 1; bell <= 7; bell++)
                {
                    if (sem2started)
                    {
                        bellCourseId = GetS2BellCourseId(studentSchedule!, bell);
                    }
                    else
                    {
                        bellCourseId = GetS1BellCourseId(studentSchedule!, bell);
                    }

                    if (!(noncheckBellCourses.Any(course => course.CourseId == bellCourseId)))
                    {
                        // Check if a bell attendance record already exists for the current student, next bell, and date
                        var entryExists = context.BellAttendanceModels.Any(a =>
                            a.StudentId == studentId &&
                            a.DateTime.Date == DateTime.Now.Date &&
                            a.BellNumId == $"Bell {bell}"
                            );

                        // If no record exists, create a new BellAttendanceModel with the appropriate details and save it to the database
                        if (!entryExists)
                        {
                            var newEntry = new BellAttendanceModel
                            {
                                StudentId = studentId,
                                DateTime = DateTime.Now,
                                Status = "Unknown",
                                ReasonForAbsence = "NA",
                                BellNumId = $"Bell {bell}",
                                CourseId = bellCourseId,
                                ChosenBellSchedule = bellsChed
                            };

                            var timestamp = new TimestampModel
                            {
                                Timestamp = DateTime.Now,
                                ActionMade = $"Bell attendance default added for Student ID {studentId}, Date: {DateTime.Now}, Email: {student.Email}, Bell: {bell}, Course DB Serial Number: {bellCourseId}",
                                MadeBy = "Bell Att. Addition Service",
                                Comments = $"The course is: {context.ActiveCourseInfoModels.Where(a => a.CourseId == bellCourseId).Select(a => a.CourseName)} taught by " +
                                $"{context.ActiveCourseInfoModels.Where(a => a.CourseId == bellCourseId).Select(a => a.Teacher!.TeacherFirstNameMod)} " +
                                $"{context.ActiveCourseInfoModels.Where(a => a.CourseId == bellCourseId).Select(a => a.Teacher!.TeacherLastNameMod)}"
                            };

                            context.BellAttendanceModels.Add(newEntry);
                            context.TimestampModels.Add(timestamp);
                            await context.SaveChangesAsync();
                        }
                        else
                        {
                            _logger.LogInformation("Entry exists already in B2BAttendance.");
                        }
                    }
                    else
                    {
                        _logger.LogInformation("Course is inside the noncheck courses.");
                    }
                }
            }

            throw new NotImplementedException();
        }

        private async Task CustomRunner(string bellsChed, IList<ApplicationUser> students, List<ActiveCourseInfoModel> noncheckBellCourses)
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

            foreach (var student in students)
            {
                var studentId = int.Parse(student.SchoolId!);
                var sem2start = context.SchedulerModels.Where(a => a.Type == SchedulerModel.Types.Semester2).Select(a => a.Date).FirstOrDefault();
                IStudentSchedule? studentSchedule = (DateOnly.FromDateTime(DateTime.Now.Date) >= sem2start) ? (await context.Sem2StudSchedules.FindAsync(studentId)) : (await context.Sem1StudSchedules.FindAsync(studentId));
                var sem2started = (DateOnly.FromDateTime(DateTime.Now.Date) >= sem2start);
                int bellCourseId;

                foreach (var bell in todayBells)
                {
                    if (sem2started)
                    {
                        bellCourseId = GetS2BellCourseId(studentSchedule!, bell);
                    }
                    else
                    {
                        bellCourseId = GetS1BellCourseId(studentSchedule!, bell);
                    }

                    if (!(noncheckBellCourses.Any(course => course.CourseId == bellCourseId)))
                    {
                        // Check if a bell attendance record already exists for the current student, next bell, and date
                        var entryExists = context.BellAttendanceModels.Any(a =>
                            a.StudentId == studentId &&
                            a.DateTime.Date == DateTime.Now.Date &&
                            a.BellNumId == $"Bell {bell}"
                            );

                        // If no record exists, create a new BellAttendanceModel with the appropriate details and save it to the database
                        if (!entryExists)
                        {
                            var newEntry = new BellAttendanceModel
                            {
                                StudentId = studentId,
                                DateTime = DateTime.Now,
                                Status = "Unknown",
                                ReasonForAbsence = "NA",
                                BellNumId = $"Bell {bell}",
                                CourseId = bellCourseId,
                                ChosenBellSchedule = bellsChed
                            };

                            var timestamp = new TimestampModel
                            {
                                Timestamp = DateTime.Now,
                                ActionMade = $"Bell attendance default added for Student ID {studentId}, Date: {DateTime.Now}, Email: {student.Email}, Bell: {bell}, Course DB Serial Number: {bellCourseId}",
                                MadeBy = "Bell Att. Addition Service",
                                Comments = $"The course is: {context.ActiveCourseInfoModels.Where(a => a.CourseId == bellCourseId).Select(a => a.CourseName)} taught by " +
                                $"{context.ActiveCourseInfoModels.Where(a => a.CourseId == bellCourseId).Select(a => a.Teacher!.TeacherFirstNameMod)} " +
                                $"{context.ActiveCourseInfoModels.Where(a => a.CourseId == bellCourseId).Select(a => a.Teacher!.TeacherLastNameMod)}"
                            };

                            context.BellAttendanceModels.Add(newEntry);
                            context.TimestampModels.Add(timestamp);
                            await context.SaveChangesAsync();
                        }
                    }
                }
            }

            throw new NotImplementedException();
        }

        //private bool IsTransitionPeriod(string chosenBellSched)
        //{
        //    using var scope = _scopeFactory.CreateAsyncScope();

        //    var _context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        //    var currentTime = DateTime.Now.TimeOfDay;
        //    var bellSchedule = _context;

        //    switch (chosenBellSched)
        //    {
        //        case "Daily Bell Schedule":
        //            {
        //                var dailyBells = bellSchedule.dailyBellScheduleModels.OrderBy(a => a.StartTime).ToList();
        //                // Check if we are currently in a transition period
        //                for (int i = 0; i < dailyBells.Count; i++)
        //                {
        //                    if (dailyBells[i].BellName.Contains("Transition") && currentTime >= dailyBells[i].StartTime && currentTime < dailyBells[i].EndTime)
        //                    {
        //                        return true;
        //                    }
        //                }
        //                return false;
        //            }
        //        case "Extended Aves Bell Schedule":
        //            {
        //                var extBells = bellSchedule.extendedAvesModels.OrderBy(a => a.StartTime).ToList();
        //                // Check if we are currently in a transition period
        //                for (int i = 0; i < extBells.Count; i++)
        //                {
        //                    if (extBells[i].BellName.Contains("Transition") && currentTime >= extBells[i].StartTime && currentTime < extBells[i].EndTime)
        //                    {
        //                        return true;
        //                    }
        //                }
        //                return false;
        //            }
        //        case "Pep Rally Bell Schedule":
        //            {
        //                var pepBells = bellSchedule.pepRallyBellScheduleModels.OrderBy(b => b.StartTime).ToList();
        //                // Check if we are currently in a transition period
        //                for (int i = 0; i < pepBells.Count; i++)
        //                {
        //                    if (pepBells[i].BellName.Contains("Transition") && currentTime >= pepBells[i].StartTime && currentTime < pepBells[i].EndTime)
        //                    {
        //                        return true;
        //                    }
        //                }
        //                return false;
        //            }
        //        case "2 Hour Delay Bell Schedule":
        //            {
        //                var twodelayBells = bellSchedule.twoHrDelayBellScheduleModels.OrderBy(b => b.StartTime).ToList();
        //                // Check if we are currently in a transition period
        //                for (int i = 0; i < twodelayBells.Count; i++)
        //                {
        //                    if (twodelayBells[i].BellName.Contains("Transition") && currentTime >= twodelayBells[i].StartTime && currentTime < twodelayBells[i].EndTime)
        //                    {
        //                        return true;
        //                    }
        //                }
        //                return false;
        //            }
        //        case "Custom Bell Schedule":
        //            {
        //                var custombells = bellSchedule.CustomSchedules.OrderBy(a => a.StartTime).ToList();
        //                for (int i = 0; i < custombells.Count; i++)
        //                {
        //                    if ((custombells[i].BellName.Contains("Transition")) && (currentTime >= custombells[i].StartTime) && (currentTime < custombells[i].EndTime))
        //                    {
        //                        return true;
        //                    }
        //                }
        //                return false;
        //            }
        //        default:
        //            {
        //                return false;
        //            }
        //    }

        //    // Check if we are currently in a transition period
        //    /*for (int i = 0; i < bellSchedule.Count - 1; i++)
        //    {
        //        if (bellSchedule[i].BellName.Contains("Transition") && currentTime >= bellSchedule[i].StartTime && currentTime < bellSchedule[i].EndTime)
        //        {
        //            return true;
        //        }
        //    }*/
        //}

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
