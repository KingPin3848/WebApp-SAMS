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
    public class CustomScheduleController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomScheduleController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CustomSchedule
        public async Task<IActionResult> Index()
        {
            return View(await _context.CustomSchedules.ToListAsync());
        }

        // GET: CustomSchedule/Details/5
        public async Task<IActionResult> Details(TimeSpan? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customScheduleModel = await _context.CustomSchedules
                .FirstOrDefaultAsync(m => m.StartTime == id);
            if (customScheduleModel == null)
            {
                return NotFound();
            }

            return View(customScheduleModel);
        }

        // GET: CustomSchedule/Create
        public IActionResult Create()
        {
            ViewData["BellNames"] = new SelectList(_context.CustomSchedules, "BellName", "BellName");
            return View();
        }

        // POST: CustomSchedule/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BellName,StartTime,EndTime,Duration")] CustomScheduleModel customScheduleModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customScheduleModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customScheduleModel);
        }

        // GET: CustomSchedule/Edit/5
        public async Task<IActionResult> Edit(TimeSpan? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customScheduleModel = await _context.CustomSchedules.FindAsync(id);
            if (customScheduleModel == null)
            {
                return NotFound();
            }
            ViewData["BellNames"] = new SelectList(_context.CustomSchedules, "BellName", "BellName");
            return View(customScheduleModel);
        }

        // POST: CustomSchedule/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TimeSpan id, [Bind("BellName,StartTime,EndTime,Duration")] CustomScheduleModel customScheduleModel)
        {
            if (id != customScheduleModel.StartTime)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customScheduleModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomScheduleModelExists(customScheduleModel.StartTime))
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
            return View(customScheduleModel);
        }

        // GET: CustomSchedule/Delete/5
        public async Task<IActionResult> Delete(TimeSpan? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customScheduleModel = await _context.CustomSchedules
                .FirstOrDefaultAsync(m => m.StartTime == id);
            if (customScheduleModel == null)
            {
                return NotFound();
            }

            return View(customScheduleModel);
        }

        // POST: CustomSchedule/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(TimeSpan id)
        {
            var customScheduleModel = await _context.CustomSchedules.FindAsync(id);
            if (customScheduleModel != null)
            {
                _context.CustomSchedules.Remove(customScheduleModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomScheduleModelExists(TimeSpan id)
        {
            return _context.CustomSchedules.Any(e => e.StartTime == id);
        }
    }
}
