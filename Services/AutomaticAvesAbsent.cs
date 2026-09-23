using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SAMS.Data;
using SAMS.Interfaces;
using SAMS.Models;

namespace SAMS.Services
{
    public class AutomaticAvesAbsent(IServiceScopeFactory scopeFactory, ILogger<AutomaticAvesAbsent> logger) : BackgroundService
    {
        private static readonly TimeSpan PollInterval = TimeSpan.FromMinutes(2);
        private static readonly TimeSpan AttendanceGracePeriod = TimeSpan.FromMinutes(5);

        private readonly IServiceScopeFactory _scopeFactory = scopeFactory;
        private readonly ILogger<AutomaticAvesAbsent> _logger = logger;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await RunCycleAsync();
                await Task.Delay(PollInterval, stoppingToken);
            }
        }

        private async Task RunCycleAsync()
        {
            using var scope = _scopeFactory.CreateAsyncScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var today = DateOnly.FromDateTime(DateTime.Now.Date);
            var isHoliday = context.SchedulerModels.Any(a => a.Type == SchedulerModel.Types.NoSchool && a.Date == today);
            if (isHoliday)
            {
                _logger.LogInformation("Skipping automatic Aves attendance because today is marked as a no-school day.");
                return;
            }

            await MarkAbsentTaskAsync(context, scope.ServiceProvider);
        }

        private async Task MarkAbsentTaskAsync(ApplicationDbContext context, IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var students = await userManager.GetUsersInRoleAsync("Student");

            var nonCheckDailyCourseIds = context.ActiveCourseInfoModels
                .Where(a => a.DailyAttChecked == false)
                .Select(a => a.CourseId)
                .ToHashSet();

            var nonCheckBellCourseIds = context.ActiveCourseInfoModels
                .Where(a => a.B2BAttChecked == false)
                .Select(a => a.CourseId)
                .ToHashSet();

            var chosenScheduleName = context.ChosenBellSchedModels.Select(a => a.Name).FirstOrDefault();
            var chosenBellSchedule = GetChosenAvesSchedule(context, chosenScheduleName);
            if (chosenBellSchedule.Count == 0)
            {
                return;
            }

            await ProcessAvesBellAttendanceAsync(context, students, nonCheckDailyCourseIds, nonCheckBellCourseIds, chosenBellSchedule);
        }

        private List<IBellSchedule> GetChosenAvesSchedule(ApplicationDbContext context, string? chosenScheduleName)
        {
            switch (chosenScheduleName)
            {
                case "Daily Bell Schedule":
                    return [.. context.DailyBellScheduleModels
                        .Where(a => a.BellName.Contains("Aves"))
                        .OrderBy(a => a.StartTime)
                        .Cast<IBellSchedule>()];

                case "Extended Aves Bell Schedule":
                    return [.. context.ExtendedAvesModels
                        .Where(a => a.BellName.Contains("Aves"))
                        .OrderBy(a => a.StartTime)
                        .Cast<IBellSchedule>()];

                case "Custom Bell Schedule":
                    var hasAvesBell = context.CustomSchedules.Any(a => a.BellName.Contains("Aves Bell"));
                    if (!hasAvesBell)
                    {
                        _logger.LogInformation("No Aves bell found in the custom bell schedule.");
                        return [];
                    }

                    return [.. context.CustomSchedules
                        .Where(a => a.BellName.Contains("Aves"))
                        .OrderBy(a => a.StartTime)
                        .Cast<IBellSchedule>()];

                default:
                    _logger.LogInformation("Unsupported schedule selected for automatic Aves attendance processing.");
                    return [];
            }
        }

        private async Task ProcessAvesBellAttendanceAsync(
            ApplicationDbContext context,
            IList<ApplicationUser> students,
            HashSet<int> nonCheckDailyCourseIds,
            HashSet<int> nonCheckBellCourseIds,
            List<IBellSchedule> chosenBellSchedule)
        {
            var now = DateTime.Now;
            var currentDate = now.Date;
            var currentTime = now.TimeOfDay;

            var avesBell = chosenBellSchedule.FirstOrDefault(b => b.BellName == "Aves Bell");
            if (avesBell is null)
            {
                _logger.LogInformation("No exact Aves Bell slot found in selected schedule.");
                return;
            }

            if (currentTime < avesBell.StartTime.Add(AttendanceGracePeriod) || currentTime >= avesBell.EndTime)
            {
                return;
            }

            var sem2Start = context.SchedulerModels
                .Where(a => a.Type == SchedulerModel.Types.Semester2)
                .Select(a => a.Date)
                .FirstOrDefault();

            var isSemester2 = DateOnly.FromDateTime(currentDate) >= sem2Start;
            var hasAttendanceUpdates = false;

            foreach (var student in students)
            {
                if (!int.TryParse(student.SchoolId, out var studentId))
                {
                    _logger.LogWarning("Unable to parse SchoolId for student {UserId}.", student.Id);
                    continue;
                }

                var studentSchedule = await GetStudentScheduleAsync(context, studentId, isSemester2);
                if (studentSchedule is null)
                {
                    continue;
                }

                var bellCourseId = GetBellCourseId(studentSchedule);
                var bellCourseName = context.ActiveCourseInfoModels
                    .Where(a => a.CourseId == bellCourseId)
                    .Select(a => a.CourseName)
                    .FirstOrDefault() ?? "Unknown";

                if (nonCheckDailyCourseIds.Contains(bellCourseId) || nonCheckBellCourseIds.Contains(bellCourseId))
                {
                    _logger.LogInformation(
                        "Skipping automatic absence for non-check course {CourseId} {CourseName}.",
                        bellCourseId,
                        bellCourseName);
                    continue;
                }

                hasAttendanceUpdates |= await MarkUnknownAvesEntryAbsentAsync(context, studentId, currentDate);
            }

            if (hasAttendanceUpdates)
            {
                await context.SaveChangesAsync();
            }
        }

        private static async Task<IStudentSchedule?> GetStudentScheduleAsync(ApplicationDbContext context, int studentId, bool isSemester2)
        {
            return isSemester2
                ? await context.Sem2StudSchedules.FindAsync(studentId)
                : await context.Sem1StudSchedules.FindAsync(studentId);
        }

        private async Task<bool> MarkUnknownAvesEntryAbsentAsync(ApplicationDbContext context, int studentId, DateTime date)
        {
            var attendanceEntry = await context.BellAttendanceModels.FirstOrDefaultAsync(a =>
                a.StudentId == studentId &&
                a.DateTime.Date == date &&
                a.BellNumId.Contains("Aves Bell") &&
                a.Status == "Unknown");

            if (attendanceEntry is null)
            {
                return false;
            }

            attendanceEntry.Status = "Absent";
            attendanceEntry.ReasonForAbsence = "Not confirmed. Student was marked absent automatically because the student did not check themselves into the aves bell class within 5 minutes of the bell start. Contact the SHS Attendance Office for any questions or concerns.";
            return true;
        }

        private static int GetBellCourseId(IStudentSchedule studentSchedule)
        {
            return studentSchedule.AvesBellCourseIDMod;
        }
    }
}
