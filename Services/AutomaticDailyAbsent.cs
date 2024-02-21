using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Microsoft.EntityFrameworkCore;
using SAMS.Controllers;
using SAMS.Data;
using SAMS.Interfaces;
using SAMS.Models;

namespace SAMS.Services
{
    public class AutomaticDailyAbsent : BackgroundService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationUser> _roleManager;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AutomaticDailyAbsent> _logger;

        public AutomaticDailyAbsent(UserManager<ApplicationUser> userManager, RoleManager<ApplicationUser> roleManager, ApplicationDbContext dbContext, ILogger<AutomaticDailyAbsent> logger)
        {
            _context = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(!stoppingToken.IsCancellationRequested)
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
                        await MarkAbsentDaily();
                    }
                }
                // Wait for one minute before checking again - COOL DOWN!!!!
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }

        private async Task MarkAbsentDaily()
        {
            var date = DateTime.Now.Date;
            var time = DateTime.Now.TimeOfDay;
            var students = await _userManager.GetUsersInRoleAsync("Student");
            var noncheckDailyCourses = _context.activeCourseInfoModels.Where(a => a.DailyAttChecked == false).ToList();
            var chosenBellSchedName = _context.chosenBellSchedModels.Select(a => a.Name).FirstOrDefault();

            List<IStudentSchedule> chosenBellSched;
            switch (chosenBellSchedName)
            {
                case "Daily Bell Schedule":
                    {
                        chosenBellSched = _context.dailyBellScheduleModels.OrderBy(a => a.StartTime).Cast<IStudentSchedule>().ToList();
                        break;
                    }
                case "Extended Aves Bell Schedule":
                    {
                        chosenBellSched = _context.extendedAvesModels.OrderBy(a => a.StartTime).Cast<IStudentSchedule>().ToList();
                        break;
                    }
                case "Pep Rally Bell Schedule":
                    {
                        chosenBellSched = _context.pepRallyBellScheduleModels.OrderBy(a => a.StartTime).Cast<IStudentSchedule>().ToList();
                        break;
                    }
                case "Two Hour Delay Bell Schedule":
                    {
                        chosenBellSched = _context.twoHrDelayBellScheduleModels.OrderBy(a => a.StartTime).Cast<IStudentSchedule>().ToList();
                        break;
                    }
                default:
                    {
                        throw new ArgumentException("Invalid bell schedule name provided.");
                    }
            }


            foreach (var bellObject in chosenBellSched)
            {
                dynamic bell = bellObject;

                // Check if we are 15 minutes into the current bell
                if (time >= bell.StartTime.Add(TimeSpan.FromMinutes(15)) && time < bell.EndTime)
                {
                    foreach (var student in students)
                    {
                        var studentId = int.Parse(student.SchoolId);
                        var sem2start = _context.schedulerModels.Where(a => a.Type == "Semester 2").Select(a => a.Date).FirstOrDefault();
                        int bellCourseId;
                        if (DateOnly.FromDateTime(DateTime.Now.Date) >= sem2start)
                        {
                            var studentSchedule = await _context.sem2StudSchedules.FindAsync(studentId);
                            bellCourseId = GetS2BellCourseId(studentSchedule, bell.BellName);
                        } else
                        {
                            var studentSchedule = await _context.sem1StudSchedules.FindAsync(studentId);
                            bellCourseId = GetS1BellCourseId(studentSchedule, bell.BellName);
                        }

                        // If the course for this bell is in noncheckDailyCourses, skip to the next student
                        if (noncheckDailyCourses.Any(course => course.CourseId == bellCourseId))
                        {
                            continue;
                        }

                        var entryExists = _context.dailyAttendanceModels.Any(a =>
                            a.StudentId == studentId &&
                            a.AttendanceDate == date &&
                            a.Status == "Unknown");

                        // If an unknown status entry exists, mark it as Absent
                        if (entryExists)
                        {
                            var attendanceEntry = _context.dailyAttendanceModels.First(a =>
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
                            _context.timestampModels.Add(timeStampEntry);
                            _context.dailyAttendanceModels.Update(attendanceEntry);
                            await _context.SaveChangesAsync();
                        }
                    }
                }
            }
        }

        private int GetS1BellCourseId(Sem1StudSchedule studentSchedule, string bellName)
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

        private int GetS2BellCourseId(Sem2StudSchedule studentSchedule, string bellName)
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

        public object GetBellSchedule(string? chosenBellSchedName)
        {

            List<object> chosenBellSched;
            switch (chosenBellSchedName)
            {
                case "Daily Bell Schedule":
                    {
                        chosenBellSched = _context.dailyBellScheduleModels.OrderBy(a => a.StartTime).Cast<object>().ToList();
                        break;
                    }
                case "Extended Aves Bell Schedule":
                    {
                        chosenBellSched = _context.extendedAvesModels.OrderBy(a => a.StartTime).Cast<object>().ToList();
                        break;
                    }
                case "Pep Rally Bell Schedule":
                    {
                        chosenBellSched = _context.pepRallyBellScheduleModels.OrderBy(a => a.StartTime).Cast<object>().ToList();
                        break;
                    }
                case "Two Hour Delay Bell Schedule":
                    {
                        chosenBellSched = _context.twoHrDelayBellScheduleModels.OrderBy(a => a.StartTime).Cast<object>().ToList();
                        break;
                    }
                default:
                    {
                        throw new ArgumentException("Invalid bell schedule name provided.");
                    }
            }
            return chosenBellSched;
        }

    }
}
