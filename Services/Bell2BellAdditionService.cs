using Microsoft.AspNetCore.Identity;
using SAMS.Controllers;
using SAMS.Data;
using SAMS.Models;
using System;

namespace SAMS.Services
{
    public class Bell2BellAdditionService : BackgroundService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationUser> _roleManager;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<Bell2BellAdditionService> _logger;

        public Bell2BellAdditionService(UserManager<ApplicationUser> userManager, RoleManager<ApplicationUser> roleManager, ApplicationDbContext dbContext, ILogger<Bell2BellAdditionService> logger)
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
                                await BellToBellAttendanceAsync("Daily Bell Schedule");
                                break;
                            }
                            break;
                        }

                    case "Pep Rally Bell Schedule":
                        {
                            if (time >= peprallyStart && time <= new TimeSpan(07, 20, 00))
                            {
                                await BellToBellAttendanceAsync("Pep Rally Bell Schedule");
                                break;
                            }
                            break;
                        }

                    case "2 Hour Delay Bell Schedule":
                        {
                            if (time >= _2hrdelStart && time <= new TimeSpan(09, 20, 00))
                            {
                                await BellToBellAttendanceAsync("2 Hour Delay Bell Schedule");
                                break;
                            }
                            break;
                        }

                    case "Extended Aves Bell Schedule":
                        {
                            if (time >= extAvesStart && time <= new TimeSpan(07, 20, 00))
                            {
                                await BellToBellAttendanceAsync("Extended Aves Bell Schedule");
                                break;
                            }
                            break;
                        }
                    default:
                        {
                            //await Task.Delay(TimeSpan.FromMinutes(5), token);
                            break;
                        }
                }
            }
            else
            {
                await Task.Delay(TimeSpan.FromDays(1), token);
            }
            //await Task.CompletedTask;
        }

        private async Task BellToBellAttendanceAsync(string bellsChed)
        {
            var students = await _userManager.GetUsersInRoleAsync("Student");
            var chosenBellSchedu = bellsChed;
            var noncheckBellCourses = _context.activeCourseInfoModels.Where(a => a.B2BAttChecked == false).ToList();

            foreach (var student in students)
            {
                var studentId = int.Parse(student.SchoolId);
                var sem2start = _context.schedulerModels.Where(a => a.Type == "Semester 2").Select(a => a.Date).FirstOrDefault();
                int bellCourseId;

                for (int i = 1; i <= 7; i++)
                {
                    if (DateOnly.FromDateTime(DateTime.Now.Date) >= sem2start)
                    {
                        var studentSchedule = await _context.sem2StudSchedules.FindAsync(studentId);
                        bellCourseId = GetS2BellCourseId(studentSchedule, i);
                    }
                    else
                    {
                        var studentSchedule = await _context.sem1StudSchedules.FindAsync(studentId);
                        bellCourseId = GetS1BellCourseId(studentSchedule, i);
                    }

                    if (noncheckBellCourses.Any(course => course.CourseId == bellCourseId))
                    {
                        continue;
                    }
                    
                    var dateTime = DateTime.Now;
                    var time = dateTime.TimeOfDay;
                    TimeSpan dailyBellStart = new TimeSpan(7, 15, 00);
                    TimeSpan peprallyStart = new TimeSpan(7, 15, 00);
                    TimeSpan _2hrdelStart = new TimeSpan(9, 15, 00);
                    TimeSpan extAvesStart = new TimeSpan(7, 15, 00);
                    if (IsTransitionPeriod(chosenBellSchedu) || 
                        (time > dailyBellStart && time < new TimeSpan(07,20,00)) || 
                        (time > peprallyStart && time < new TimeSpan(07,20,00)) ||
                        (time > _2hrdelStart && time < new TimeSpan(09,15,00)) ||
                        (time > extAvesStart && time < new TimeSpan(07, 15, 00))
                        )
                    {
                        // Check if a bell attendance record already exists for the current student, next bell, and date
                        var entryExists = _context.bellAttendanceModels.Any(a =>
                            a.StudentId == studentId &&
                            a.DateTime.Date == DateTime.Now.Date &&
                            a.BellNumId == $"Bell {i}"
                            );
                            //&&
                            //a.BellNumId == i);

                        // If no record exists, create a new BellAttendanceModel with the appropriate details and save it to the database
                        if (!entryExists)
                        {
                            var newEntry = new BellAttendanceModel
                            {
                                StudentId = studentId,
                                DateTime = DateTime.Now,
                                Status = "Unknown",
                                ReasonForAbsence = "NA",
                                BellNumId = $"Bell {i}",
                                CourseId = bellCourseId,  // Assuming bellCourseId is the ScheduleId
                                ChosenBellSchedule = chosenBellSchedu
                            };

                            _context.bellAttendanceModels.Add(newEntry);
                            await _context.SaveChangesAsync();
                        }
                    }
                }
            }
        }

        private bool IsTransitionPeriod(string chosenBellSched)
        {
            // Get the current time
            var currentTime = DateTime.Now.TimeOfDay;
            var bellSchedule = _context;
            List<DailyBellScheduleModel> dailyBells;
            List<ExtendedAvesBellScheduleModel> extBells;
            List<PepRallyBellScheduleModel> pepBells;
            List<TwoHrDelayBellScheduleModel> twodelayBells;

            // Get the chosen bell schedule
            switch (chosenBellSched) {
                case "Daily Bell Schedule": {
                        dailyBells = bellSchedule.dailyBellScheduleModels.OrderBy(a => a.StartTime).ToList();
                        // Check if we are currently in a transition period
                        for (int i = 0; i < dailyBells.Count; i++)
                        {
                            if (dailyBells[i].BellName.Contains("Transition") && currentTime >= dailyBells[i].StartTime && currentTime < dailyBells[i].EndTime)
                            {
                                return true;
                            }
                        }
                        return false;
                    }
                case "Extended Aves Bell Schedule":
                    {
                        extBells = bellSchedule.extendedAvesModels.OrderBy(a => a.StartTime).ToList();
                        // Check if we are currently in a transition period
                        for (int i = 0; i < extBells.Count; i++)
                        {
                            if (extBells[i].BellName.Contains("Transition") && currentTime >= extBells[i].StartTime && currentTime < extBells[i].EndTime)
                            {
                                return true;
                            }
                        }
                        return false;
                    }
                case "Pep Rally Bell Schedule":
                    {
                        pepBells = bellSchedule.pepRallyBellScheduleModels.OrderBy(b => b.StartTime).ToList();
                        // Check if we are currently in a transition period
                        for (int i = 0; i < pepBells.Count; i++)
                        {
                            if (pepBells[i].BellName.Contains("Transition") && currentTime >= pepBells[i].StartTime && currentTime < pepBells[i].EndTime)
                            {
                                return true;
                            }
                        }
                        return false;
                    }
                case "Two Hour Delay Bell Schedule":
                    {
                        twodelayBells = bellSchedule.twoHrDelayBellScheduleModels.OrderBy(b => b.StartTime).ToList();
                        // Check if we are currently in a transition period
                        for (int i = 0; i < twodelayBells.Count; i++)
                        {
                            if (twodelayBells[i].BellName.Contains("Transition") && currentTime >= twodelayBells[i].StartTime && currentTime < twodelayBells[i].EndTime)
                            {
                                return true;
                            }
                        }
                        return false;
                    }
                default:
                    {
                        return false;
                    }
            }

            // Check if we are currently in a transition period
            /*for (int i = 0; i < bellSchedule.Count - 1; i++)
            {
                if (bellSchedule[i].BellName.Contains("Transition") && currentTime >= bellSchedule[i].StartTime && currentTime < bellSchedule[i].EndTime)
                {
                    return true;
                }
            }*/
        }

        private int GetS1BellCourseId(Sem1StudSchedule studentSchedule, int bell)
        {
            switch (bell)
            {
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
