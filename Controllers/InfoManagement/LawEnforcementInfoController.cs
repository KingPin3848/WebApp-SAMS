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
    public class LawEnforcementInfoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LawEnforcementInfoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: LawEnforcementInfo
        public async Task<IActionResult> Index()
        {
            return View("~/Views/InfoManagement/LawEnforcementInfo/Index.cshtml", await _context.lawEnforcementInfoModels.ToListAsync());
        }

        // GET: LawEnforcementInfo/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lawEnforcementInfoModel = await _context.lawEnforcementInfoModels
                .FirstOrDefaultAsync(m => m.LawenfID == id);
            if (lawEnforcementInfoModel == null)
            {
                return NotFound();
            }

            return View("~/Views/InfoManagement/LawEnforcementInfo/Details.cshtml", lawEnforcementInfoModel);
        }

        // GET: LawEnforcementInfo/Create
        public IActionResult Create()
        {
            return View("~/Views/InfoManagement/LawEnforcementInfo/Create.cshtml");
        }

        // POST: LawEnforcementInfo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LawenfID,LaweFirstNameMod,LaweMiddleNameMod,LaweLastNameMod,LawePreferredNameMod,LaweEmailMod,LawePhoneMod")] LawEnforcementInfoModel lawEnforcementInfoModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lawEnforcementInfoModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View("~/Views/InfoManagement/LawEnforcementInfo/Create.cshtml", lawEnforcementInfoModel);
        }

        // GET: LawEnforcementInfo/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lawEnforcementInfoModel = await _context.lawEnforcementInfoModels.FindAsync(id);
            if (lawEnforcementInfoModel == null)
            {
                return NotFound();
            }
            return View("~/Views/InfoManagement/LawEnforcementInfo/Edit.cshtml", lawEnforcementInfoModel);
        }

        // POST: LawEnforcementInfo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("LawenfID,LaweFirstNameMod,LaweMiddleNameMod,LaweLastNameMod,LawePreferredNameMod,LaweEmailMod,LawePhoneMod")] LawEnforcementInfoModel lawEnforcementInfoModel)
        {
            if (id != lawEnforcementInfoModel.LawenfID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lawEnforcementInfoModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LawEnforcementInfoModelExists(lawEnforcementInfoModel.LawenfID))
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
            return View("~/Views/InfoManagement/LawEnforcementInfo/Edit.cshtml", lawEnforcementInfoModel);
        }

        // GET: LawEnforcementInfo/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lawEnforcementInfoModel = await _context.lawEnforcementInfoModels
                .FirstOrDefaultAsync(m => m.LawenfID == id);
            if (lawEnforcementInfoModel == null)
            {
                return NotFound();
            }

            return View("~/Views/InfoManagement/LawEnforcementInfo/Delete.cshtml", lawEnforcementInfoModel);
        }

        // POST: LawEnforcementInfo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var lawEnforcementInfoModel = await _context.lawEnforcementInfoModels.FindAsync(id);
            if (lawEnforcementInfoModel != null)
            {
                _context.lawEnforcementInfoModels.Remove(lawEnforcementInfoModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LawEnforcementInfoModelExists(string id)
        {
            return _context.lawEnforcementInfoModels.Any(e => e.LawenfID == id);
        }
    }
}
