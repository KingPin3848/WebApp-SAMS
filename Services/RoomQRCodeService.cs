using SAMS.Data;
using SAMS.Models;

#pragma warning disable CA1848
namespace SAMS.Services
{
    public class RoomQRCodeService(ILogger<RoomQRCodeService> logger, IServiceScopeFactory scopeFactory) : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory = scopeFactory;
        private readonly ILogger<RoomQRCodeService> _logger = logger;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await HolidayRun(stoppingToken).ConfigureAwait(true);
            }
        }

        private async Task HolidayRun(CancellationToken stoppingToken)
        {
            using var scope = _scopeFactory.CreateAsyncScope();

            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var holidayDates = context.SchedulerModels.Where(a => a.Type == "No School @SHS").Select(a => a.Date).ToList();
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
            using var scope = _scopeFactory.CreateAsyncScope();
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
                            var bells = context.DailyBellScheduleModels.Select(a => a.BellName).ToList();
                            var startTimes = context.DailyBellScheduleModels.Select(a => a.StartTime).ToList();
                            var endTimes = context.DailyBellScheduleModels.Select(a => a.EndTime).ToList();

                            for (int i = 0; i < bells.Count; i++)
                            {
                                if (bells[i] == "Bell 0" || bells[i].Contains("Transition", StringComparison.Ordinal))
                                {
                                    bells.Remove(bells[i]);
                                    startTimes.Remove(startTimes[i]);
                                    endTimes.Remove(endTimes[i]);
                                }
                            }

                            if (bells.Count == startTimes.Count && bells.Count == endTimes.Count && startTimes.Count == endTimes.Count)
                            {
                                for (int i = 0; i < bells.Count; i++)
                                {
                                    if (time >= startTimes[i] && time <= startTimes[i].Add(TimeSpan.FromMinutes(5)))
                                    {
                                        ChangeQRCode(stoppingToken);
                                        await Task.Delay(TimeSpan.FromMinutes(15), stoppingToken).ConfigureAwait(false);
                                    }
                                }
                            }
                            break;
                        }

                    case "Pep Rally Bell Schedule":
                        {
                            var bells = context.PepRallyBellScheduleModels.Select(a => a.BellName).ToList();
                            var startTimes = context.PepRallyBellScheduleModels.Select(a => a.StartTime).ToList();
                            var endTimes = context.PepRallyBellScheduleModels.Select(a => a.EndTime).ToList();

                            for (int i = 0; i < bells.Count; i++)
                            {
                                if (bells[i] == "Bell 0" || bells[i].Contains("Transition", StringComparison.Ordinal))
                                {
                                    bells.Remove(bells[i]);
                                    startTimes.Remove(startTimes[i]);
                                    endTimes.Remove(endTimes[i]);
                                }
                            }

                            if (bells.Count == startTimes.Count && bells.Count == endTimes.Count && startTimes.Count == endTimes.Count)
                            {
                                for (int i = 0; i < bells.Count; i++)
                                {
                                    if (time >= startTimes[i] && time <= startTimes[i].Add(TimeSpan.FromMinutes(5)))
                                    {
                                        ChangeQRCode(stoppingToken);
                                        await Task.Delay(TimeSpan.FromMinutes(15), stoppingToken).ConfigureAwait(false);
                                    }
                                }
                            }
                            break;
                        }

                    case "2 Hour Delay Bell Schedule":
                        {
                            var bells = context.TwoHrDelayBellScheduleModels.Select(a => a.BellName).ToList();
                            var startTimes = context.TwoHrDelayBellScheduleModels.Select(a => a.StartTime).ToList();
                            var endTimes = context.TwoHrDelayBellScheduleModels.Select(a => a.EndTime).ToList();

                            for (int i = 0; i < bells.Count; i++)
                            {
                                if (bells[i] == "Bell 0" || bells[i].Contains("Transition", StringComparison.Ordinal))
                                {
                                    bells.Remove(bells[i]);
                                    startTimes.Remove(startTimes[i]);
                                    endTimes.Remove(endTimes[i]);
                                }
                            }

                            if (bells.Count == startTimes.Count && bells.Count == endTimes.Count && startTimes.Count == endTimes.Count)
                            {
                                for (int i = 0; i < bells.Count; i++)
                                {
                                    if (time >= startTimes[i] && time <= startTimes[i].Add(TimeSpan.FromMinutes(5)))
                                    {
                                        ChangeQRCode(stoppingToken);
                                        await Task.Delay(TimeSpan.FromMinutes(15), stoppingToken).ConfigureAwait(false);
                                    }
                                }
                            }
                            break;
                        }

                    case "Extended Aves Bell Schedule":
                        {
                            var bells = context.ExtendedAvesModels.Select(a => a.BellName).ToList();
                            var startTimes = context.ExtendedAvesModels.Select(a => a.StartTime).ToList();
                            var endTimes = context.ExtendedAvesModels.Select(a => a.EndTime).ToList();

                            for (int i = 0; i < bells.Count; i++)
                            {
                                if (bells[i] == "Bell 0" || bells[i].Contains("Transition", StringComparison.Ordinal))
                                {
                                    bells.Remove(bells[i]);
                                    startTimes.Remove(startTimes[i]);
                                    endTimes.Remove(endTimes[i]);
                                }
                            }

                            if (bells.Count == startTimes.Count && bells.Count == endTimes.Count && startTimes.Count == endTimes.Count)
                            {
                                for (int i = 0; i < bells.Count; i++)
                                {
                                    if (time >= startTimes[i] && time <= startTimes[i].Add(TimeSpan.FromMinutes(5)))
                                    {
                                        ChangeQRCode(stoppingToken);
                                        await Task.Delay(TimeSpan.FromMinutes(15), stoppingToken).ConfigureAwait(false);
                                    }
                                }
                            }
                            break;
                        }

                    case "Custom Bell Schedule":
                        {
                            var bells = context.CustomSchedules.Select(a => a.BellName).ToList();
                            var startTimes = context.CustomSchedules.Select(a => a.StartTime).ToList();
                            var endTimes = context.CustomSchedules.Select(a => a.EndTime).ToList();

                            for (int i = 0; i < bells.Count; i++)
                            {
                                if (bells[i] == "Bell 0" || bells[i].Contains("Transition", StringComparison.Ordinal))
                                {
                                    bells.Remove(bells[i]);
                                    startTimes.Remove(startTimes[i]);
                                    endTimes.Remove(endTimes[i]);
                                }
                            }

                            if (bells.Count == startTimes.Count && bells.Count == endTimes.Count && startTimes.Count == endTimes.Count)
                            {
                                for (int i = 0; i < bells.Count; i++)
                                {
                                    if (time >= startTimes[i] && time <= startTimes[i].Add(TimeSpan.FromMinutes(5)))
                                    {
                                        ChangeQRCode(stoppingToken);
                                        await Task.Delay(TimeSpan.FromMinutes(15), stoppingToken).ConfigureAwait(false);
                                    }
                                }
                            }
                            break;
                        }

                    default:
                        {
                            await Task.Delay(TimeSpan.FromDays(1), stoppingToken).ConfigureAwait(true);
                            break;
                        }
                }
            }
            else
            {
                await Task.Delay(TimeSpan.FromDays(1), stoppingToken).ConfigureAwait(true);
            }
        }

        private async void ChangeQRCode(CancellationToken stoppingToken)
        {
            using var scope = _scopeFactory.CreateAsyncScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var roomIds = context.RoomLocationInfoModels.Select(a => a.RoomNumberMod).ToList();

            foreach (var id in roomIds)
            {
                var entry = context.RoomQRCodeModels.Any(a => a.RoomId == id);
                var roomName = context.RoomLocationInfoModels.Find(id);
                if (entry)
                {
                    var roomCodes = context.RoomQRCodeModels.First(a => a.RoomId == id);
                    var oldCode = roomCodes.Code;
                    var newCode = new Guid().ToString("n");
                    var timeStamp = new TimestampModel
                    {
                        Timestamp = DateTime.Now,
                        ActionMade = "Updated Room Code for QR Codes",
                        MadeBy = $"Automated QR Code Service (Update) - SAMS Program {DateTime.Now}",
                        Comments = $"The Code for room: {roomName?.RoomNumberMod} from Old Code of {oldCode} " +
                        $"to New Code {newCode} at {DateTime.Now} Please contact the Sycamore High School Attendance Office for any further questions or concerns."
                    };
                    roomCodes.Code = newCode;

                    context.RoomQRCodeModels.Update(roomCodes);
                    context.TimestampModels.Add(timeStamp);
                }
                else
                {
                    var newEntry = new RoomQRCodeModel
                    {
                        RoomId = id,
                        Code = new Guid().ToString("n")
                    };
                    context.RoomQRCodeModels.Add(newEntry);
                    var timeStamp = new TimestampModel
                    {
                        Timestamp = DateTime.Now,
                        ActionMade = "Added New Room Code in RoomQRCode",
                        MadeBy = $"Automated QR Code Service (Add) - SAMS Program {DateTime.Now}",
                        Comments = $"An entry was added to RoomQRCode for room number {roomName!.RoomNumberMod} with a new code" +
                        $"{newEntry.Code} at {DateTime.Now}. Please contact Sycamore HS Attendance Office for any further questions or concerns."
                    };
                    context.TimestampModels.Add(timeStamp);
                }
            }
            await context.SaveChangesAsync(stoppingToken).ConfigureAwait(true);
        }
    }
}
#pragma warning restore CA1848