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
    public class Sem1StudScheduleController : Controller
    {
        private readonly ApplicationDbContext _context;

        public Sem1StudScheduleController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Sem1StudSchedule
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.sem1StudSchedules.Include(s => s.Student);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Sem1StudSchedule/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sem1StudSchedule = await _context.sem1StudSchedules
                .Include(s => s.Student)
                .FirstOrDefaultAsync(m => m.StudentID == id);
            if (sem1StudSchedule == null)
            {
                return NotFound();
            }

            return View(sem1StudSchedule);
        }

        // GET: Sem1StudSchedule/Create
        public IActionResult Create()
        {
            ViewData["StudentID"] = new SelectList(_context.studentInfoModels, "StudentID", "StudentID");
            return View();
        }

        // POST: Sem1StudSchedule/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StudentID,Bell1CourseIDMod,Bell2MonWedCourseIDMod,Bell2TueThurCourseIDMod,Bell3MonWedCourseIDMod,Bell3TueThurCourseIDMod,Bell4MonWedCourseIDMod,Bell4TueThurCourseIDMod,Bell5MonWedCourseIDMod,Bell5TueThurCourseIDMod,Bell6MonWedCourseIDMod,Bell6TueThurCourseIDMod,Bell7MonWedCourseIDMod,Bell7TueThurCourseIDMod,FriBell2CourseIDMod,FriBell3CourseIDMod,FriBell4CourseIDMod,FriBell5CourseIDMod,FriBell6CourseIDMod,FriBell7CourseIDMod,AvesBellCourseIDMod,LunchCodeMod")] Sem1StudSchedule sem1StudSchedule)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sem1StudSchedule);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["StudentID"] = new SelectList(_context.studentInfoModels, "StudentID", "StudentID", sem1StudSchedule.StudentID);
            return View(sem1StudSchedule);
        }

        // GET: Sem1StudSchedule/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sem1StudSchedule = await _context.sem1StudSchedules.FindAsync(id);
            if (sem1StudSchedule == null)
            {
                return NotFound();
            }
            ViewData["StudentID"] = new SelectList(_context.studentInfoModels, "StudentID", "StudentID", sem1StudSchedule.StudentID);
            return View(sem1StudSchedule);
        }

        // POST: Sem1StudSchedule/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StudentID,Bell1CourseIDMod,Bell2MonWedCourseIDMod,Bell2TueThurCourseIDMod,Bell3MonWedCourseIDMod,Bell3TueThurCourseIDMod,Bell4MonWedCourseIDMod,Bell4TueThurCourseIDMod,Bell5MonWedCourseIDMod,Bell5TueThurCourseIDMod,Bell6MonWedCourseIDMod,Bell6TueThurCourseIDMod,Bell7MonWedCourseIDMod,Bell7TueThurCourseIDMod,FriBell2CourseIDMod,FriBell3CourseIDMod,FriBell4CourseIDMod,FriBell5CourseIDMod,FriBell6CourseIDMod,FriBell7CourseIDMod,AvesBellCourseIDMod,LunchCodeMod")] Sem1StudSchedule sem1StudSchedule)
        {
            if (id != sem1StudSchedule.StudentID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sem1StudSchedule);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Sem1StudScheduleExists(sem1StudSchedule.StudentID))
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
            ViewData["StudentID"] = new SelectList(_context.studentInfoModels, "StudentID", "StudentID", sem1StudSchedule.StudentID);
            return View(sem1StudSchedule);
        }

        // GET: Sem1StudSchedule/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sem1StudSchedule = await _context.sem1StudSchedules
                .Include(s => s.Student)
                .FirstOrDefaultAsync(m => m.StudentID == id);
            if (sem1StudSchedule == null)
            {
                return NotFound();
            }

            return View(sem1StudSchedule);
        }

        // POST: Sem1StudSchedule/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sem1StudSchedule = await _context.sem1StudSchedules.FindAsync(id);
            if (sem1StudSchedule != null)
            {
                _context.sem1StudSchedules.Remove(sem1StudSchedule);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Sem1StudScheduleExists(int id)
        {
            return _context.sem1StudSchedules.Any(e => e.StudentID == id);
        }
    }
}
