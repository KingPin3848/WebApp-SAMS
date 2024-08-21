using Microsoft.AspNetCore.Identity;
using SAMS.Controllers;
using SAMS.Data;
using SAMS.Interfaces;
using SAMS.Models;

namespace SAMS.Services
{
    public class AvesBellAdditionService(ILogger<AvesBellAdditionService> logger, IServiceScopeFactory scopefactory) : BackgroundService
    {
        private readonly ILogger<AvesBellAdditionService> _logger = logger;
        private readonly IServiceScopeFactory _scopefactory = scopefactory;


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await HolidayRun();
            }
        }

        private async Task HolidayRun()
        {
            using var scope = _scopefactory.CreateAsyncScope();

            //Scoped DbContext
            var _context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            //List of all holiday dates
            var holidayDates = _context.SchedulerModels.Where(a => a.Type == SchedulerModel.Types.NoSchool).Select(a => a.Date).ToList();
            //Today's date
            var todayDate = DateOnly.FromDateTime(DateTime.Now.Date);

            //Null reference check for holidayDates
            if (holidayDates == null)
            {
                _logger.LogWarning("Holidays is null and the task if delayed by 1 DAY. Done by the if statement in holidayRun");
                await Task.Delay(TimeSpan.FromDays(1));
            }
            else
            {
                //Going through each holiday date and comparing it with today's date. If same, then we DO NOT generate data. Else, we DO generate data.
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
            using var scope = _scopefactory.CreateAsyncScope();

            //Scoped DbContext
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            //Get the bell schedule chosen for the day
            var chosenBellSched = context.ChosenBellSchedModels.Select(a => a.Name).ToList();
            //Get today's date and time
            var dateTime = DateTime.Now;
            //Get today's day (like Monday or Tuesday, etc.)
            var day = dateTime.DayOfWeek;

            //Checking if the day is a weekday. If it is, then continue the algorithm. Else, delay the task for a day.
            if (day == DayOfWeek.Monday || day == DayOfWeek.Tuesday || day == DayOfWeek.Wednesday || day == DayOfWeek.Thursday || day == DayOfWeek.Friday)
            {
                var time = dateTime.TimeOfDay;

                switch (chosenBellSched[0])
                {
                    case "Daily Bell Schedule":
                        {
                            if (IsTransitionPeriod("Daily Bell Schedule"))
                            {
                                await GenerateAttendanceFieldsDailyAttTask("Daily Bell Schedule");
                                break;
                            }
                            break;
                        }
                    case "Extended Aves Bell Schedule":
                        {
                            if (IsTransitionPeriod("Extended Aves Bell Schedule"))
                            {
                                await GenerateAttendanceFieldsDailyAttTask("Extended Aves Bell Schedule");
                                break;
                            }
                            break;
                        }
                    case "Custom Bell Schedule":
                        {
                            TimeSpan avesstart = context.CustomSchedules.Where(a => a.BellName == "Aves Bell").Select(a => a.StartTime).DefaultIfEmpty(TimeSpan.Zero).First();
                            if (avesstart == TimeSpan.Zero)
                            {
                                _logger.LogInformation("There is no aves bell start present in the custom schedule. The service will now be delayed for ONE DAY.");
                                await Task.Delay(TimeSpan.FromDays(1.0));
                            }
                            else
                            {
                                if (time >= avesstart.Subtract(TimeSpan.FromMinutes(5.0)) && time <= avesstart)
                                {
                                    await GenerateAttendanceFieldsDailyAttTask("Custom Bell Schedule");
                                    break;
                                }
                                break;
                            }
                            break;
                        }
                    default:
                        {
                            await Task.Delay(TimeSpan.FromDays(1));
                            break;
                        }
                }
            }
            else
            {
                await Task.Delay(TimeSpan.FromDays(1));
            }
        }

        private async Task GenerateAttendanceFieldsDailyAttTask(string bellsChed)
        {
            using var scope = _scopefactory.CreateAsyncScope();
            //Scoped DbContext
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            //Get all users who are assigned Student role
            var students = await userManager.GetUsersInRoleAsync("Student");
            //Get all courses that have Bell-to-bell attendance made false
            var noncheckBellCourses = context.ActiveCourseInfoModels.Where(a => a.B2BAttChecked == false).ToList();
            //Get just today's date
            var date = DateOnly.FromDateTime(DateTime.Now.Date);

            switch (bellsChed)
            {
                case "Daily Bell Schedule":
                    {
                        await NormalAdditionRunner(bellsChed, students, noncheckBellCourses);
                        break;
                    }
                case "Extended Aves Bell Schedule":
                    {
                        await NormalAdditionRunner(bellsChed, students, noncheckBellCourses);
                        break;
                    }
                case "Custom Bell Schedule":
                    {
                        await CustomAdditionRunner(bellsChed, students, noncheckBellCourses);
                        break;
                    }
                default:
                    {
                        _logger.LogInformation("A schedule was chosen other than daily, extended, and custom which doesn't have aves bell. No need to further generate fields. If somehow the service reached here, task is delayed by ONE DAY.");
                        await Task.Delay(TimeSpan.FromDays(1));
                        break;
                    }
            }
        }

        private async Task CustomAdditionRunner(string v, IList<ApplicationUser> users, List<ActiveCourseInfoModel> courses)
        {
            using var scope = _scopefactory.CreateAsyncScope();
            //Scoped DbContext
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            //Checking if the time (within the next method) is a transition period
            if (IsTransitionPeriod(v))
            {
                //Going through each student 
                foreach (var student in users)
                {
                    //Each student's Id
                    var studentId = int.Parse(student.SchoolId!);

                    //Getting the data entry of Semester 2 for the year
                    var sem2start = context.SchedulerModels.Where(a => a.Type == SchedulerModel.Types.Semester2).Select(a => a.Date).First();
                    //Based on the semester found (1 or 2), getting the student's schedule for that semester
                    IStudentSchedule? studentSchedule = (DateOnly.FromDateTime(DateTime.Now.Date) >= sem2start) ? (await context.Sem2StudSchedules.FindAsync(studentId)) : (await context.Sem1StudSchedules.FindAsync(studentId));
                    //If semester 2 has started or not.
                    var sem2started = (DateOnly.FromDateTime(DateTime.Now.Date) >= sem2start);
                    int bellCourseId;


                    //Getting the courseId for the semester
                    if (sem2started)
                    {
                        bellCourseId = GetS2BellCourseId(studentSchedule!);
                    }
                    else
                    {
                        bellCourseId = GetS1BellCourseId(studentSchedule!);
                    }

                    //Checking if the aves bell course id is the same as one found in noncheck courses
                    if (courses.Any(course => course.CourseId == bellCourseId))
                    {
                        foreach (var item in courses)
                        {
                            _logger.LogInformation("We don't add this courseId {CourseId} for aves bell.", bellCourseId);
                        }
                        continue;
                    }

                    //Today's date and time and the time of run
                    var dateTime = DateTime.Now;
                    //Just time for the day
                    var time = dateTime.TimeOfDay;
                    //Start time of the aves bell in custom schedule, IF there is aves bell present in the data
                    TimeSpan customAvesStart = context.CustomSchedules.Where(a => a.BellName.Contains("Aves Bell")).Select(a => a.StartTime).DefaultIfEmpty(TimeSpan.Zero).First();

                    //Checking if the start time is zero if it could not be found, and delay the task for ONE Day.
                    if (customAvesStart == TimeSpan.Zero)
                    {
                        _logger.LogInformation("No start for Aves Bell available in the entered data. Task will be delayed by ONE DAY.");
                        await Task.Delay(TimeSpan.FromDays(1));
                    }
                    else
                    {
                        // Check if a bell attendance record already exists for the current student, next bell, and date
                        var entryExists = context.BellAttendanceModels.Any(a =>
                            a.StudentId == studentId &&
                            a.DateTime.Date == DateTime.Now.Date &&
                            a.BellNumId.Contains("Aves Bell")
                            );

                        // If no record exists, create a new BellAttendanceModel with the appropriate details and save it to the database
                        if (!entryExists)
                        {
                            //Another check to make sure that the chosen bell schedule is custom bell schedule before any changes have been made to the database tracker
                            if (v == "Custom Bell Schedule")
                            {
                                var newEntry = new BellAttendanceModel
                                {
                                    StudentId = studentId,
                                    DateTime = DateTime.Now,
                                    Status = "Unknown",
                                    ReasonForAbsence = "NA",
                                    BellNumId = "Aves Bell",
                                    CourseId = bellCourseId,
                                    ChosenBellSchedule = v
                                };
                                context.BellAttendanceModels.Add(newEntry);
                                await context.SaveChangesAsync();
                            }
                            else
                            {
                                _logger.LogInformation("It isn't a custom bell schedule. Something went wrong with the request and data changes.");
                                break;
                            }
                        }
                    }
                }
            }
            else
            {
                await Task.Delay(TimeSpan.FromMinutes(2));
            }
        }

        private async Task NormalAdditionRunner(string v, IList<ApplicationUser> users, List<ActiveCourseInfoModel> courses)
        {
            using var scope = _scopefactory.CreateAsyncScope();

            //Scoped DbContext
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();


            if (IsTransitionPeriod(v))
            {
                foreach (var student in users)
                {
                    var studentId = int.Parse(student.SchoolId!);

                    var sem2start = context.SchedulerModels.Where(a => a.Type == SchedulerModel.Types.Semester2).Select(a => a.Date).FirstOrDefault();
                    IStudentSchedule? studentSchedule = (DateOnly.FromDateTime(DateTime.Now.Date) >= sem2start) ? (await context.Sem2StudSchedules.FindAsync(studentId)) : (await context.Sem1StudSchedules.FindAsync(studentId));
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

                    if (courses.Any(course => course.CourseId == bellCourseId))
                    {
                        foreach (var item in courses)
                        {
                            _logger.LogInformation("We don't add this courseId {CourseId} for aves bell.", bellCourseId);
                        }
                        continue;
                    };


                    // Check if a bell attendance record already exists for the current student, next bell, and date
                    var entryExists = context.BellAttendanceModels.Any(a =>
                        a.StudentId == studentId &&
                        a.DateTime.Date == DateTime.Now.Date &&
                        a.BellNumId.Contains("Aves Bell")
                        );

                    // If no record exists, create a new BellAttendanceModel with the appropriate details and save it to the database
                    if (!entryExists)
                    {
                        switch (v)
                        {
                            case "Daily Bell Schedule":
                                {
                                    var newEntry = new BellAttendanceModel
                                    {
                                        StudentId = studentId,
                                        DateTime = DateTime.Now,
                                        Status = "Unknown",
                                        ReasonForAbsence = "NA",
                                        BellNumId = "Aves Bell",
                                        CourseId = bellCourseId,
                                        ChosenBellSchedule = v
                                    };
                                    context.BellAttendanceModels.Add(newEntry);
                                    break;
                                }
                            case "Extended Aves Bell Schedule":
                                {
                                    var newEntry = new BellAttendanceModel
                                    {
                                        StudentId = studentId,
                                        DateTime = DateTime.Now,
                                        Status = "Unknown",
                                        ReasonForAbsence = "NA",
                                        BellNumId = "Extended Aves Bell",
                                        CourseId = bellCourseId,
                                        ChosenBellSchedule = v
                                    };
                                    context.BellAttendanceModels.Add(newEntry);
                                    break;
                                }
                            default:
                                {
                                    _logger.LogInformation("A schedule was chosen other than Daily, Extended Aves, and Custom bell schedules.");
                                    break;
                                }
                        }
                        await context.SaveChangesAsync();
                    }
                }
            }
            else
            {
                await Task.Delay(TimeSpan.FromMinutes(2));
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

        private bool IsTransitionPeriod(string chosenBellSched)
        {
            using var scope = _scopefactory.CreateAsyncScope();

            //Scoped DbContext
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            // Get the current time
            var currentTime = DateTime.Now.TimeOfDay;

            // Get the chosen bell schedule
            switch (chosenBellSched)
            {
                case "Daily Bell Schedule":
                    {
                        var dailyBells = context.DailyBellScheduleModels.OrderBy(a => a.StartTime).Where(a => a.BellName == "Transition 2").ToList();
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
                        var extBells = context.ExtendedAvesModels.OrderBy(a => a.StartTime).Where(a => a.BellName == "Transition 2").ToList();
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
                case "Custom Bell Schedule":
                    {
                        var custBells = context.CustomSchedules.OrderBy(a => a.StartTime).ToList();
                        if (custBells == null)
                        {
                            _logger.LogCritical("There is no Aves Bell.");
                            return false;
                        }
                        int indexAves = custBells.FindIndex(a => a.BellName == "Aves Bell");
                        if (indexAves == -1)
                        {
                            _logger.LogCritical("Null reference. Couldn't find the index of Aves Bell");
                        }
                        var hasAvesTransition = custBells[indexAves - 1].BellName.Contains("Transition");
                        if (hasAvesTransition)
                        {
                            if ((currentTime >= custBells[indexAves - 1].StartTime) && (currentTime < custBells[indexAves - 1].EndTime))
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            if ((currentTime >= custBells[indexAves - 1].EndTime.Subtract(TimeSpan.FromMinutes(2))) && (currentTime <= custBells[indexAves].StartTime.Add(TimeSpan.FromMinutes(2))))
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                default:
                    {
                        _logger.LogInformation("Some other schedule was chosen, and hence IsTransitionPeriod will return false since schedules other than Daily, Extended Aves, and Custom (exceptions excluded) don't have Aves Bell.");
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
