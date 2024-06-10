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
    public class CounselorController(ApplicationDbContext context) : Controller
    {
        private readonly ApplicationDbContext _context = context;

        // GET: Counselor
        public async Task<IActionResult> Index()
        {
            return View(await _context.CounselorModels.ToListAsync());
        }

        // GET: Counselor/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var counselorModel = await _context.CounselorModels
                .FirstOrDefaultAsync(m => m.CounselorId == id);
            if (counselorModel == null)
            {
                return NotFound();
            }

            return View(counselorModel);
        }

        // GET: Counselor/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Counselor/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CounselorId,CounselorFirstName,CounselorMiddleName,CounselorLastName,CounselorPreferredName,CounselorEmail,CounselorPhone")] CounselorModel counselorModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(counselorModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(counselorModel);
        }

        // GET: Counselor/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var counselorModel = await _context.CounselorModels.FindAsync(id);
            if (counselorModel == null)
            {
                return NotFound();
            }
            return View(counselorModel);
        }

        // POST: Counselor/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("CounselorId,CounselorFirstName,CounselorMiddleName,CounselorLastName,CounselorPreferredName,CounselorEmail,CounselorPhone")] CounselorModel counselorModel)
        {
            if (id != counselorModel.CounselorId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(counselorModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CounselorModelExists(counselorModel.CounselorId))
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
            return View(counselorModel);
        }

        // GET: Counselor/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var counselorModel = await _context.CounselorModels
                .FirstOrDefaultAsync(m => m.CounselorId == id);
            if (counselorModel == null)
            {
                return NotFound();
            }

            return View(counselorModel);
        }

        // POST: Counselor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var counselorModel = await _context.CounselorModels.FindAsync(id);
            if (counselorModel != null)
            {
                _context.CounselorModels.Remove(counselorModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CounselorModelExists(string id)
        {
            return _context.CounselorModels.Any(e => e.CounselorId == id);
        }
    }
}
