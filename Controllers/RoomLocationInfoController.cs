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
    public class RoomLocationInfoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RoomLocationInfoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: RoomLocationInfo
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.roomLocationInfoModels.Include(r => r.Teacher);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: RoomLocationInfo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roomLocationInfoModel = await _context.roomLocationInfoModels
                .Include(r => r.Teacher)
                .FirstOrDefaultAsync(m => m.RoomId == id);
            if (roomLocationInfoModel == null)
            {
                return NotFound();
            }

            return View(roomLocationInfoModel);
        }

        // GET: RoomLocationInfo/Create
        public IActionResult Create()
        {
            ViewData["RoomAssignedToTeacherID"] = new SelectList(_context.teacherInfoModels, "TeacherID", "TeacherID");
            return View();
        }

        // POST: RoomLocationInfo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RoomId,RoomNumberMod,WingNameMod,RoomCodeMod,RoomAssignedToTeacherID,RoomScannerId")] RoomLocationInfoModel roomLocationInfoModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(roomLocationInfoModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoomAssignedToTeacherID"] = new SelectList(_context.teacherInfoModels, "TeacherID", "TeacherID", roomLocationInfoModel.RoomAssignedToTeacherID);
            return View(roomLocationInfoModel);
        }

        // GET: RoomLocationInfo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roomLocationInfoModel = await _context.roomLocationInfoModels.FindAsync(id);
            if (roomLocationInfoModel == null)
            {
                return NotFound();
            }
            ViewData["RoomAssignedToTeacherID"] = new SelectList(_context.teacherInfoModels, "TeacherID", "TeacherID", roomLocationInfoModel.RoomAssignedToTeacherID);
            return View(roomLocationInfoModel);
        }

        // POST: RoomLocationInfo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RoomId,RoomNumberMod,WingNameMod,RoomCodeMod,RoomAssignedToTeacherID,RoomScannerId")] RoomLocationInfoModel roomLocationInfoModel)
        {
            if (id != roomLocationInfoModel.RoomId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(roomLocationInfoModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomLocationInfoModelExists(roomLocationInfoModel.RoomId))
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
            ViewData["RoomAssignedToTeacherID"] = new SelectList(_context.teacherInfoModels, "TeacherID", "TeacherID", roomLocationInfoModel.RoomAssignedToTeacherID);
            return View(roomLocationInfoModel);
        }

        // GET: RoomLocationInfo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roomLocationInfoModel = await _context.roomLocationInfoModels
                .Include(r => r.Teacher)
                .FirstOrDefaultAsync(m => m.RoomId == id);
            if (roomLocationInfoModel == null)
            {
                return NotFound();
            }

            return View(roomLocationInfoModel);
        }

        // POST: RoomLocationInfo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var roomLocationInfoModel = await _context.roomLocationInfoModels.FindAsync(id);
            if (roomLocationInfoModel != null)
            {
                _context.roomLocationInfoModels.Remove(roomLocationInfoModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RoomLocationInfoModelExists(int id)
        {
            return _context.roomLocationInfoModels.Any(e => e.RoomId == id);
        }
    }
}
