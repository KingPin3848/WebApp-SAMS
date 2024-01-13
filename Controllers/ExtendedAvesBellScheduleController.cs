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
    public class ExtendedAvesBellScheduleController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExtendedAvesBellScheduleController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ExtendedAvesBellSchedule
        public async Task<IActionResult> Index()
        {
              return _context.extendedAvesModels != null ? 
                          View(await _context.extendedAvesModels.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.extendedAvesModels'  is null.");
        }

        // GET: ExtendedAvesBellSchedule/Details/5
        public async Task<IActionResult> Details(TimeSpan? id)
        {
            if (id == null || _context.extendedAvesModels == null)
            {
                return NotFound();
            }

            var extendedAvesBellScheduleModel = await _context.extendedAvesModels
                .FirstOrDefaultAsync(m => m.StartTime == id);
            if (extendedAvesBellScheduleModel == null)
            {
                return NotFound();
            }

            return View(extendedAvesBellScheduleModel);
        }

        // GET: ExtendedAvesBellSchedule/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ExtendedAvesBellSchedule/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BellName,StartTime,EndTime,Duration")] ExtendedAvesBellScheduleModel extendedAvesBellScheduleModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(extendedAvesBellScheduleModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(extendedAvesBellScheduleModel);
        }

        // GET: ExtendedAvesBellSchedule/Edit/5
        public async Task<IActionResult> Edit(TimeSpan? id)
        {
            if (id == null || _context.extendedAvesModels == null)
            {
                return NotFound();
            }

            var extendedAvesBellScheduleModel = await _context.extendedAvesModels.FindAsync(id);
            if (extendedAvesBellScheduleModel == null)
            {
                return NotFound();
            }
            return View(extendedAvesBellScheduleModel);
        }

        // POST: ExtendedAvesBellSchedule/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TimeSpan id, [Bind("BellName,StartTime,EndTime,Duration")] ExtendedAvesBellScheduleModel extendedAvesBellScheduleModel)
        {
            if (id != extendedAvesBellScheduleModel.StartTime)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(extendedAvesBellScheduleModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExtendedAvesBellScheduleModelExists(extendedAvesBellScheduleModel.StartTime))
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
            return View(extendedAvesBellScheduleModel);
        }

        // GET: ExtendedAvesBellSchedule/Delete/5
        public async Task<IActionResult> Delete(TimeSpan? id)
        {
            if (id == null || _context.extendedAvesModels == null)
            {
                return NotFound();
            }

            var extendedAvesBellScheduleModel = await _context.extendedAvesModels
                .FirstOrDefaultAsync(m => m.StartTime == id);
            if (extendedAvesBellScheduleModel == null)
            {
                return NotFound();
            }

            return View(extendedAvesBellScheduleModel);
        }

        // POST: ExtendedAvesBellSchedule/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(TimeSpan id)
        {
            if (_context.extendedAvesModels == null)
            {
                return Problem("Entity set 'ApplicationDbContext.extendedAvesModels'  is null.");
            }
            var extendedAvesBellScheduleModel = await _context.extendedAvesModels.FindAsync(id);
            if (extendedAvesBellScheduleModel != null)
            {
                _context.extendedAvesModels.Remove(extendedAvesBellScheduleModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExtendedAvesBellScheduleModelExists(TimeSpan id)
        {
          return (_context.extendedAvesModels?.Any(e => e.StartTime == id)).GetValueOrDefault();
        }
    }
}
