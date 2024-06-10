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
    public class DailyAttendanceController(ApplicationDbContext context) : Controller
    {
        private readonly ApplicationDbContext _context = context;

        // GET: DailyAttendance
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.DailyAttendanceModels.Include(d => d.Student);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: DailyAttendance/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dailyAttendanceModel = await _context.DailyAttendanceModels
                .Include(d => d.Student)
                .FirstOrDefaultAsync(m => m.AttendanceId == id);
            if (dailyAttendanceModel == null)
            {
                return NotFound();
            }

            return View(dailyAttendanceModel);
        }

        // GET: DailyAttendance/Create
        public IActionResult Create()
        {
            ViewData["StudentId"] = new SelectList(_context.StudentInfoModels, "StudentID", "StudentID");
            return View();
        }

        // POST: DailyAttendance/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AttendanceId,StudentId,AttendanceDate,Status,ReasonForAbsence,ChosenBellSchedule")] DailyAttendanceModel dailyAttendanceModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dailyAttendanceModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["StudentId"] = new SelectList(_context.StudentInfoModels, "StudentID", "StudentID", dailyAttendanceModel.StudentId);
            return View(dailyAttendanceModel);
        }

        // GET: DailyAttendance/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dailyAttendanceModel = await _context.DailyAttendanceModels.FindAsync(id);
            if (dailyAttendanceModel == null)
            {
                return NotFound();
            }
            ViewData["StudentId"] = new SelectList(_context.StudentInfoModels, "StudentID", "StudentID", dailyAttendanceModel.StudentId);
            return View(dailyAttendanceModel);
        }

        // POST: DailyAttendance/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AttendanceId,StudentId,AttendanceDate,Status,ReasonForAbsence,ChosenBellSchedule")] DailyAttendanceModel dailyAttendanceModel)
        {
            if (id != dailyAttendanceModel.AttendanceId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dailyAttendanceModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DailyAttendanceModelExists(dailyAttendanceModel.AttendanceId))
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
            ViewData["StudentId"] = new SelectList(_context.StudentInfoModels, "StudentID", "StudentID", dailyAttendanceModel.StudentId);
            return View(dailyAttendanceModel);
        }

        // GET: DailyAttendance/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dailyAttendanceModel = await _context.DailyAttendanceModels
                .Include(d => d.Student)
                .FirstOrDefaultAsync(m => m.AttendanceId == id);
            if (dailyAttendanceModel == null)
            {
                return NotFound();
            }

            return View(dailyAttendanceModel);
        }

        // POST: DailyAttendance/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dailyAttendanceModel = await _context.DailyAttendanceModels.FindAsync(id);
            if (dailyAttendanceModel != null)
            {
                _context.DailyAttendanceModels.Remove(dailyAttendanceModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DailyAttendanceModelExists(int id)
        {
            return _context.DailyAttendanceModels.Any(e => e.AttendanceId == id);
        }
    }
}
