using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SAMS.Data;
using SAMS.Models;

namespace SAMS.Controllers.InfoManagement
{
    public class ReportController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReportController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Report
        public async Task<IActionResult> Index()
        {
            return View(await _context.ErrorProcessingModel.ToListAsync().ConfigureAwait(true));
        }

        // GET: Report/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reportModel = await _context.ErrorProcessingModel
                .FirstOrDefaultAsync(m => m.ReportId == id).ConfigureAwait(true);
            if (reportModel == null)
            {
                return NotFound();
            }

            return View(reportModel);
        }

        // GET: Report/Create
        [HttpGet]
        public IActionResult Create()
        {
            ViewData["Types"] = new SelectList(_context.ErrorProcessingModel, "TypeOfReport", "TypeOfReport");
            return View();
        }

        // POST: Report/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReportId,TypeOfReport,DeveloperReference,Description")] ReportModel reportModel)
        {
            if (ModelState.IsValid)
            {
                if (reportModel == null)
                {
                    return NotFound();
                }

                switch (reportModel.TypeOfReport)
                {
                    case ReportModel.ErrorType.AttendanceStatusError:
                        {
                            /* Attendance Status Error is the first (1st) option in the Types of Errors to report/submit */
                            reportModel.Severity = ReportModel.SeverityLevel.High;
                            break;
                        }
                    case ReportModel.ErrorType.AttendanceScanningError:
                        {
                            /* Attendance Scanning Error is the second (2nd) option in the Types of Errors to report/submit */
                            reportModel.Severity = ReportModel.SeverityLevel.Low;
                            break;
                        }
                    case ReportModel.ErrorType.HallPassError:
                        {
                            /* Hall Pass Error is the third (3rd) option in the Types of Errors to report/submit */
                            reportModel.Severity = ReportModel.SeverityLevel.Medium;
                            break;
                        }
                    case ReportModel.ErrorType.StudentLocationError:
                        {
                            /* Student Location Error is the fourth (4th) option in the Types of Errors to report/submit */
                            reportModel.Severity = ReportModel.SeverityLevel.High;
                            break;
                        }
                    case ReportModel.ErrorType.ProcessingError:
                        {
                            /* Processing Error is the fifth (5th) option in the Types of Errors to report/submit */
                            reportModel.Severity = ReportModel.SeverityLevel.Medium;
                            break;
                        }
                    case ReportModel.ErrorType.Bug:
                        {
                            /* Bug is the sixth (6th) option in the Types of Errors to report/submit */
                            reportModel.Severity = ReportModel.SeverityLevel.Low;
                            break;
                        }
                    case ReportModel.ErrorType.SystemFeedback:
                        {
                            /* System Feedback is the seventh (7th) option in the Types of Errors to report/submit */
                            reportModel.Severity = ReportModel.SeverityLevel.Low;
                            break;
                        }
                    default:
                        {
                            /* Options other than the above will be considered as a breach and will still be reported. */
                            reportModel.Severity = ReportModel.SeverityLevel.High;
                            break;
                        }
                }
                reportModel.StatusOfReport = ReportModel.Status.SubmittedToAppropriatePersonnel;
                _context.Add(reportModel);
                await _context.SaveChangesAsync().ConfigureAwait(true);
                return RedirectToAction(nameof(Index));
            }
            return View(reportModel);
        }

        // GET: Report/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reportModel = await _context.ErrorProcessingModel.FindAsync(id).ConfigureAwait(true);
            if (reportModel == null)
            {
                return NotFound();
            }
            return View(reportModel);
        }

        // POST: Report/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReportId,StatusOfReport")] ReportModel reportModel)
        {
            if (reportModel == null)
            {
                return NotFound();
            }

            switch (reportModel.TypeOfReport)
            {
                case ReportModel.ErrorType.AttendanceStatusError:
                    {
                        /* Attendance Status Error is the first (1st) option in the Types of Errors to report/submit */
                        reportModel.Severity = ReportModel.SeverityLevel.High;
                        break;
                    }
                case ReportModel.ErrorType.AttendanceScanningError:
                    {
                        /* Attendance Scanning Error is the second (2nd) option in the Types of Errors to report/submit */
                        reportModel.Severity = ReportModel.SeverityLevel.Low;
                        break;
                    }
                case ReportModel.ErrorType.HallPassError:
                    {
                        /* Hall Pass Error is the third (3rd) option in the Types of Errors to report/submit */
                        reportModel.Severity = ReportModel.SeverityLevel.Medium;
                        break;
                    }
                case ReportModel.ErrorType.StudentLocationError:
                    {
                        /* Student Location Error is the fourth (4th) option in the Types of Errors to report/submit */
                        reportModel.Severity = ReportModel.SeverityLevel.High;
                        break;
                    }
                case ReportModel.ErrorType.ProcessingError:
                    {
                        /* Processing Error is the fifth (5th) option in the Types of Errors to report/submit */
                        reportModel.Severity = ReportModel.SeverityLevel.Medium;
                        break;
                    }
                case ReportModel.ErrorType.Bug:
                    {
                        /* Bug is the sixth (6th) option in the Types of Errors to report/submit */
                        reportModel.Severity = ReportModel.SeverityLevel.Low;
                        break;
                    }
                case ReportModel.ErrorType.SystemFeedback:
                    {
                        /* System Feedback is the seventh (7th) option in the Types of Errors to report/submit */
                        reportModel.Severity = ReportModel.SeverityLevel.Low;
                        break;
                    }
                default:
                    {
                        /* Options other than the above will be considered as a breach and will still be reported. */
                        reportModel.Severity = ReportModel.SeverityLevel.High;
                        break;
                    }
            }

            if (id != reportModel.ReportId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reportModel);
                    await _context.SaveChangesAsync().ConfigureAwait(true);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReportModelExists(reportModel.ReportId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(reportModel);
        }

        /*// GET: Report/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reportModel = await _context.ErrorProcessingModel
                .FirstOrDefaultAsync(m => m.ReportId == id);
            if (reportModel == null)
            {
                return NotFound();
            }

            return View(reportModel);
        }

        // POST: Report/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reportModel = await _context.ErrorProcessingModel.FindAsync(id);
            if (reportModel != null)
            {
                _context.ErrorProcessingModel.Remove(reportModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        */

        private bool ReportModelExists(int id)
        {
            return _context.ErrorProcessingModel.Any(e => e.ReportId == id);
        }
    }
}
