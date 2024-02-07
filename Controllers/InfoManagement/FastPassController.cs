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
    public class FastPassController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FastPassController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: FastPass
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.fastPassModels.Include(f => f.Room).Include(f => f.Student).Include(f => f.StudentSchedule);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: FastPass/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fastPassModel = await _context.fastPassModels
                .Include(f => f.Room)
                .Include(f => f.Student)
                .Include(f => f.StudentSchedule)
                .FirstOrDefaultAsync(m => m.FastPassIDMod == id);
            if (fastPassModel == null)
            {
                return NotFound();
            }

            return View(fastPassModel);
        }

        // GET: FastPass/Create
        public IActionResult Create()
        {
            ViewData["EndLocationID"] = new SelectList(_context.roomLocationInfoModels, "RoomId", "RoomId");
            ViewData["StudentID"] = new SelectList(_context.studentInfoModels, "StudentID", "StudentID");
            ViewData["CourseIDFromStudentSchedule"] = new SelectList(_context.studentScheduleInfoModels, "StudentID", "StudentID");
            return View();
        }

        // POST: FastPass/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FastPassIDMod,StudentID,StudentNameMod,StartDateTime,EndDateTime,Duration,BellNumber,StartLocationID,EndLocationID,CourseIDFromStudentSchedule")] FastPassModel fastPassModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(fastPassModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EndLocationID"] = new SelectList(_context.roomLocationInfoModels, "RoomId", "RoomId", fastPassModel.EndLocationID);
            ViewData["StudentID"] = new SelectList(_context.studentInfoModels, "StudentID", "StudentID", fastPassModel.StudentID);
            ViewData["CourseIDFromStudentSchedule"] = new SelectList(_context.studentScheduleInfoModels, "StudentID", "StudentID", fastPassModel.CourseIDFromStudentSchedule);
            return View(fastPassModel);
        }

        // GET: FastPass/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fastPassModel = await _context.fastPassModels.FindAsync(id);
            if (fastPassModel == null)
            {
                return NotFound();
            }
            ViewData["EndLocationID"] = new SelectList(_context.roomLocationInfoModels, "RoomId", "RoomId", fastPassModel.EndLocationID);
            ViewData["StudentID"] = new SelectList(_context.studentInfoModels, "StudentID", "StudentID", fastPassModel.StudentID);
            ViewData["CourseIDFromStudentSchedule"] = new SelectList(_context.studentScheduleInfoModels, "StudentID", "StudentID", fastPassModel.CourseIDFromStudentSchedule);
            return View(fastPassModel);
        }

        // POST: FastPass/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("FastPassIDMod,StudentID,StudentNameMod,StartDateTime,EndDateTime,Duration,BellNumber,StartLocationID,EndLocationID,CourseIDFromStudentSchedule")] FastPassModel fastPassModel)
        {
            if (id != fastPassModel.FastPassIDMod)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fastPassModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FastPassModelExists(fastPassModel.FastPassIDMod))
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
            ViewData["EndLocationID"] = new SelectList(_context.roomLocationInfoModels, "RoomId", "RoomId", fastPassModel.EndLocationID);
            ViewData["StudentID"] = new SelectList(_context.studentInfoModels, "StudentID", "StudentID", fastPassModel.StudentID);
            ViewData["CourseIDFromStudentSchedule"] = new SelectList(_context.studentScheduleInfoModels, "StudentID", "StudentID", fastPassModel.CourseIDFromStudentSchedule);
            return View(fastPassModel);
        }

        // GET: FastPass/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fastPassModel = await _context.fastPassModels
                .Include(f => f.Room)
                .Include(f => f.Student)
                .Include(f => f.StudentSchedule)
                .FirstOrDefaultAsync(m => m.FastPassIDMod == id);
            if (fastPassModel == null)
            {
                return NotFound();
            }

            return View(fastPassModel);
        }

        // POST: FastPass/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var fastPassModel = await _context.fastPassModels.FindAsync(id);
            if (fastPassModel != null)
            {
                _context.fastPassModels.Remove(fastPassModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FastPassModelExists(string id)
        {
            return _context.fastPassModels.Any(e => e.FastPassIDMod == id);
        }
    }
}
