using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SAMS.Data;
using SAMS.Models;

namespace SAMS.Controllers
{
    public class TeachingScheduleController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TeachingScheduleController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TeachingSchedule
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.TeachingScheduleModel.Include(t => t.Teacher);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: TeachingSchedule/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TeachingScheduleModel == null)
            {
                return NotFound();
            }

            var teachingScheduleModel = await _context.TeachingScheduleModel
                .Include(t => t.Teacher)
                .FirstOrDefaultAsync(m => m.ScheduleID == id);
            if (teachingScheduleModel == null)
            {
                return NotFound();
            }

            return View(teachingScheduleModel);
        }

        // GET: TeachingSchedule/Create
        public IActionResult Create()
        {
            ViewData["TeacherID"] = new SelectList(_context.teacherInfoModels, "TeacherID", "TeacherID");
            return View();
        }

        // POST: TeachingSchedule/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ScheduleID,TeacherID,DaysOfWeek")] TeachingScheduleModel teachingScheduleModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(teachingScheduleModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TeacherID"] = new SelectList(_context.teacherInfoModels, "TeacherID", "TeacherID", teachingScheduleModel.TeacherID);
            return View(teachingScheduleModel);
        }

        // GET: TeachingSchedule/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TeachingScheduleModel == null)
            {
                return NotFound();
            }

            var teachingScheduleModel = await _context.TeachingScheduleModel.FindAsync(id);
            if (teachingScheduleModel == null)
            {
                return NotFound();
            }
            ViewData["TeacherID"] = new SelectList(_context.teacherInfoModels, "TeacherID", "TeacherID", teachingScheduleModel.TeacherID);
            return View(teachingScheduleModel);
        }

        // POST: TeachingSchedule/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ScheduleID,TeacherID,DaysOfWeek")] TeachingScheduleModel teachingScheduleModel)
        {
            if (id != teachingScheduleModel.ScheduleID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(teachingScheduleModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeachingScheduleModelExists(teachingScheduleModel.ScheduleID))
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
            ViewData["TeacherID"] = new SelectList(_context.teacherInfoModels, "TeacherID", "TeacherID", teachingScheduleModel.TeacherID);
            return View(teachingScheduleModel);
        }

        // GET: TeachingSchedule/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TeachingScheduleModel == null)
            {
                return NotFound();
            }

            var teachingScheduleModel = await _context.TeachingScheduleModel
                .Include(t => t.Teacher)
                .FirstOrDefaultAsync(m => m.ScheduleID == id);
            if (teachingScheduleModel == null)
            {
                return NotFound();
            }

            return View(teachingScheduleModel);
        }

        // POST: TeachingSchedule/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TeachingScheduleModel == null)
            {
                return Problem("Entity set 'ApplicationDbContext.TeachingScheduleModel'  is null.");
            }
            var teachingScheduleModel = await _context.TeachingScheduleModel.FindAsync(id);
            if (teachingScheduleModel != null)
            {
                _context.TeachingScheduleModel.Remove(teachingScheduleModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeachingScheduleModelExists(int id)
        {
          return (_context.TeachingScheduleModel?.Any(e => e.ScheduleID == id)).GetValueOrDefault();
        }
    }
}
