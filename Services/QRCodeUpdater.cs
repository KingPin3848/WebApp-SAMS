using SAMS.Data;
using SAMS.Models;
using System.Security.Cryptography;
using System.Text;

#pragma warning disable CA1848
namespace SAMS.Services
{
    public class QRCodeUpdater(IServiceScopeFactory scopefactory, ILogger<QRCodeUpdater> logger) : BackgroundService
    {
        private readonly IServiceScopeFactory scopeFactory = scopefactory;
        private readonly ILogger<QRCodeUpdater> _logger = logger;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await HolidayRun(stoppingToken).ConfigureAwait(true);
            }
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
            var day = dateTime.DayOfWeek;

            if (day == DayOfWeek.Monday || day == DayOfWeek.Tuesday || day == DayOfWeek.Wednesday || day == DayOfWeek.Thursday || day == DayOfWeek.Friday)
            {
                var time = dateTime.TimeOfDay;

                switch (chosenBellSched)
                {
                    case "Daily Bell Schedule":
                        {
                            TimeSpan dailyBellStart = new(7, 15, 00);
                            if (time >= dailyBellStart && time <= new TimeSpan(07, 15, 00))
                            {
                                var schedule = context.DailyBellScheduleModels.ToList();
                                await DailyQRCodeUpdateService(schedule, stoppingToken).ConfigureAwait(true);
                                break;
                            }
                            break;
                        }

                    case "Pep Rally Bell Schedule":
                        {
                            TimeSpan peprallyStart = new(7, 15, 00);
                            if (time >= peprallyStart && time <= new TimeSpan(07, 15, 00))
                            {
                                var schedule = context.PepRallyBellScheduleModels.ToList();
                                await PepRallyQRCodeUpdateService(schedule, stoppingToken).ConfigureAwait(true);
                                break;
                            }
                            break;
                        }

                    case "2 Hour Delay Bell Schedule":
                        {
                            TimeSpan _2hrdelStart = new(9, 15, 00);
                            if (time >= _2hrdelStart && time <= new TimeSpan(09, 15, 00))
                            {
                                var schedule = context.TwoHrDelayBellScheduleModels.ToList();
                                await TwoHrDelayQRCodeUpdateService(schedule, stoppingToken).ConfigureAwait(true);
                                break;
                            }
                            break;
                        }

                    case "Extended Aves Bell Schedule":
                        {
                            TimeSpan extAvesStart = new(7, 15, 00);
                            if (time >= extAvesStart && time <= new TimeSpan(07, 15, 00))
                            {
                                var schedule = context.ExtendedAvesModels.ToList();
                                await ExtendedQRCodeUpdateService(schedule, stoppingToken).ConfigureAwait(true);
                                break;
                            }
                            break;
                        }
                    case "Custom Bell Schedule":
                        {
                            TimeSpan customStart = context.CustomSchedules.OrderBy(a => a.StartTime).Where(a => a.BellName.Contains("Bell ")).Select(a => a.StartTime).First();
                            if (time >= customStart.Subtract(TimeSpan.FromMinutes(5.0)) && time <= customStart)
                            {
                                var schedule = context.CustomSchedules.ToList();
                                await CustomQRCodeUpdateService(schedule, stoppingToken).ConfigureAwait(true);
                                break;
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

        private async Task CustomQRCodeUpdateService(List<CustomScheduleModel> schedule, CancellationToken stoppingToken)
        {
            using var scope = scopeFactory.CreateAsyncScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var roomqrcodemodel = context.RoomQRCodeModels.ToList();
            List<TimeSpan> startTimes = [];
            foreach (var item in schedule)
            {
                startTimes.Add(item.StartTime);
            }

            var currenttime = DateTime.Now.TimeOfDay;

            for (int i = 0; i < startTimes.Count; i++)
            {
                if (currenttime <= startTimes[i].Subtract(TimeSpan.FromMinutes(2.0)) && currenttime >= startTimes[i].Add(TimeSpan.FromMinutes(2.0)))
                {
                    for (int j = 0; j < roomqrcodemodel.Count; j++)
                    {
                        var roomqrcode = new RoomQRCodeModel()
                        {
                            RoomId = roomqrcodemodel[j].RoomId,
                            Code = GenerateRandomCode()
                        };
                        context.Update(roomqrcode);

                        if (!context.SaveChangesAsync(stoppingToken).IsCompletedSuccessfully)
                        {
                            context.Entry(roomqrcode).State = Microsoft.EntityFrameworkCore.EntityState.Unchanged;
                            await context.SaveChangesAsync(stoppingToken).ConfigureAwait(true);
                        }
                    }
                    await Task.Delay(TimeSpan.FromMinutes(5.0), stoppingToken).ConfigureAwait(true);
                }
            }
        }

        private async Task ExtendedQRCodeUpdateService(List<ExtendedAvesBellScheduleModel> schedule, CancellationToken stoppingToken)
        {
            using var scope = scopeFactory.CreateAsyncScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var roomqrcodemodel = context.RoomQRCodeModels.ToList();
            List<TimeSpan> startTimes = [];
            foreach (var item in schedule)
            {
                startTimes.Add(item.StartTime);
            }

            var currenttime = DateTime.Now.TimeOfDay;

            for (int i = 0; i < startTimes.Count; i++)
            {
                if (currenttime <= startTimes[i].Subtract(TimeSpan.FromMinutes(2.0)) && currenttime >= startTimes[i].Add(TimeSpan.FromMinutes(2.0)))
                {
                    for (int j = 0; j < roomqrcodemodel.Count; j++)
                    {
                        var roomqrcode = new RoomQRCodeModel()
                        {
                            RoomId = roomqrcodemodel[j].RoomId,
                            Code = GenerateRandomCode()
                        };
                        context.Update(roomqrcode);

                        if (!context.SaveChangesAsync(stoppingToken).IsCompletedSuccessfully)
                        {
                            context.Entry(roomqrcode).State = Microsoft.EntityFrameworkCore.EntityState.Unchanged;
                            await context.SaveChangesAsync(stoppingToken).ConfigureAwait(true);
                        }
                    }
                    await Task.Delay(TimeSpan.FromMinutes(5.0), stoppingToken).ConfigureAwait(true);
                }
            }
        }

        private async Task TwoHrDelayQRCodeUpdateService(List<TwoHrDelayBellScheduleModel> schedule, CancellationToken stoppingToken)
        {
            using var scope = scopeFactory.CreateAsyncScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var roomqrcodemodel = context.RoomQRCodeModels.ToList();
            List<TimeSpan> startTimes = [];
            foreach (var item in schedule)
            {
                startTimes.Add(item.StartTime);
            }

            var currenttime = DateTime.Now.TimeOfDay;

            for (int i = 0; i < startTimes.Count; i++)
            {
                if (currenttime <= startTimes[i].Subtract(TimeSpan.FromMinutes(2.0)) && currenttime >= startTimes[i].Add(TimeSpan.FromMinutes(2.0)))
                {
                    for (int j = 0; j < roomqrcodemodel.Count; j++)
                    {
                        var roomqrcode = new RoomQRCodeModel()
                        {
                            RoomId = roomqrcodemodel[j].RoomId,
                            Code = GenerateRandomCode()
                        };
                        context.Update(roomqrcode);

                        if (!context.SaveChangesAsync(stoppingToken).IsCompletedSuccessfully)
                        {
                            context.Entry(roomqrcode).State = Microsoft.EntityFrameworkCore.EntityState.Unchanged;
                            await context.SaveChangesAsync(stoppingToken).ConfigureAwait(true);
                        }
                    }
                    await Task.Delay(TimeSpan.FromMinutes(5.0), stoppingToken).ConfigureAwait(true);
                }
            }
        }

        private async Task PepRallyQRCodeUpdateService(List<PepRallyBellScheduleModel> schedule, CancellationToken stoppingToken)
        {
            using var scope = scopeFactory.CreateAsyncScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var roomqrcodemodel = context.RoomQRCodeModels.ToList();
            List<TimeSpan> startTimes = [];
            foreach (var item in schedule)
            {
                startTimes.Add(item.StartTime);
            }

            var currenttime = DateTime.Now.TimeOfDay;

            for (int i = 0; i < startTimes.Count; i++)
            {
                if (currenttime <= startTimes[i].Subtract(TimeSpan.FromMinutes(2.0)) && currenttime >= startTimes[i].Add(TimeSpan.FromMinutes(2.0)))
                {
                    for (int j = 0; j < roomqrcodemodel.Count; j++)
                    {
                        var roomqrcode = new RoomQRCodeModel()
                        {
                            RoomId = roomqrcodemodel[j].RoomId,
                            Code = GenerateRandomCode()
                        };
                        context.Update(roomqrcode);

                        if (!context.SaveChangesAsync(stoppingToken).IsCompletedSuccessfully)
                        {
                            context.Entry(roomqrcode).State = Microsoft.EntityFrameworkCore.EntityState.Unchanged;
                            await context.SaveChangesAsync(stoppingToken).ConfigureAwait(true);
                        }
                    }
                    await Task.Delay(TimeSpan.FromMinutes(5.0), stoppingToken).ConfigureAwait(true);
                }
            }
        }

        private async Task DailyQRCodeUpdateService(List<DailyBellScheduleModel> schedule, CancellationToken stoppingToken)
        {
            using var scope = scopeFactory.CreateAsyncScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var roomqrcodemodel = context.RoomQRCodeModels.ToList();
            List<TimeSpan> startTimes = [];
            foreach (var item in schedule)
            {
                startTimes.Add(item.StartTime);
            }

            var currenttime = DateTime.Now.TimeOfDay;

            for (int i = 0; i < startTimes.Count; i++)
            {
                if (currenttime <= startTimes[i].Subtract(TimeSpan.FromMinutes(2.0)) && currenttime >= startTimes[i].Add(TimeSpan.FromMinutes(2.0)))
                {
                    for (int j = 0; j < roomqrcodemodel.Count; j++)
                    {
                        var roomqrcode = new RoomQRCodeModel()
                        {
                            RoomId = roomqrcodemodel[j].RoomId,
                            Code = GenerateRandomCode()
                        };
                        context.Update(roomqrcode);

                        if (!context.SaveChangesAsync(stoppingToken).IsCompletedSuccessfully)
                        {
                            context.Entry(roomqrcode).State = Microsoft.EntityFrameworkCore.EntityState.Unchanged;
                            await context.SaveChangesAsync(stoppingToken).ConfigureAwait(true);
                        }
                    }
                    await Task.Delay(TimeSpan.FromMinutes(5.0), stoppingToken).ConfigureAwait(true);
                }
            }
        }

        private static string GenerateRandomCode()
        {
            const string Characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()_+-=~`[]{}|;:',.<>?/";

            var bytes = new byte[64];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(bytes);
            }

            // Use StringBuilder for efficient string concatenation
            var result = new StringBuilder(64);
            foreach (var b in bytes)
            {
                // Ensure we use a more even distribution for the characters
                int index = b & 0x3F; // Masking to get a value between 0 and 63 (0x3F)
                result.Append(Characters[index % Characters.Length]);
            }

            return result.ToString();
        }
    }
}
#pragma warning restore CA1848