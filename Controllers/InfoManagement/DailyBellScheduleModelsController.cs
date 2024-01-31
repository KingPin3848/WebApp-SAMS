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
    public class DailyBellScheduleModelsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DailyBellScheduleModelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DailyBellScheduleModels
        public async Task<IActionResult> Index()
        {
            return _context.dailyBellScheduleModels != null ?
                        View(await _context.dailyBellScheduleModels.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.dailyBellScheduleModels'  is null.");
        }

        // GET: DailyBellScheduleModels/Details/5
        public async Task<IActionResult> Details(TimeSpan? id)
        {
            if (id == null || _context.dailyBellScheduleModels == null)
            {
                return NotFound();
            }

            var dailyBellScheduleModel = await _context.dailyBellScheduleModels
                .FirstOrDefaultAsync(m => m.StartTime == id);
            if (dailyBellScheduleModel == null)
            {
                return NotFound();
            }

            return View(dailyBellScheduleModel);
        }

        // GET: DailyBellScheduleModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DailyBellScheduleModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BellName,StartTime,EndTime,Duration")] DailyBellScheduleModel dailyBellScheduleModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dailyBellScheduleModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dailyBellScheduleModel);
        }

        // GET: DailyBellScheduleModels/Edit/5
        public async Task<IActionResult> Edit(TimeSpan? id)
        {
            if (id == null || _context.dailyBellScheduleModels == null)
            {
                return NotFound();
            }

            var dailyBellScheduleModel = await _context.dailyBellScheduleModels.FindAsync(id);
            if (dailyBellScheduleModel == null)
            {
                return NotFound();
            }
            return View(dailyBellScheduleModel);
        }

        // POST: DailyBellScheduleModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TimeSpan id, [Bind("BellName,StartTime,EndTime,Duration")] DailyBellScheduleModel dailyBellScheduleModel)
        {
            if (id != dailyBellScheduleModel.StartTime)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dailyBellScheduleModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DailyBellScheduleModelExists(dailyBellScheduleModel.StartTime))
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
            return View(dailyBellScheduleModel);
        }

        // GET: DailyBellScheduleModels/Delete/5
        public async Task<IActionResult> Delete(TimeSpan? id)
        {
            if (id == null || _context.dailyBellScheduleModels == null)
            {
                return NotFound();
            }

            var dailyBellScheduleModel = await _context.dailyBellScheduleModels
                .FirstOrDefaultAsync(m => m.StartTime == id);
            if (dailyBellScheduleModel == null)
            {
                return NotFound();
            }

            return View(dailyBellScheduleModel);
        }

        // POST: DailyBellScheduleModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(TimeSpan id)
        {
            if (_context.dailyBellScheduleModels == null)
            {
                return Problem("Entity set 'ApplicationDbContext.dailyBellScheduleModels'  is null.");
            }
            var dailyBellScheduleModel = await _context.dailyBellScheduleModels.FindAsync(id);
            if (dailyBellScheduleModel != null)
            {
                _context.dailyBellScheduleModels.Remove(dailyBellScheduleModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DailyBellScheduleModelExists(TimeSpan id)
        {
            return (_context.dailyBellScheduleModels?.Any(e => e.StartTime == id)).GetValueOrDefault();
        }
    }
}
