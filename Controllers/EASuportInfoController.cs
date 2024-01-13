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
    public class EASuportInfoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EASuportInfoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: EASuportInfo
        public async Task<IActionResult> Index()
        {
              return _context.eASuportInfoModels != null ? 
                          View(await _context.eASuportInfoModels.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.eASuportInfoModels'  is null.");
        }

        // GET: EASuportInfo/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.eASuportInfoModels == null)
            {
                return NotFound();
            }

            var eASuportInfoModel = await _context.eASuportInfoModels
                .FirstOrDefaultAsync(m => m.EaID == id);
            if (eASuportInfoModel == null)
            {
                return NotFound();
            }

            return View(eASuportInfoModel);
        }

        // GET: EASuportInfo/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: EASuportInfo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EaID,EaFirstNameMod,EaMiddleNameMod,EaLastNameMod,EaPreferredNameMod,EaEmailMod,EaPhoneMod,EaStudentManaged")] EASuportInfoModel eASuportInfoModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(eASuportInfoModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(eASuportInfoModel);
        }

        // GET: EASuportInfo/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.eASuportInfoModels == null)
            {
                return NotFound();
            }

            var eASuportInfoModel = await _context.eASuportInfoModels.FindAsync(id);
            if (eASuportInfoModel == null)
            {
                return NotFound();
            }
            return View(eASuportInfoModel);
        }

        // POST: EASuportInfo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("EaID,EaFirstNameMod,EaMiddleNameMod,EaLastNameMod,EaPreferredNameMod,EaEmailMod,EaPhoneMod,EaStudentManaged")] EASuportInfoModel eASuportInfoModel)
        {
            if (id != eASuportInfoModel.EaID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(eASuportInfoModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EASuportInfoModelExists(eASuportInfoModel.EaID))
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
            return View(eASuportInfoModel);
        }

        // GET: EASuportInfo/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.eASuportInfoModels == null)
            {
                return NotFound();
            }

            var eASuportInfoModel = await _context.eASuportInfoModels
                .FirstOrDefaultAsync(m => m.EaID == id);
            if (eASuportInfoModel == null)
            {
                return NotFound();
            }

            return View(eASuportInfoModel);
        }

        // POST: EASuportInfo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.eASuportInfoModels == null)
            {
                return Problem("Entity set 'ApplicationDbContext.eASuportInfoModels'  is null.");
            }
            var eASuportInfoModel = await _context.eASuportInfoModels.FindAsync(id);
            if (eASuportInfoModel != null)
            {
                _context.eASuportInfoModels.Remove(eASuportInfoModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EASuportInfoModelExists(string id)
        {
          return (_context.eASuportInfoModels?.Any(e => e.EaID == id)).GetValueOrDefault();
        }
    }
}
