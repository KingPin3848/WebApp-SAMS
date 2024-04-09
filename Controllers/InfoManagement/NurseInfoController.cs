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
    public class NurseInfoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NurseInfoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: NurseInfo
        public async Task<IActionResult> Index()
        {
            return View(await _context.nurseInfoModels.ToListAsync());
        }

        // GET: NurseInfo/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nurseInfoModel = await _context.nurseInfoModels
                .FirstOrDefaultAsync(m => m.NurseID == id);
            if (nurseInfoModel == null)
            {
                return NotFound();
            }

            return View(nurseInfoModel);
        }

        // GET: NurseInfo/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: NurseInfo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NurseID,NurseFirstNameMod,NurseMiddleNameMod,NurseLastNameMod,NursePreferredNameMod,NurseEmailMod,NursePhoneMod")] NurseInfoModel nurseInfoModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(nurseInfoModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(nurseInfoModel);
        }

        // GET: NurseInfo/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nurseInfoModel = await _context.nurseInfoModels.FindAsync(id);
            if (nurseInfoModel == null)
            {
                return NotFound();
            }
            return View(nurseInfoModel);
        }

        // POST: NurseInfo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("NurseID,NurseFirstNameMod,NurseMiddleNameMod,NurseLastNameMod,NursePreferredNameMod,NurseEmailMod,NursePhoneMod")] NurseInfoModel nurseInfoModel)
        {
            if (id != nurseInfoModel.NurseID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nurseInfoModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NurseInfoModelExists(nurseInfoModel.NurseID))
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
            return View(nurseInfoModel);
        }

        // GET: NurseInfo/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nurseInfoModel = await _context.nurseInfoModels
                .FirstOrDefaultAsync(m => m.NurseID == id);
            if (nurseInfoModel == null)
            {
                return NotFound();
            }

            return View(nurseInfoModel);
        }

        // POST: NurseInfo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var nurseInfoModel = await _context.nurseInfoModels.FindAsync(id);
            if (nurseInfoModel != null)
            {
                _context.nurseInfoModels.Remove(nurseInfoModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NurseInfoModelExists(string id)
        {
            return _context.nurseInfoModels.Any(e => e.NurseID == id);
        }
    }
}
