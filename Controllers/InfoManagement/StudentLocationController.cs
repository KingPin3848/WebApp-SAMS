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
    public class StudentLocationController(ApplicationDbContext context) : Controller
    {
        private readonly ApplicationDbContext _context = context;

        // GET: StudentLocation
        public async Task<IActionResult> Index()
        {
            return View(await _context.StudentLocationModels.ToListAsync());
        }

        // GET: StudentLocation/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentLocationModel = await _context.StudentLocationModels
                .FirstOrDefaultAsync(m => m.StudentId == id);
            if (studentLocationModel == null)
            {
                return NotFound();
            }

            return View(studentLocationModel);
        }

        // GET: StudentLocation/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: StudentLocation/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StudentId,StudentName,StudentLocation")] StudentLocationModel studentLocationModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(studentLocationModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(studentLocationModel);
        }

        // GET: StudentLocation/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentLocationModel = await _context.StudentLocationModels.FindAsync(id);
            if (studentLocationModel == null)
            {
                return NotFound();
            }
            return View(studentLocationModel);
        }

        // POST: StudentLocation/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StudentId,StudentName,StudentLocation")] StudentLocationModel studentLocationModel)
        {
            if (id != studentLocationModel.StudentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(studentLocationModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentLocationModelExists(studentLocationModel.StudentId))
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
            return View(studentLocationModel);
        }

        // GET: StudentLocation/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentLocationModel = await _context.StudentLocationModels
                .FirstOrDefaultAsync(m => m.StudentId == id);
            if (studentLocationModel == null)
            {
                return NotFound();
            }

            return View(studentLocationModel);
        }

        // POST: StudentLocation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var studentLocationModel = await _context.StudentLocationModels.FindAsync(id);
            if (studentLocationModel != null)
            {
                _context.StudentLocationModels.Remove(studentLocationModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentLocationModelExists(int id)
        {
            return _context.StudentLocationModels.Any(e => e.StudentId == id);
        }
    }
}
