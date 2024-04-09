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
    public class SchedulerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SchedulerController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Scheduler
        public async Task<IActionResult> Index()
        {
            return View(await _context.schedulerModels.ToListAsync());
        }

        // GET: Scheduler/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schedulerModel = await _context.schedulerModels
                .FirstOrDefaultAsync(m => m.Id == id);
            if (schedulerModel == null)
            {
                return NotFound();
            }

            return View(schedulerModel);
        }

        // GET: Scheduler/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Scheduler/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NameOfEvent,Date,Type")] SchedulerModel schedulerModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(schedulerModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(schedulerModel);
        }

        // GET: Scheduler/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schedulerModel = await _context.schedulerModels.FindAsync(id);
            if (schedulerModel == null)
            {
                return NotFound();
            }
            return View(schedulerModel);
        }

        // POST: Scheduler/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NameOfEvent,Date,Type")] SchedulerModel schedulerModel)
        {
            if (id != schedulerModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(schedulerModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SchedulerModelExists(schedulerModel.Id))
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
            return View(schedulerModel);
        }

        // GET: Scheduler/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schedulerModel = await _context.schedulerModels
                .FirstOrDefaultAsync(m => m.Id == id);
            if (schedulerModel == null)
            {
                return NotFound();
            }

            return View(schedulerModel);
        }

        // POST: Scheduler/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var schedulerModel = await _context.schedulerModels.FindAsync(id);
            if (schedulerModel != null)
            {
                _context.schedulerModels.Remove(schedulerModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SchedulerModelExists(int id)
        {
            return _context.schedulerModels.Any(e => e.Id == id);
        }
    }
}
