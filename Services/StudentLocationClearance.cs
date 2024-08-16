
using SAMS.Data;
using SAMS.Models;

#pragma warning disable CA1848
namespace SAMS.Services
{
    public class StudentLocationClearance(ILogger<StudentLocationClearance> ilogger, IServiceScopeFactory scopefactory) : BackgroundService
    {
        private readonly ILogger<StudentLocationClearance> _logger = ilogger;
        private readonly IServiceScopeFactory scopeFactory = scopefactory;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await HolidayRun(stoppingToken).ConfigureAwait(true);
        }

        private async Task HolidayRun(CancellationToken stoppingToken)
        {
            using var scope = scopeFactory.CreateAsyncScope();

            var _context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var holidayDates = _context.SchedulerModels.Where(a => a.Type == "No School @SHS").Select(a => a.Date).ToList();
            var todayDate = DateOnly.FromDateTime(DateTime.Now.Date);

            if (holidayDates == null)
            {
                _logger.LogWarning("Holidays is null and the task if delayed by 1 DAY. Done by the if statement in holidayRun");
                await Task.Delay(TimeSpan.FromDays(1), stoppingToken).ConfigureAwait(true);
            }
            else
            {
                foreach (var date in holidayDates)
                {
                    if (date == todayDate)
                    {
                        _logger.LogWarning("Today is a holiday and the task is delayed by 1 DAY. Done by the if statement in holidayRun");
                        await Task.Delay(TimeSpan.FromDays(1), stoppingToken).ConfigureAwait(true);
                    }
                    else
                    {
                        await ScheduleRunner(stoppingToken).ConfigureAwait(true);
                    }
                }
            }
        }

