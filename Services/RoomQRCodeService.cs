
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.Elfie.Extensions;
using Microsoft.EntityFrameworkCore;
using NuGet.Common;
using SAMS.Controllers;
using SAMS.Data;
using SAMS.Models;

namespace SAMS.Services
{
    public class RoomQRCodeService : BackgroundService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationUser> _roleManager;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<RoomQRCodeService> _logger;

        public RoomQRCodeService(UserManager<ApplicationUser> userManager, RoleManager<ApplicationUser> roleManager, ApplicationDbContext dbContext, ILogger<RoomQRCodeService> logger)
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
                                ChangeQRCode();
                                break;
                            }
                            break;
                        }

                    case "Pep Rally Bell Schedule":
                        {
                            if (time >= peprallyStart && time <= new TimeSpan(07, 20, 00))
                            {
                                ChangeQRCode();
                                break;
                            }
                            break;
                        }

                    case "2 Hour Delay Bell Schedule":
                        {
                            if (time >= _2hrdelStart && time <= new TimeSpan(09, 20, 00))
                            {
                                ChangeQRCode();
                                break;
                            }
                            break;
                        }

                    case "Extended Aves Bell Schedule":
                        {
                            if (time >= extAvesStart && time <= new TimeSpan(07, 20, 00))
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
            var roomIds = _context.roomLocationInfoModels.Select(a => a.RoomId).ToList();

            foreach (var id in roomIds)
            {
                var entry = _context.roomQRCodeModels.Any(a => a.RoomId == id);
                var roomNames = _context.roomLocationInfoModels.Find(roomIds);
                if (entry)
                {
                    var roomCodes = _context.roomQRCodeModels.First(a => a.RoomId == id);
                    var oldCode = roomCodes.Code;
                    var newCode = new Guid().ToString("n");
                    var timeStamp = new TimestampModel
                    {
                        Timestamp = DateTime.Now,
                        ActionMade = "Updated Room Code for QR Codes",
                        MadeBy = $"Automated QR Code Service (Update) - SAMS Program {DateTime.Now}",
                        Comments = $"The Code for room: {roomNames?.RoomNumberMod} from Old Code of {oldCode} " +
                        $"to New Code {newCode} at {DateTime.Now} Please contact the Sycamore High School Attendance Office for any further questions or concerns."
                    };
                    roomCodes.Code = newCode;

                    _context.roomQRCodeModels.Update(roomCodes);
                    _context.timestampModels.Add(timeStamp);
                }
                else
                {
                    var newEntry = new RoomQRCodeModel
                    {
                        RoomId = id,
                        Code = new Guid().ToString("n")
                    };
                    _context.roomQRCodeModels.Add(newEntry);
                    var timeStamp = new TimestampModel
                    {
                        Timestamp = DateTime.Now,
                        ActionMade = "Added New Room Code in RoomQRCode",
                        MadeBy = $"Automated QR Code Service (Add) - SAMS Program {DateTime.Now}",
                        Comments = $"An entry was added to RoomQRCode for room number {roomNames?.RoomNumberMod} with a new code" +
                        $"{newEntry.Code} at {DateTime.Now}. Please contact Sycamore HS Attendance Office for any further questions or concerns."
                    };
                    _context.timestampModels.Add(timeStamp);
                }
            }
            await _context.SaveChangesAsync();
        }
    }
}
