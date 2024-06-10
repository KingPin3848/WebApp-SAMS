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
    public class AdminInfoController(ApplicationDbContext context) : Controller
    {
        private readonly ApplicationDbContext _context = context;

        // GET: AdminInfo
        public async Task<IActionResult> Index()
        {
            return View(await _context.AdminInfoModels.ToListAsync());
        }

        // GET: AdminInfo/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adminInfoModel = await _context.AdminInfoModels
                .FirstOrDefaultAsync(m => m.AdminID == id);
            if (adminInfoModel == null)
            {
                return NotFound();
            }

            return View(adminInfoModel);
        }

        // GET: AdminInfo/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AdminInfo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AdminID,AdminFirstNameMod,AdminMiddleNameMod,AdminLastNameMod,AdminPreferredNameMod,AdminEmailMod,AdminPhoneMod,AdminLabelMod")] AdminInfoModel adminInfoModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(adminInfoModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(adminInfoModel);
        }

        // GET: AdminInfo/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adminInfoModel = await _context.AdminInfoModels.FindAsync(id);
            if (adminInfoModel == null)
            {
                return NotFound();
            }
            return View(adminInfoModel);
        }

        // POST: AdminInfo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("AdminID,AdminFirstNameMod,AdminMiddleNameMod,AdminLastNameMod,AdminPreferredNameMod,AdminEmailMod,AdminPhoneMod,AdminLabelMod")] AdminInfoModel adminInfoModel)
        {
            if (id != adminInfoModel.AdminID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(adminInfoModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdminInfoModelExists(adminInfoModel.AdminID))
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
            return View(adminInfoModel);
        }

        // GET: AdminInfo/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adminInfoModel = await _context.AdminInfoModels
                .FirstOrDefaultAsync(m => m.AdminID == id);
            if (adminInfoModel == null)
            {
                return NotFound();
            }

            return View(adminInfoModel);
        }

        // POST: AdminInfo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var adminInfoModel = await _context.AdminInfoModels.FindAsync(id);
            if (adminInfoModel != null)
            {
                _context.AdminInfoModels.Remove(adminInfoModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdminInfoModelExists(string id)
        {
            return _context.AdminInfoModels.Any(e => e.AdminID == id);
        }
    }
}
