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
    public class TwoHrDelayBellScheduleController(ApplicationDbContext context) : Controller
    {
        private readonly ApplicationDbContext _context = context;

        // GET: TwoHrDelayBellSchedule
        public async Task<IActionResult> Index()
        {
              return _context.TwoHrDelayBellScheduleModels != null ? 
                          View(await _context.TwoHrDelayBellScheduleModels.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.twoHrDelayBellScheduleModels'  is null.");
        }

        // GET: TwoHrDelayBellSchedule/Details/5
        public async Task<IActionResult> Details(TimeSpan? id)
        {
            if (id == null || _context.TwoHrDelayBellScheduleModels == null)
            {
                return NotFound();
            }

            var twoHrDelayBellScheduleModel = await _context.TwoHrDelayBellScheduleModels
                .FirstOrDefaultAsync(m => m.StartTime == id);
            if (twoHrDelayBellScheduleModel == null)
            {
                return NotFound();
            }

            return View(twoHrDelayBellScheduleModel);
        }

        // GET: TwoHrDelayBellSchedule/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TwoHrDelayBellSchedule/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BellName,StartTime,EndTime,Duration")] TwoHrDelayBellScheduleModel twoHrDelayBellScheduleModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(twoHrDelayBellScheduleModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(twoHrDelayBellScheduleModel);
        }

        // GET: TwoHrDelayBellSchedule/Edit/5
        public async Task<IActionResult> Edit(TimeSpan? id)
        {
            if (id == null || _context.TwoHrDelayBellScheduleModels == null)
            {
                return NotFound();
            }

            var twoHrDelayBellScheduleModel = await _context.TwoHrDelayBellScheduleModels.FindAsync(id);
            if (twoHrDelayBellScheduleModel == null)
            {
                return NotFound();
            }
            return View(twoHrDelayBellScheduleModel);
        }

        // POST: TwoHrDelayBellSchedule/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TimeSpan id, [Bind("BellName,StartTime,EndTime,Duration")] TwoHrDelayBellScheduleModel twoHrDelayBellScheduleModel)
        {
            if (id != twoHrDelayBellScheduleModel.StartTime)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(twoHrDelayBellScheduleModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TwoHrDelayBellScheduleModelExists(twoHrDelayBellScheduleModel.StartTime))
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
            return View(twoHrDelayBellScheduleModel);
        }

        // GET: TwoHrDelayBellSchedule/Delete/5
        public async Task<IActionResult> Delete(TimeSpan? id)
        {
            if (id == null || _context.TwoHrDelayBellScheduleModels == null)
            {
                return NotFound();
            }

            var twoHrDelayBellScheduleModel = await _context.TwoHrDelayBellScheduleModels
                .FirstOrDefaultAsync(m => m.StartTime == id);
            if (twoHrDelayBellScheduleModel == null)
            {
                return NotFound();
            }

            return View(twoHrDelayBellScheduleModel);
        }

        // POST: TwoHrDelayBellSchedule/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(TimeSpan id)
        {
            if (_context.TwoHrDelayBellScheduleModels == null)
            {
                return Problem("Entity set 'ApplicationDbContext.twoHrDelayBellScheduleModels'  is null.");
            }
            var twoHrDelayBellScheduleModel = await _context.TwoHrDelayBellScheduleModels.FindAsync(id);
            if (twoHrDelayBellScheduleModel != null)
            {
                _context.TwoHrDelayBellScheduleModels.Remove(twoHrDelayBellScheduleModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TwoHrDelayBellScheduleModelExists(TimeSpan id)
        {
          return (_context.TwoHrDelayBellScheduleModels?.Any(e => e.StartTime == id)).GetValueOrDefault();
        }
    }
}
