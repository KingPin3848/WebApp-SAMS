using SAMS.Data;
using SAMS.Models;

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
                await HolidayRun();
            }
        }

        private async Task HolidayRun()
        {
            using var scope = _scopeFactory.CreateAsyncScope();

            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var holidayDates = context.SchedulerModels.Where(a => a.Type == "No School @SHS").Select(a => a.Date).ToList();
            var todayDate = DateOnly.FromDateTime(DateTime.Now.Date);

            if (holidayDates == null)
            {
                _logger.LogWarning("Holidays is null and the task if delayed by 1 DAY. Done by the if statement in holidayRun");
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
                                ChangeQRCode();
                                break;
                            }
                            break;
                        }

                    case "Pep Rally Bell Schedule":
                        {
                            TimeSpan peprallyStart = new(7, 15, 00);
                            if (time >= peprallyStart && time <= new TimeSpan(07, 20, 00))
                            {
                                ChangeQRCode();
                                break;
                            }
                            break;
                        }

                    case "2 Hour Delay Bell Schedule":
                        {
                            TimeSpan _2hrdelStart = new(9, 15, 00);
                            if (time >= _2hrdelStart && time <= new TimeSpan(09, 20, 00))
                            {
                                ChangeQRCode();
                                break;
                            }
                            break;
                        }

                    case "Extended Aves Bell Schedule":
                        {
                            TimeSpan extAvesStart = new(7, 15, 00);
                            if (time >= extAvesStart && time <= new TimeSpan(07, 20, 00))
                            {
                                ChangeQRCode();
                                break;
                            }
                            break;
                        }

                    case "Custom Bell Schedule":
                        {
                            TimeSpan customScheduleStartTime = context.CustomSchedules.OrderBy(a => a.StartTime).Where(a => a.BellName.Contains("Bell ")).First().StartTime;
                            if (time >= customScheduleStartTime.Subtract(TimeSpan.FromMinutes(5)) && time <= customScheduleStartTime)
                            {
                                ChangeQRCode();
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
                await Task.Delay(TimeSpan.FromMinutes(20));
            }
            else
            {
                await Task.Delay(TimeSpan.FromDays(1));
            }
        }

        private async void ChangeQRCode()
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
            await context.SaveChangesAsync();
        }
    }
}
