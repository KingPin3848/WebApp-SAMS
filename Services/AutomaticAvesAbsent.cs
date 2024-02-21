
using Microsoft.AspNetCore.Identity;
using SAMS.Controllers;
using SAMS.Data;
using SAMS.Interfaces;
using SAMS.Models;

namespace SAMS.Services
{
    public class AutomaticAvesAbsent : BackgroundService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationUser> _roleManager;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AutomaticAvesAbsent> _logger;

        public AutomaticAvesAbsent(UserManager<ApplicationUser> userManager, RoleManager<ApplicationUser> roleManager, ApplicationDbContext context, ILogger<AutomaticAvesAbsent> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(!stoppingToken.IsCancellationRequested)
            {
                await MarkAbsentTask();

                await Task.Delay(TimeSpan.FromMinutes(1));
            }
        }

        private async Task MarkAbsentTask()
        {
            var date = DateTime.Now.Date;
            var time = DateTime.Now.TimeOfDay;
            var students = await _userManager.GetUsersInRoleAsync("Student");
            var noncheckDailyCourses = _context.activeCourseInfoModels.Where(a => a.DailyAttChecked == false).ToList();
            var chosenBellSchedName = _context.chosenBellSchedModels.Select(a => a.Name).FirstOrDefault();

            List<IBellSchedule>? chosenBellSched;
            switch (chosenBellSchedName)
            {
                case "Daily Bell Schedule":
                {
                    chosenBellSched = _context.dailyBellScheduleModels.Where(a => a.BellName.Contains("Aves")).OrderBy(a => a.StartTime).Cast<IBellSchedule>().ToList();
                    break;
                }
                case "Extended Aves Bell Schedule":
                {
                    chosenBellSched = _context.extendedAvesModels.Where(a => a.BellName.Contains("Aves")).OrderBy(a => a.StartTime).Cast<IBellSchedule>().ToList();
                    break;
                }
                default:
                {
                    chosenBellSched = null!;
                    break; 
                }
            }

            if (chosenBellSched == null)
            {
                await Task.CompletedTask;
            } else
            {
                foreach (var bellObject in chosenBellSched)
                {
                    IBellSchedule bell = bellObject;
                    // Check if we are 15 minutes into the current bell
                    if (time >= bell.StartTime.Add(TimeSpan.FromMinutes(10)) && time < bell.EndTime)
                    {
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
                            var bellCourseName = _context.activeCourseInfoModels.Find(bellCourseId).CourseName;

                            // If the course for this bell is in noncheckDailyCourses, skip to the next student
                            if (noncheckDailyCourses.Any(course => course.CourseId == bellCourseId))
                            {
                                continue;
                            }

                            var entryExists = _context.bellAttendanceModels.Any(a =>
                                a.StudentId == studentId &&
                                a.DateTime.Date == date &&
                                a.BellNumId.Contains("Aves Bell") &&
                                a.Status == "Unknown");

                            // If an unknown status entry exists, mark it as Absent
                            if (entryExists)
                            {
                                var attendanceEntry = _context.bellAttendanceModels.First(a =>
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
                                _context.bellAttendanceModels.Update(attendanceEntry);
                                await _context.SaveChangesAsync();
                            }
                        }
                    }
                }
            }
        }

        private int GetS1BellCourseId(Sem1StudSchedule? studentSchedule)
        {
            return studentSchedule.AvesBellCourseIDMod;
        }

        private int GetS2BellCourseId(Sem2StudSchedule? studentSchedule)
        {
            return studentSchedule.AvesBellCourseIDMod;
        }
    }
}
