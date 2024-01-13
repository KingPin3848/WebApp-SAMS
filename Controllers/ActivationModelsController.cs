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
    public class ActivationModelsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ActivationModelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ActivationModels
        public async Task<IActionResult> Index()
        {
              return _context.activationModels != null ? 
                          View(await _context.activationModels.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.activationModels'  is null.");
        }

        // GET: ActivationModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.activationModels == null)
            {
                return NotFound();
            }

            var activationModel = await _context.activationModels
                .FirstOrDefaultAsync(m => m.StudId == id);
            if (activationModel == null)
            {
                return NotFound();
            }

            return View(activationModel);
        }

        // GET: ActivationModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ActivationModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StudId,Code")] ActivationModel activationModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(activationModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(activationModel);
        }

        // GET: ActivationModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.activationModels == null)
            {
                return NotFound();
            }

            var activationModel = await _context.activationModels
                .FirstOrDefaultAsync(m => m.StudId == id);
            if (activationModel == null)
            {
                return NotFound();
            }

            return View(activationModel);
        }

        // POST: ActivationModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.activationModels == null)
            {
                return Problem("Entity set 'ApplicationDbContext.activationModels'  is null.");
            }
            var activationModel = await _context.activationModels.FindAsync(id);
            if (activationModel != null)
            {
                _context.activationModels.Remove(activationModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ActivationModelExists(int id)
        {
          return (_context.activationModels?.Any(e => e.StudId == id)).GetValueOrDefault();
        }
    }
}
