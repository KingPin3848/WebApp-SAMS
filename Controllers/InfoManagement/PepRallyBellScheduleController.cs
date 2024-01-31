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
    public class PepRallyBellScheduleController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PepRallyBellScheduleController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PepRallyBellSchedule
        public async Task<IActionResult> Index()
        {
            return _context.pepRallyBellScheduleModels != null ?
                        View(await _context.pepRallyBellScheduleModels.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.pepRallyBellScheduleModels'  is null.");
        }

        // GET: PepRallyBellSchedule/Details/5
        public async Task<IActionResult> Details(TimeSpan? id)
        {
            if (id == null || _context.pepRallyBellScheduleModels == null)
            {
                return NotFound();
            }

            var pepRallyBellScheduleModel = await _context.pepRallyBellScheduleModels
                .FirstOrDefaultAsync(m => m.StartTime == id);
            if (pepRallyBellScheduleModel == null)
            {
                return NotFound();
            }

            return View(pepRallyBellScheduleModel);
        }

        // GET: PepRallyBellSchedule/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PepRallyBellSchedule/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BellName,StartTime,EndTime,Duration")] PepRallyBellScheduleModel pepRallyBellScheduleModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pepRallyBellScheduleModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(pepRallyBellScheduleModel);
        }

        // GET: PepRallyBellSchedule/Edit/5
        public async Task<IActionResult> Edit(TimeSpan? id)
        {
            if (id == null || _context.pepRallyBellScheduleModels == null)
            {
                return NotFound();
            }

            var pepRallyBellScheduleModel = await _context.pepRallyBellScheduleModels.FindAsync(id);
            if (pepRallyBellScheduleModel == null)
            {
                return NotFound();
            }
            return View(pepRallyBellScheduleModel);
        }

        // POST: PepRallyBellSchedule/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TimeSpan id, [Bind("BellName,StartTime,EndTime,Duration")] PepRallyBellScheduleModel pepRallyBellScheduleModel)
        {
            if (id != pepRallyBellScheduleModel.StartTime)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pepRallyBellScheduleModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PepRallyBellScheduleModelExists(pepRallyBellScheduleModel.StartTime))
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
            return View(pepRallyBellScheduleModel);
        }

        // GET: PepRallyBellSchedule/Delete/5
        public async Task<IActionResult> Delete(TimeSpan? id)
        {
            if (id == null || _context.pepRallyBellScheduleModels == null)
            {
                return NotFound();
            }

            var pepRallyBellScheduleModel = await _context.pepRallyBellScheduleModels
                .FirstOrDefaultAsync(m => m.StartTime == id);
            if (pepRallyBellScheduleModel == null)
            {
                return NotFound();
            }

            return View(pepRallyBellScheduleModel);
        }

        // POST: PepRallyBellSchedule/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(TimeSpan id)
        {
            if (_context.pepRallyBellScheduleModels == null)
            {
                return Problem("Entity set 'ApplicationDbContext.pepRallyBellScheduleModels'  is null.");
            }
            var pepRallyBellScheduleModel = await _context.pepRallyBellScheduleModels.FindAsync(id);
            if (pepRallyBellScheduleModel != null)
            {
                _context.pepRallyBellScheduleModels.Remove(pepRallyBellScheduleModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PepRallyBellScheduleModelExists(TimeSpan id)
        {
            return (_context.pepRallyBellScheduleModels?.Any(e => e.StartTime == id)).GetValueOrDefault();
        }
    }
}
