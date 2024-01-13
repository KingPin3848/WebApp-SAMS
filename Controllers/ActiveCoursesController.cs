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
    public class ActiveCoursesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ActiveCoursesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ActiveCourses
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.activeCourseInfoModels.Include(a => a.Room).Include(a => a.Teacher);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ActiveCourses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.activeCourseInfoModels == null)
            {
                return NotFound();
            }

            var activeCourseInfoModel = await _context.activeCourseInfoModels
                .Include(a => a.Room)
                .Include(a => a.Teacher)
                .FirstOrDefaultAsync(m => m.CourseId == id);
            if (activeCourseInfoModel == null)
            {
                return NotFound();
            }

            return View(activeCourseInfoModel);
        }

        // GET: ActiveCourses/Create
        public IActionResult Create()
        {
            ViewData["CourseRoomID"] = new SelectList(_context.roomLocationInfoModels, "RoomId", "RoomId");
            ViewData["CourseTeacherID"] = new SelectList(_context.teacherInfoModels, "TeacherID", "TeacherID");
            return View();
        }

        // POST: ActiveCourses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CourseId,CourseName,CourseCode,CourseLevel,CourseTeacherID,CourseRoomID,CourseBellNumber,CourseLength,CourseTaughtDays")] ActiveCourseInfoModel activeCourseInfoModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(activeCourseInfoModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseRoomID"] = new SelectList(_context.roomLocationInfoModels, "RoomId", "RoomId", activeCourseInfoModel.CourseRoomID);
            ViewData["CourseTeacherID"] = new SelectList(_context.teacherInfoModels, "TeacherID", "TeacherID", activeCourseInfoModel.CourseTeacherID);
            return View(activeCourseInfoModel);
        }

        // GET: ActiveCourses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.activeCourseInfoModels == null)
            {
                return NotFound();
            }

            var activeCourseInfoModel = await _context.activeCourseInfoModels.FindAsync(id);
            if (activeCourseInfoModel == null)
            {
                return NotFound();
            }
            ViewData["CourseRoomID"] = new SelectList(_context.roomLocationInfoModels, "RoomId", "RoomId", activeCourseInfoModel.CourseRoomID);
            ViewData["CourseTeacherID"] = new SelectList(_context.teacherInfoModels, "TeacherID", "TeacherID", activeCourseInfoModel.CourseTeacherID);
            return View(activeCourseInfoModel);
        }

        // POST: ActiveCourses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CourseId,CourseName,CourseCode,CourseLevel,CourseTeacherID,CourseRoomID,CourseBellNumber,CourseLength,CourseTaughtDays")] ActiveCourseInfoModel activeCourseInfoModel)
        {
            if (id != activeCourseInfoModel.CourseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(activeCourseInfoModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActiveCourseInfoModelExists(activeCourseInfoModel.CourseId))
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
            ViewData["CourseRoomID"] = new SelectList(_context.roomLocationInfoModels, "RoomId", "RoomId", activeCourseInfoModel.CourseRoomID);
            ViewData["CourseTeacherID"] = new SelectList(_context.teacherInfoModels, "TeacherID", "TeacherID", activeCourseInfoModel.CourseTeacherID);
            return View(activeCourseInfoModel);
        }

        // GET: ActiveCourses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.activeCourseInfoModels == null)
            {
                return NotFound();
            }

            var activeCourseInfoModel = await _context.activeCourseInfoModels
                .Include(a => a.Room)
                .Include(a => a.Teacher)
                .FirstOrDefaultAsync(m => m.CourseId == id);
            if (activeCourseInfoModel == null)
            {
                return NotFound();
            }

            return View(activeCourseInfoModel);
        }

        // POST: ActiveCourses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.activeCourseInfoModels == null)
            {
                return Problem("Entity set 'ApplicationDbContext.activeCourseInfoModels'  is null.");
            }
            var activeCourseInfoModel = await _context.activeCourseInfoModels.FindAsync(id);
            if (activeCourseInfoModel != null)
            {
                _context.activeCourseInfoModels.Remove(activeCourseInfoModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ActiveCourseInfoModelExists(int id)
        {
          return (_context.activeCourseInfoModels?.Any(e => e.CourseId == id)).GetValueOrDefault();
        }
    }
}
