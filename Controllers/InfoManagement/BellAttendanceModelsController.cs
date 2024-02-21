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
    public class BellAttendanceModelsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BellAttendanceModelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BellAttendanceModels
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.bellAttendanceModels.Include(b => b.ActiveCourses).Include(b => b.StudentInfo);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: BellAttendanceModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bellAttendanceModel = await _context.bellAttendanceModels
                .Include(b => b.ActiveCourses)
                .Include(b => b.StudentInfo)
                .FirstOrDefaultAsync(m => m.StudentId == id);
            if (bellAttendanceModel == null)
            {
                return NotFound();
            }

            return View(bellAttendanceModel);
        }

        // GET: BellAttendanceModels/Create
        public IActionResult Create()
        {
            ViewData["CourseId"] = new SelectList(_context.activeCourseInfoModels, "CourseId", "CourseId");
            ViewData["StudentId"] = new SelectList(_context.studentInfoModels, "StudentID", "StudentID");
            return View();
        }

        // POST: BellAttendanceModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BellAttendanceId,StudentId,DateTime,Status,ReasonForAbsence,BellNumId,CourseId,ChosenBellSchedule")] BellAttendanceModel bellAttendanceModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bellAttendanceModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseId"] = new SelectList(_context.activeCourseInfoModels, "CourseId", "CourseId", bellAttendanceModel.CourseId);
            ViewData["StudentId"] = new SelectList(_context.studentInfoModels, "StudentID", "StudentID", bellAttendanceModel.StudentId);
            return View(bellAttendanceModel);
        }

        // GET: BellAttendanceModels/Edit/5
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
            ViewData["CourseId"] = new SelectList(_context.activeCourseInfoModels, "CourseId", "CourseId", bellAttendanceModel.CourseId);
            ViewData["StudentId"] = new SelectList(_context.studentInfoModels, "StudentID", "StudentID", bellAttendanceModel.StudentId);
            return View(bellAttendanceModel);
        }

        // POST: BellAttendanceModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BellAttendanceId,StudentId,DateTime,Status,ReasonForAbsence,BellNumId,CourseId,ChosenBellSchedule")] BellAttendanceModel bellAttendanceModel)
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
            ViewData["CourseId"] = new SelectList(_context.activeCourseInfoModels, "CourseId", "CourseId", bellAttendanceModel.CourseId);
            ViewData["StudentId"] = new SelectList(_context.studentInfoModels, "StudentID", "StudentID", bellAttendanceModel.StudentId);
            return View(bellAttendanceModel);
        }

        // GET: BellAttendanceModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bellAttendanceModel = await _context.bellAttendanceModels
                .Include(b => b.ActiveCourses)
                .Include(b => b.StudentInfo)
                .FirstOrDefaultAsync(m => m.StudentId == id);
            if (bellAttendanceModel == null)
            {
                return NotFound();
            }

            return View(bellAttendanceModel);
        }

        // POST: BellAttendanceModels/Delete/5
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
