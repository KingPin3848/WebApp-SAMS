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
    public class RoomScheduleController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RoomScheduleController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: RoomSchedule
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.roomScheduleModels.Include(r => r.Room).Include(r => r.Schedule).Include(r => r.Teacher);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: RoomSchedule/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.roomScheduleModels == null)
            {
                return NotFound();
            }

            var roomScheduleModel = await _context.roomScheduleModels
                .Include(r => r.Room)
                .Include(r => r.Schedule)
                .Include(r => r.Teacher)
                .FirstOrDefaultAsync(m => m.RoomScheduleID == id);
            if (roomScheduleModel == null)
            {
                return NotFound();
            }

            return View(roomScheduleModel);
        }

        // GET: RoomSchedule/Create
        public IActionResult Create()
        {
            ViewData["RoomId"] = new SelectList(_context.roomLocationInfoModels, "RoomId", "RoomId");
            ViewData["ScheduleID"] = new SelectList(_context.Set<TeachingScheduleModel>(), "ScheduleID", "ScheduleID");
            ViewData["TeacherID"] = new SelectList(_context.teacherInfoModels, "TeacherID", "TeacherID");
            return View();
        }

        // POST: RoomSchedule/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RoomScheduleID,RoomId,TeacherID,ScheduleID")] RoomScheduleModel roomScheduleModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(roomScheduleModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoomId"] = new SelectList(_context.roomLocationInfoModels, "RoomId", "RoomId", roomScheduleModel.RoomId);
            ViewData["ScheduleID"] = new SelectList(_context.Set<TeachingScheduleModel>(), "ScheduleID", "ScheduleID", roomScheduleModel.ScheduleID);
            ViewData["TeacherID"] = new SelectList(_context.teacherInfoModels, "TeacherID", "TeacherID", roomScheduleModel.TeacherID);
            return View(roomScheduleModel);
        }

        // GET: RoomSchedule/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.roomScheduleModels == null)
            {
                return NotFound();
            }

            var roomScheduleModel = await _context.roomScheduleModels.FindAsync(id);
            if (roomScheduleModel == null)
            {
                return NotFound();
            }
            ViewData["RoomId"] = new SelectList(_context.roomLocationInfoModels, "RoomId", "RoomId", roomScheduleModel.RoomId);
            ViewData["ScheduleID"] = new SelectList(_context.Set<TeachingScheduleModel>(), "ScheduleID", "ScheduleID", roomScheduleModel.ScheduleID);
            ViewData["TeacherID"] = new SelectList(_context.teacherInfoModels, "TeacherID", "TeacherID", roomScheduleModel.TeacherID);
            return View(roomScheduleModel);
        }

        // POST: RoomSchedule/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RoomScheduleID,RoomId,TeacherID,ScheduleID")] RoomScheduleModel roomScheduleModel)
        {
            if (id != roomScheduleModel.RoomScheduleID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(roomScheduleModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomScheduleModelExists(roomScheduleModel.RoomScheduleID))
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
            ViewData["RoomId"] = new SelectList(_context.roomLocationInfoModels, "RoomId", "RoomId", roomScheduleModel.RoomId);
            ViewData["ScheduleID"] = new SelectList(_context.Set<TeachingScheduleModel>(), "ScheduleID", "ScheduleID", roomScheduleModel.ScheduleID);
            ViewData["TeacherID"] = new SelectList(_context.teacherInfoModels, "TeacherID", "TeacherID", roomScheduleModel.TeacherID);
            return View(roomScheduleModel);
        }

        // GET: RoomSchedule/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.roomScheduleModels == null)
            {
                return NotFound();
            }

            var roomScheduleModel = await _context.roomScheduleModels
                .Include(r => r.Room)
                .Include(r => r.Schedule)
                .Include(r => r.Teacher)
                .FirstOrDefaultAsync(m => m.RoomScheduleID == id);
            if (roomScheduleModel == null)
            {
                return NotFound();
            }

            return View(roomScheduleModel);
        }

        // POST: RoomSchedule/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.roomScheduleModels == null)
            {
                return Problem("Entity set 'ApplicationDbContext.roomScheduleModels'  is null.");
            }
            var roomScheduleModel = await _context.roomScheduleModels.FindAsync(id);
            if (roomScheduleModel != null)
            {
                _context.roomScheduleModels.Remove(roomScheduleModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RoomScheduleModelExists(int id)
        {
          return (_context.roomScheduleModels?.Any(e => e.RoomScheduleID == id)).GetValueOrDefault();
        }
    }
}
