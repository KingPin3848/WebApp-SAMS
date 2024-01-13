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
    public class CourseEnrollmentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CourseEnrollmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CourseEnrollment
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.courseEnrollmentModels.Include(c => c.ActiveCourses).Include(c => c.Student).Include(c => c.StudentSchedules);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: CourseEnrollment/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.courseEnrollmentModels == null)
            {
                return NotFound();
            }

            var courseEnrollmentModel = await _context.courseEnrollmentModels
                .Include(c => c.ActiveCourses)
                .Include(c => c.Student)
                .Include(c => c.StudentSchedules)
                .FirstOrDefaultAsync(m => m.EnrollmentId == id);
            if (courseEnrollmentModel == null)
            {
                return NotFound();
            }

            return View(courseEnrollmentModel);
        }

        // GET: CourseEnrollment/Create
        public IActionResult Create()
        {
            ViewData["EnrollmentCourseId"] = new SelectList(_context.activeCourseInfoModels, "CourseId", "CourseId");
            ViewData["EnrollmentStudentId"] = new SelectList(_context.studentInfoModels, "StudentID", "StudentID");
            ViewData["EnrollmentStudentId"] = new SelectList(_context.studentScheduleInfoModels, "StudentID", "StudentID");
            return View();
        }

        // POST: CourseEnrollment/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EnrollmentId,EnrollmentStudentId,EnrollmentCourseId,EnrollmentDateTime")] CourseEnrollmentModel courseEnrollmentModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(courseEnrollmentModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EnrollmentCourseId"] = new SelectList(_context.activeCourseInfoModels, "CourseId", "CourseId", courseEnrollmentModel.EnrollmentCourseId);
            ViewData["EnrollmentStudentId"] = new SelectList(_context.studentInfoModels, "StudentID", "StudentID", courseEnrollmentModel.EnrollmentStudentId);
            ViewData["EnrollmentStudentId"] = new SelectList(_context.studentScheduleInfoModels, "StudentID", "StudentID", courseEnrollmentModel.EnrollmentStudentId);
            return View(courseEnrollmentModel);
        }

        // GET: CourseEnrollment/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.courseEnrollmentModels == null)
            {
                return NotFound();
            }

            var courseEnrollmentModel = await _context.courseEnrollmentModels.FindAsync(id);
            if (courseEnrollmentModel == null)
            {
                return NotFound();
            }
            ViewData["EnrollmentCourseId"] = new SelectList(_context.activeCourseInfoModels, "CourseId", "CourseId", courseEnrollmentModel.EnrollmentCourseId);
            ViewData["EnrollmentStudentId"] = new SelectList(_context.studentInfoModels, "StudentID", "StudentID", courseEnrollmentModel.EnrollmentStudentId);
            ViewData["EnrollmentStudentId"] = new SelectList(_context.studentScheduleInfoModels, "StudentID", "StudentID", courseEnrollmentModel.EnrollmentStudentId);
            return View(courseEnrollmentModel);
        }

        // POST: CourseEnrollment/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EnrollmentId,EnrollmentStudentId,EnrollmentCourseId,EnrollmentDateTime")] CourseEnrollmentModel courseEnrollmentModel)
        {
            if (id != courseEnrollmentModel.EnrollmentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(courseEnrollmentModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseEnrollmentModelExists(courseEnrollmentModel.EnrollmentId))
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
            ViewData["EnrollmentCourseId"] = new SelectList(_context.activeCourseInfoModels, "CourseId", "CourseId", courseEnrollmentModel.EnrollmentCourseId);
            ViewData["EnrollmentStudentId"] = new SelectList(_context.studentInfoModels, "StudentID", "StudentID", courseEnrollmentModel.EnrollmentStudentId);
            ViewData["EnrollmentStudentId"] = new SelectList(_context.studentScheduleInfoModels, "StudentID", "StudentID", courseEnrollmentModel.EnrollmentStudentId);
            return View(courseEnrollmentModel);
        }

        // GET: CourseEnrollment/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.courseEnrollmentModels == null)
            {
                return NotFound();
            }

            var courseEnrollmentModel = await _context.courseEnrollmentModels
                .Include(c => c.ActiveCourses)
                .Include(c => c.Student)
                .Include(c => c.StudentSchedules)
                .FirstOrDefaultAsync(m => m.EnrollmentId == id);
            if (courseEnrollmentModel == null)
            {
                return NotFound();
            }

            return View(courseEnrollmentModel);
        }

        // POST: CourseEnrollment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.courseEnrollmentModels == null)
            {
                return Problem("Entity set 'ApplicationDbContext.courseEnrollmentModels'  is null.");
            }
            var courseEnrollmentModel = await _context.courseEnrollmentModels.FindAsync(id);
            if (courseEnrollmentModel != null)
            {
                _context.courseEnrollmentModels.Remove(courseEnrollmentModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseEnrollmentModelExists(int id)
        {
          return (_context.courseEnrollmentModels?.Any(e => e.EnrollmentId == id)).GetValueOrDefault();
        }
    }
}