        private async Task ScheduleRunner(CancellationToken stoppingToken)
        {
            using var scope = scopeFactory.CreateAsyncScope();

            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var chosenBellSched = context.ChosenBellSchedModels.Select(a => a.Name).First();
            var dateTime = DateTime.Now;
            var day = DateTime.Now.DayOfWeek;

            if (day == DayOfWeek.Monday || day == DayOfWeek.Tuesday || day == DayOfWeek.Wednesday || day == DayOfWeek.Thursday || day == DayOfWeek.Friday)
            {
                switch (chosenBellSched)
                {
                    case "Daily Bell Schedule":
                        {
                            var endOfDayTime = context.DailyBellScheduleModels.Last().EndTime;

                            if (dateTime.TimeOfDay >= endOfDayTime && dateTime.TimeOfDay <= new TimeSpan(23,59,59))
                            {
                                var studentLocations = context.StudentLocationModels.ToList();
                                foreach (var location in studentLocations)
                                {
                                    if (location.StudentLocation is not "Out of School (Not in Session)")
                                    {
                                        var newLocation = new StudentLocationModel()
                                        {
                                            StudentIdMod = location.StudentIdMod,
                                            StudentName = location.StudentName,
                                            StudentLocation = "Out of School (Not in Session)"
                                        };
                                        context.Entry(newLocation).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                                        if (!context.SaveChangesAsync(stoppingToken).IsCompletedSuccessfully)
                                        {
                                            context.Entry(newLocation).State = Microsoft.EntityFrameworkCore.EntityState.Unchanged;
                                            await context.SaveChangesAsync(stoppingToken).ConfigureAwait(false);
                                        }
                                    }
                                }
                            }
                            break;
                        }

                    case "Pep Rally Bell Schedule":
                        {
                            var endOfDayTime = context.PepRallyBellScheduleModels.Last().EndTime;

                            if (dateTime.TimeOfDay >= endOfDayTime && dateTime.TimeOfDay <= new TimeSpan(23, 59, 59))
                            {
                                var studentLocations = context.StudentLocationModels.ToList();
                                foreach (var location in studentLocations)
                                {
                                    if (location.StudentLocation is not "Out of School (Not in Session)")
                                    {
                                        var newLocation = new StudentLocationModel()
                                        {
                                            StudentIdMod = location.StudentIdMod,
                                            StudentName = location.StudentName,
                                            StudentLocation = "Out of School (Not in Session)"
                                        };
                                        context.Entry(newLocation).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                                        if (!context.SaveChangesAsync(stoppingToken).IsCompletedSuccessfully)
                                        {
                                            context.Entry(newLocation).State = Microsoft.EntityFrameworkCore.EntityState.Unchanged;
                                            await context.SaveChangesAsync(stoppingToken).ConfigureAwait(false);
                                        }
                                    }
                                }
                            }
                            break;
                        }

                    case "2 Hour Delay Bell Schedule":
                        {
                            var endOfDayTime = context.TwoHrDelayBellScheduleModels.Last().EndTime;

                            if (dateTime.TimeOfDay >= endOfDayTime && dateTime.TimeOfDay <= new TimeSpan(23, 59, 59))
                            {
                                var studentLocations = context.StudentLocationModels.ToList();
                                foreach (var location in studentLocations)
                                {
                                    if (location.StudentLocation is not "Out of School (Not in Session)")
                                    {
                                        var newLocation = new StudentLocationModel()
                                        {
                                            StudentIdMod = location.StudentIdMod,
                                            StudentName = location.StudentName,
                                            StudentLocation = "Out of School (Not in Session)"
                                        };
                                        context.Entry(newLocation).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                                        if (!context.SaveChangesAsync(stoppingToken).IsCompletedSuccessfully)
                                        {
                                            context.Entry(newLocation).State = Microsoft.EntityFrameworkCore.EntityState.Unchanged;
                                            await context.SaveChangesAsync(stoppingToken).ConfigureAwait(false);
                                        }
                                    }
                                }
                            }
                            break;
                        }

                    case "Extended Aves Bell Schedule":
                        {
                            var endOfDayTime = context.ExtendedAvesModels.Last().EndTime;

                            if (dateTime.TimeOfDay >= endOfDayTime && dateTime.TimeOfDay <= new TimeSpan(23, 59, 59))
                            {
                                var studentLocations = context.StudentLocationModels.ToList();
                                foreach (var location in studentLocations)
                                {
                                    if (location.StudentLocation is not "Out of School (Not in Session)")
                                    {
                                        var newLocation = new StudentLocationModel()
                                        {
                                            StudentIdMod = location.StudentIdMod,
                                            StudentName = location.StudentName,
                                            StudentLocation = "Out of School (Not in Session)"
                                        };
                                        context.Entry(newLocation).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                                        if (!context.SaveChangesAsync(stoppingToken).IsCompletedSuccessfully)
                                        {
                                            context.Entry(newLocation).State = Microsoft.EntityFrameworkCore.EntityState.Unchanged;
                                            await context.SaveChangesAsync(stoppingToken).ConfigureAwait(false);
                                        }
                                    }
                                }
                            }
                            break;
                        }

                    case "Custom Bell Schedule":
                        {
                            var endOfDayTime = context.CustomSchedules.Last().EndTime;

                            if (dateTime.TimeOfDay >= endOfDayTime && dateTime.TimeOfDay <= new TimeSpan(23, 59, 59))
                            {
                                var studentLocations = context.StudentLocationModels.ToList();
                                foreach (var location in studentLocations)
                                {
                                    if (location.StudentLocation is not "Out of School (Not in Session)")
                                    {
                                        var newLocation = new StudentLocationModel()
                                        {
                                            StudentIdMod = location.StudentIdMod,
                                            StudentName = location.StudentName,
                                            StudentLocation = "Out of School (Not in Session)"
                                        };
                                        context.Entry(newLocation).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                                        if (!context.SaveChangesAsync(stoppingToken).IsCompletedSuccessfully)
                                        {
                                            context.Entry(newLocation).State = Microsoft.EntityFrameworkCore.EntityState.Unchanged;
                                            await context.SaveChangesAsync(stoppingToken).ConfigureAwait(false);
                                        }
                                    }
                                }
                            }
                            break;
                        }

                    default:
                        {
                            _logger.LogInformation("The task is supposed to be delayed for 1 DAY. Done by default case in ScheduleRunner of QRCode Updater.");
                            await Task.Delay(TimeSpan.FromDays(1), stoppingToken).ConfigureAwait(true);
                            break;
                        }
                }
            }
            else
            {
                _logger.LogWarning("The task is going to be delayed for 1 DAY. Done the by the else statement @line 136 in ScheduleRunner.");
                await Task.Delay(TimeSpan.FromDays(1), stoppingToken).ConfigureAwait(true);
            }
        }
    }
}
#pragma warning restore CA1848