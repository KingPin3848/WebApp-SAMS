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
    public class SubstituteInfoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SubstituteInfoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SubstituteInfo
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.substituteInfoModels.Include(s => s.TeacherManaged);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: SubstituteInfo/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.substituteInfoModels == null)
            {
                return NotFound();
            }

            var substituteInfoModel = await _context.substituteInfoModels
                .Include(s => s.TeacherManaged)
                .FirstOrDefaultAsync(m => m.SubID == id);
            if (substituteInfoModel == null)
            {
                return NotFound();
            }

            return View(substituteInfoModel);
        }

        // GET: SubstituteInfo/Create
        public IActionResult Create()
        {
            ViewData["ManagedTeacherIdMod"] = new SelectList(_context.teacherInfoModels, "TeacherID", "TeacherID");
            return View();
        }

        // POST: SubstituteInfo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SubID,SubFirstNameMod,SubMiddleNameMod,SubLastNameMod,SubPreferredNameMod,SubEmailMod,SubPhoneMod,ManagedTeacherIdMod,ScheduledDate")] SubstituteInfoModel substituteInfoModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(substituteInfoModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ManagedTeacherIdMod"] = new SelectList(_context.teacherInfoModels, "TeacherID", "TeacherID", substituteInfoModel.ManagedTeacherIdMod);
            return View(substituteInfoModel);
        }

        // GET: SubstituteInfo/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.substituteInfoModels == null)
            {
                return NotFound();
            }

            var substituteInfoModel = await _context.substituteInfoModels.FindAsync(id);
            if (substituteInfoModel == null)
            {
                return NotFound();
            }
            ViewData["ManagedTeacherIdMod"] = new SelectList(_context.teacherInfoModels, "TeacherID", "TeacherID", substituteInfoModel.ManagedTeacherIdMod);
            return View(substituteInfoModel);
        }

        // POST: SubstituteInfo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("SubID,SubFirstNameMod,SubMiddleNameMod,SubLastNameMod,SubPreferredNameMod,SubEmailMod,SubPhoneMod,ManagedTeacherIdMod,ScheduledDate")] SubstituteInfoModel substituteInfoModel)
        {
            if (id != substituteInfoModel.SubID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(substituteInfoModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubstituteInfoModelExists(substituteInfoModel.SubID))
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
            ViewData["ManagedTeacherIdMod"] = new SelectList(_context.teacherInfoModels, "TeacherID", "TeacherID", substituteInfoModel.ManagedTeacherIdMod);
            return View(substituteInfoModel);
        }

        // GET: SubstituteInfo/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.substituteInfoModels == null)
            {
                return NotFound();
            }

            var substituteInfoModel = await _context.substituteInfoModels
                .Include(s => s.TeacherManaged)
                .FirstOrDefaultAsync(m => m.SubID == id);
            if (substituteInfoModel == null)
            {
                return NotFound();
            }

            return View(substituteInfoModel);
        }

        // POST: SubstituteInfo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.substituteInfoModels == null)
            {
                return Problem("Entity set 'ApplicationDbContext.substituteInfoModels'  is null.");
            }
            var substituteInfoModel = await _context.substituteInfoModels.FindAsync(id);
            if (substituteInfoModel != null)
            {
                _context.substituteInfoModels.Remove(substituteInfoModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SubstituteInfoModelExists(string id)
        {
          return (_context.substituteInfoModels?.Any(e => e.SubID == id)).GetValueOrDefault();
        }
    }
}
