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
    public class DeveloperInfoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DeveloperInfoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DeveloperInfo
        public async Task<IActionResult> Index()
        {
            return View(await _context.developerInfoModels.ToListAsync());
        }

        // GET: DeveloperInfo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var developerInfoModel = await _context.developerInfoModels
                .FirstOrDefaultAsync(m => m.DeveloperID == id);
            if (developerInfoModel == null)
            {
                return NotFound();
            }

            return View(developerInfoModel);
        }

        // GET: DeveloperInfo/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DeveloperInfo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DeveloperID,DeveloperFirstNameMod,DeveloperMiddleNameMod,DeveloperLastNameMod,DeveloperPreferredNameMod,DeveloperEmailMod")] DeveloperInfoModel developerInfoModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(developerInfoModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(developerInfoModel);
        }

        // GET: DeveloperInfo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var developerInfoModel = await _context.developerInfoModels.FindAsync(id);
            if (developerInfoModel == null)
            {
                return NotFound();
            }
            return View(developerInfoModel);
        }

        // POST: DeveloperInfo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DeveloperID,DeveloperFirstNameMod,DeveloperMiddleNameMod,DeveloperLastNameMod,DeveloperPreferredNameMod,DeveloperEmailMod")] DeveloperInfoModel developerInfoModel)
        {
            if (id != developerInfoModel.DeveloperID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(developerInfoModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DeveloperInfoModelExists(developerInfoModel.DeveloperID))
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
            return View(developerInfoModel);
        }

        // GET: DeveloperInfo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var developerInfoModel = await _context.developerInfoModels
                .FirstOrDefaultAsync(m => m.DeveloperID == id);
            if (developerInfoModel == null)
            {
                return NotFound();
            }

            return View(developerInfoModel);
        }

        // POST: DeveloperInfo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var developerInfoModel = await _context.developerInfoModels.FindAsync(id);
            if (developerInfoModel != null)
            {
                _context.developerInfoModels.Remove(developerInfoModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DeveloperInfoModelExists(int id)
        {
            return _context.developerInfoModels.Any(e => e.DeveloperID == id);
        }
    }
}
