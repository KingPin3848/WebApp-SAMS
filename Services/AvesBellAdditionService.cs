
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NuGet.Common;
using SAMS.Controllers;
using SAMS.Data;
using SAMS.Models;

namespace SAMS.Services
{
    public class AvesBellAdditionService : BackgroundService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationUser> _roleManager;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AvesBellAdditionService> _logger;

        public AvesBellAdditionService(UserManager<ApplicationUser> userManager, RoleManager<ApplicationUser> roleManager, ApplicationDbContext context, ILogger<AvesBellAdditionService> logger)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Scheduler(stoppingToken);
            }
            throw new NotImplementedException();
        }

        private async Task Scheduler(CancellationToken token)
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
                                await GenerateAttendanceFieldsDailyAttTask("Daily Bell Schedule");
                                break;
                            }
                            break;
                        }

                    case "Extended Aves Bell Schedule":
                        {
                            if (time >= extAvesStart && time <= new TimeSpan(07, 20, 00))
                            {
                                await GenerateAttendanceFieldsDailyAttTask("Extended Aves Bell Schedule");
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

        private async Task GenerateAttendanceFieldsDailyAttTask(string bellsChed)
        {
            var students = await _userManager.GetUsersInRoleAsync("Student");
            var chosenBellSchedu = bellsChed;
            var noncheckBellCourses = _context.activeCourseInfoModels.Where(a => a.B2BAttChecked == false).ToList();

            foreach (var student in students)
            {
                var studentId = int.Parse(student.SchoolId);
                var sem2start = _context.schedulerModels.Where(a => a.Type == "Semester 2").Select(a => a.Date).FirstOrDefault();
                int bellCourseId;

                if (DateOnly.FromDateTime(DateTime.Now.Date) >= sem2start)
                {
                    var studentSchedule = await _context.sem2StudSchedules.FindAsync(studentId);
                    bellCourseId = GetS2BellCourseId(studentSchedule);
                }
                else
                {
                    var studentSchedule = await _context.sem1StudSchedules.FindAsync(studentId);
                    bellCourseId = GetS1BellCourseId(studentSchedule);
                }

                if (noncheckBellCourses.Any(course => course.CourseId == bellCourseId))
                {
                    continue;
                }

                var dateTime = DateTime.Now;
                var time = dateTime.TimeOfDay;
                TimeSpan dailyBellStart = new TimeSpan(7, 15, 00);
                TimeSpan extAvesStart = new TimeSpan(7, 15, 00);
                if (IsTransitionPeriod(chosenBellSchedu))
                {
                    // Check if a bell attendance record already exists for the current student, next bell, and date
                    var entryExists = _context.bellAttendanceModels.Any(a =>
                        a.StudentId == studentId &&
                        a.DateTime.Date == DateTime.Now.Date &&
                        a.BellNumId.Contains("Aves Bell")
                        );
                    //&&
                    //a.BellNumId == i);

                    // If no record exists, create a new BellAttendanceModel with the appropriate details and save it to the database
                    if (!entryExists)
                    {
                        if (chosenBellSchedu == "Daily Bell Schedule")
                        {
                            var newEntry = new BellAttendanceModel
                            {
                                StudentId = studentId,
                                DateTime = DateTime.Now,
                                Status = "Unknown",
                                ReasonForAbsence = "NA",
                                BellNumId = "Aves Bell",
                                CourseId = bellCourseId,  // Assuming bellCourseId is the ScheduleId
                                ChosenBellSchedule = chosenBellSchedu
                            };
                            _context.bellAttendanceModels.Add(newEntry);
                        }
                        else if (chosenBellSchedu == "Extended Aves Bell Schedule")
                        {
                            var newEntry = new BellAttendanceModel
                            {
                                StudentId = studentId,
                                DateTime = DateTime.Now,
                                Status = "Unknown",
                                ReasonForAbsence = "NA",
                                BellNumId = "Extended Aves Bell",
                                CourseId = bellCourseId,  // Assuming bellCourseId is the ScheduleId
                                ChosenBellSchedule = chosenBellSchedu
                            };
                            _context.bellAttendanceModels.Add(newEntry);
                        }
                        await _context.SaveChangesAsync();
                    }
                }
            }
        }

        private int GetS1BellCourseId(Sem1StudSchedule studentSchedule)
        {
            return studentSchedule.AvesBellCourseIDMod;
        }

        private int GetS2BellCourseId(Sem2StudSchedule studentSchedule)
        {
            return studentSchedule.AvesBellCourseIDMod;
        }

        private bool IsTransitionPeriod(string chosenBellSched)
        {
            // Get the current time
            var currentTime = DateTime.Now.TimeOfDay;
            var bellSchedule = _context;
            List<DailyBellScheduleModel> dailyBells;
            List<ExtendedAvesBellScheduleModel> extBells;

            // Get the chosen bell schedule
            switch (chosenBellSched)
            {
                case "Daily Bell Schedule":
                    {
                        dailyBells = bellSchedule.dailyBellScheduleModels.OrderBy(a => a.StartTime).ToList();
                        // Check if we are currently in a transition period
                        for (int i = 0; i < dailyBells.Count; i++)
                        {
                            if (dailyBells[i].BellName.Contains("Transition 2") && currentTime >= dailyBells[i].StartTime && currentTime < dailyBells[i].EndTime)
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
                            if (extBells[i].BellName.Contains("Transition 2") && currentTime >= extBells[i].StartTime && currentTime < extBells[i].EndTime)
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
    }
}
