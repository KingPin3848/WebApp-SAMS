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
    public class BellAttendanceController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BellAttendanceController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BellAttendance
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.bellAttendanceModels.Include(b => b.StudentInfo).Include(b => b.StudentScheduleInfoModel);
            return View("~/Views/InfoManagement/BellAttendance/Index.cshtml", await applicationDbContext.ToListAsync());
        }

        // GET: BellAttendance/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bellAttendanceModel = await _context.bellAttendanceModels
                .Include(b => b.StudentInfo)
                .Include(b => b.StudentScheduleInfoModel)
                .FirstOrDefaultAsync(m => m.StudentId == id);
            if (bellAttendanceModel == null)
            {
                return NotFound();
            }

            return View("~/Views/InfoManagement/BellAttendance/Details.cshtml", bellAttendanceModel);
        }

        // GET: BellAttendance/Create
        public IActionResult Create()
        {
            ViewData["StudentId"] = new SelectList(_context.studentInfoModels, "StudentID", "StudentID");
            ViewData["ScheduleId"] = new SelectList(_context.studentScheduleInfoModels, "StudentID", "StudentID");
            return View("~/Views/InfoManagement/BellAttendance/Create.cshtml");
        }

        // POST: BellAttendance/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BellAttendanceId,StudentId,DateTime,Status,ReasonForAbsence,BellNumId,ScheduleId")] BellAttendanceModel bellAttendanceModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bellAttendanceModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["StudentId"] = new SelectList(_context.studentInfoModels, "StudentID", "StudentID", bellAttendanceModel.StudentId);
            ViewData["ScheduleId"] = new SelectList(_context.studentScheduleInfoModels, "StudentID", "StudentID", bellAttendanceModel.ScheduleId);
            return View("~/Views/InfoManagement/BellAttendance/Create.cshtml", bellAttendanceModel);
        }

        // GET: BellAttendance/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bellAttendanceModel = await _context.bellAttendanceModels.FindAsync(id);
            if (bellAttendanceModel == null)
            {
                return NotFound();
            }
            ViewData["StudentId"] = new SelectList(_context.studentInfoModels, "StudentID", "StudentID", bellAttendanceModel.StudentId);
            ViewData["ScheduleId"] = new SelectList(_context.studentScheduleInfoModels, "StudentID", "StudentID", bellAttendanceModel.ScheduleId);
            return View("~/Views/InfoManagement/BellAttendance/Edit.cshtml", bellAttendanceModel);
        }

        // POST: BellAttendance/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BellAttendanceId,StudentId,DateTime,Status,ReasonForAbsence,BellNumId,ScheduleId")] BellAttendanceModel bellAttendanceModel)
        {
            if (id != bellAttendanceModel.StudentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bellAttendanceModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BellAttendanceModelExists(bellAttendanceModel.StudentId))
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
            ViewData["StudentId"] = new SelectList(_context.studentInfoModels, "StudentID", "StudentID", bellAttendanceModel.StudentId);
            ViewData["ScheduleId"] = new SelectList(_context.studentScheduleInfoModels, "StudentID", "StudentID", bellAttendanceModel.ScheduleId);
            return View("~/Views/InfoManagement/BellAttendance/Edit.cshtml", bellAttendanceModel);
        }

        // GET: BellAttendance/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bellAttendanceModel = await _context.bellAttendanceModels
                .Include(b => b.StudentInfo)
                .Include(b => b.StudentScheduleInfoModel)
                .FirstOrDefaultAsync(m => m.StudentId == id);
            if (bellAttendanceModel == null)
            {
                return NotFound();
            }

            return View("~/Views/InfoManagement/BellAttendance/Delete.cshtml", bellAttendanceModel);
        }

        // POST: BellAttendance/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bellAttendanceModel = await _context.bellAttendanceModels.FindAsync(id);
            if (bellAttendanceModel != null)
            {
                _context.bellAttendanceModels.Remove(bellAttendanceModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BellAttendanceModelExists(int id)
        {
            return _context.bellAttendanceModels.Any(e => e.StudentId == id);
        }
    }
}
