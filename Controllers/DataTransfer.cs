using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using SAMS.Data;
using SAMS.Models;
using SQLitePCL;

namespace SAMS.Controllers
{
    public class DataTransfer(ILogger<DataTransfer> logger, ApplicationDbContext context) : Controller
    {
        private readonly ILogger<DataTransfer> _logger = logger;
        private readonly ApplicationDbContext _context = context;

        public async Task<IActionResult> Index()
        {
            try
            {
                //CODE AND/OR SCRIPT TO RESEED THE NUMBERING
                var rawSqlString = "DBCC CHECKIDENT ('activeCourseInfoModels', RESEED, 0);";
                _context.Database.ExecuteSqlRaw(rawSqlString);

                using var package = new ExcelPackage(new FileInfo("C:\\Users\\Shiva\\OneDrive\\SAMS\\ActiveCourseInfo-Cloud.xlsx"));

                var worksheet = package.Workbook.Worksheets[0];

                for (int row = 2; row <= worksheet.Dimension.Rows; row++)
                {
                    //var name = worksheet.Cells[1, 1].Value.ToString();
                    //_logger.LogInformation($"This is column name: {name}");

                    var worksheetBell = worksheet.Cells[row, 7].Value.ToString();
                    if (worksheetBell == "?")
                    {
                        var b2bboolcheck1 = worksheet.Cells[row, 10].Value.ToString() == ("TRUE");
                        var b2bboolcheck2 = worksheet.Cells[row, 10].Value.ToString() == ("True");
                        var dailyboolcheck1 = worksheet.Cells[row, 9].Value.ToString() == ("TRUE");
                        var dailyboolcheck2 = worksheet.Cells[row, 9].Value.ToString() == ("True");

                        bool checkedb2b;
                        bool checkeddaily;

                        if (b2bboolcheck1 || b2bboolcheck2)
                        {
                            checkedb2b = true;
                        }
                        else
                        {
                            checkedb2b = false;
                        }

                        if (dailyboolcheck1 || dailyboolcheck2)
                        {
                            checkeddaily = true;
                        }
                        else
                        {
                            checkeddaily = false;
                        }

                        var course = new ActiveCourseInfoModel
                        {
                            CourseName = worksheet.Cells[row, 2].Value.ToString()!,
                            CourseCode = worksheet.Cells[row, 1].Value.ToString()!,
                            CourseLevel = worksheet.Cells[row, 3].Value.ToString()!,
                            CourseBellNumber = "-1",
                            CourseLength = worksheet.Cells[row, 5].Value.ToString()!,
                            B2BAttChecked = checkedb2b,
                            DailyAttChecked = checkeddaily,
                            CourseRoomID = 5,
                            CourseTeacherID = "pletzj"
                        };
                        _context.ActiveCourseInfoModels.Add(course);
                        //await _context.SaveChangesAsync();
                    }
                    else
                    {
                        var b2bboolcheck1 = worksheet.Cells[row, 10].Value.ToString() == ("TRUE");
                        var b2bboolcheck2 = worksheet.Cells[row, 10].Value.ToString() == ("True");
                        var dailyboolcheck1 = worksheet.Cells[row, 9].Value.ToString() == ("TRUE");
                        var dailyboolcheck2 = worksheet.Cells[row, 9].Value.ToString() == ("True");

                        bool checkedb2b;
                        bool checkeddaily;

                        if (b2bboolcheck1 || b2bboolcheck2)
                        {
                            checkedb2b = true;
                        }
                        else
                        {
                            checkedb2b = false;
                        }

                        if (dailyboolcheck1 || dailyboolcheck2)
                        {
                            checkeddaily = true;
                        }
                        else
                        {
                            checkeddaily = false;
                        }

                        var course = new ActiveCourseInfoModel
                        {
                            CourseName = worksheet.Cells[row, 2].Value.ToString()!,
                            CourseCode = worksheet.Cells[row, 1].Value.ToString()!,
                            CourseLevel = worksheet.Cells[row, 3].Value.ToString()!,
                            CourseBellNumber = worksheet.Cells[row, 7].Value.ToString()!,
                            CourseLength = worksheet.Cells[row, 5].Value.ToString()!,
                            B2BAttChecked = checkedb2b,
                            DailyAttChecked = checkeddaily,
                            CourseRoomID = 5,
                            CourseTeacherID = "pletzj"
                        };
                        _context.ActiveCourseInfoModels.Add(course);
                        //await _context.SaveChangesAsync();
                    }
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogCritical("Message: \n {Message}", ex.Message);
                _logger.LogDebug("Stack Trace \n {StackTrace}", ex.StackTrace);
                _logger.LogWarning("Message: \n {Message}", ex.Message);
            }

            return View();
        }

        public async Task<IActionResult> TeacherTransfer()
        {
            try
            {
                //CODE AND/OR SCRIPT TO RESEED THE NUMBERING
                //var rawSqlString = "DBCC CHECKIDENT ('activeCourseInfoModels', RESEED, 0);";
                //_context.Database.ExecuteSqlRaw(rawSqlString);

                using var package = new ExcelPackage(new FileInfo("C:\\Users\\Shiva\\OneDrive\\SAMS\\ActiveCourseInfo-Cloud.xlsx"));

                var worksheet = package.Workbook.Worksheets[1];

                for (int row = 2; row <= worksheet.Dimension.Rows; row++)
                {
                    //var name = worksheet.Cells[1, 1].Value.ToString();
                    //_logger.LogInformation($"This is column name: {name}");

                    var teacher = new TeacherInfoModel
                    {
                        TeacherID = worksheet.Cells[row, 1].Value.ToString()!,
                        TeacherFirstNameMod = worksheet.Cells[row, 2].Value.ToString()!,
                        TeacherLastNameMod = worksheet.Cells[row, 3].Value.ToString()!,
                        TeacherEmailMod = worksheet.Cells[row, 4].Value.ToString()!,
                    };
                    _context.TeacherInfoModels.Add(teacher);
                    //await _context.SaveChangesAsync();
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogCritical("Message: \n {Message}", ex.Message);
                _logger.LogDebug("Stack Trace \n {StackTrace}", ex.StackTrace);
                _logger.LogWarning("Message: \n {Message}", ex.Message);
            }

            return View("Index");
        }
    }
}
