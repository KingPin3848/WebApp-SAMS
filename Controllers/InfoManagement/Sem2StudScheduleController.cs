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
    public class Sem2StudScheduleController(ApplicationDbContext context) : Controller
    {
        private readonly ApplicationDbContext _context = context;

        // GET: Sem2StudSchedule
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Sem2StudSchedules.Include(s => s.Student);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Sem2StudSchedule/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sem2StudSchedule = await _context.Sem2StudSchedules
                .Include(s => s.Student)
                .FirstOrDefaultAsync(m => m.StudentID == id);
            if (sem2StudSchedule == null)
            {
                return NotFound();
            }

            return View(sem2StudSchedule);
        }

        // GET: Sem2StudSchedule/Create
        public IActionResult Create()
        {
            ViewData["StudentID"] = new SelectList(_context.StudentInfoModels, "StudentID", "StudentID");
            return View();
        }

        // POST: Sem2StudSchedule/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StudentID,Bell1CourseIDMod,Bell2MonWedCourseIDMod,Bell2TueThurCourseIDMod,Bell3MonWedCourseIDMod,Bell3TueThurCourseIDMod,Bell4MonWedCourseIDMod,Bell4TueThurCourseIDMod,Bell5MonWedCourseIDMod,Bell5TueThurCourseIDMod,Bell6MonWedCourseIDMod,Bell6TueThurCourseIDMod,Bell7MonWedCourseIDMod,Bell7TueThurCourseIDMod,FriBell2CourseIDMod,FriBell3CourseIDMod,FriBell4CourseIDMod,FriBell5CourseIDMod,FriBell6CourseIDMod,FriBell7CourseIDMod,AvesBellCourseIDMod,LunchCodeMod")] Sem2StudSchedule sem2StudSchedule)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sem2StudSchedule);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["StudentID"] = new SelectList(_context.StudentInfoModels, "StudentID", "StudentID", sem2StudSchedule.StudentID);
            return View(sem2StudSchedule);
        }

        // GET: Sem2StudSchedule/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sem2StudSchedule = await _context.Sem2StudSchedules.FindAsync(id);
            if (sem2StudSchedule == null)
            {
                return NotFound();
            }
            ViewData["StudentID"] = new SelectList(_context.StudentInfoModels, "StudentID", "StudentID", sem2StudSchedule.StudentID);
            return View(sem2StudSchedule);
        }

        // POST: Sem2StudSchedule/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StudentID,Bell1CourseIDMod,Bell2MonWedCourseIDMod,Bell2TueThurCourseIDMod,Bell3MonWedCourseIDMod,Bell3TueThurCourseIDMod,Bell4MonWedCourseIDMod,Bell4TueThurCourseIDMod,Bell5MonWedCourseIDMod,Bell5TueThurCourseIDMod,Bell6MonWedCourseIDMod,Bell6TueThurCourseIDMod,Bell7MonWedCourseIDMod,Bell7TueThurCourseIDMod,FriBell2CourseIDMod,FriBell3CourseIDMod,FriBell4CourseIDMod,FriBell5CourseIDMod,FriBell6CourseIDMod,FriBell7CourseIDMod,AvesBellCourseIDMod,LunchCodeMod")] Sem2StudSchedule sem2StudSchedule)
        {
            if (id != sem2StudSchedule.StudentID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sem2StudSchedule);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Sem2StudScheduleExists(sem2StudSchedule.StudentID))
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
            ViewData["StudentID"] = new SelectList(_context.StudentInfoModels, "StudentID", "StudentID", sem2StudSchedule.StudentID);
            return View(sem2StudSchedule);
        }

        // GET: Sem2StudSchedule/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sem2StudSchedule = await _context.Sem2StudSchedules
                .Include(s => s.Student)
                .FirstOrDefaultAsync(m => m.StudentID == id);
            if (sem2StudSchedule == null)
            {
                return NotFound();
            }

            return View(sem2StudSchedule);
        }

        // POST: Sem2StudSchedule/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sem2StudSchedule = await _context.Sem2StudSchedules.FindAsync(id);
            if (sem2StudSchedule != null)
            {
                _context.Sem2StudSchedules.Remove(sem2StudSchedule);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Sem2StudScheduleExists(int id)
        {
            return _context.Sem2StudSchedules.Any(e => e.StudentID == id);
        }
    }
}
