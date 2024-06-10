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
    public class StudentManagementController(ApplicationDbContext context) : Controller
    {
        private readonly ApplicationDbContext _context = context;

        // GET: StudentManagement
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.StudentInfoModels.Include(s => s.AssignedEASuport).Include(s => s.Counselor);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: StudentManagement/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentInfoModel = await _context.StudentInfoModels
                .Include(s => s.AssignedEASuport)
                .Include(s => s.Counselor)
                .FirstOrDefaultAsync(m => m.StudentID == id);
            if (studentInfoModel == null)
            {
                return NotFound();
            }

            return View(studentInfoModel);
        }

        // GET: StudentManagement/Create
        public IActionResult Create()
        {
            ViewData["StudentEAID"] = new SelectList(_context.EASuportInfoModels, "EaID", "EaID");
            ViewData["StudentCounselorID"] = new SelectList(_context.CounselorModels, "CounselorId", "CounselorId");
            return View();
        }

        // POST: StudentManagement/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StudentID,StudentFirstNameMod,StudentMiddleNameMod,StudentLastNameMod,StudentPreferredNameMod,StudentEmailMod,StudentPhoneMod,StudentGradYearMod,StudentCounselorID,HasEASupport,StudentEAID,Parentguard1NameMod,Parentguard1EmailMod,Parentguard2NameMod,Parentguard2EmailMod,ActivationCode")] StudentInfoModel studentInfoModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(studentInfoModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["StudentEAID"] = new SelectList(_context.EASuportInfoModels, "EaID", "EaID", studentInfoModel.StudentEAID);
            ViewData["StudentCounselorID"] = new SelectList(_context.CounselorModels, "CounselorId", "CounselorId", studentInfoModel.StudentCounselorID);
            return View(studentInfoModel);
        }

        // GET: StudentManagement/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentInfoModel = await _context.StudentInfoModels.FindAsync(id);
            if (studentInfoModel == null)
            {
                return NotFound();
            }
            ViewData["StudentEAID"] = new SelectList(_context.EASuportInfoModels, "EaID", "EaID", studentInfoModel.StudentEAID);
            ViewData["StudentCounselorID"] = new SelectList(_context.CounselorModels, "CounselorId", "CounselorId", studentInfoModel.StudentCounselorID);
            return View(studentInfoModel);
        }

        // POST: StudentManagement/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StudentID,StudentFirstNameMod,StudentMiddleNameMod,StudentLastNameMod,StudentPreferredNameMod,StudentEmailMod,StudentPhoneMod,StudentGradYearMod,StudentCounselorID,HasEASupport,StudentEAID,Parentguard1NameMod,Parentguard1EmailMod,Parentguard2NameMod,Parentguard2EmailMod,ActivationCode")] StudentInfoModel studentInfoModel)
        {
            if (id != studentInfoModel.StudentID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(studentInfoModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentInfoModelExists(studentInfoModel.StudentID))
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
            ViewData["StudentEAID"] = new SelectList(_context.EASuportInfoModels, "EaID", "EaID", studentInfoModel.StudentEAID);
            ViewData["StudentCounselorID"] = new SelectList(_context.CounselorModels, "CounselorId", "CounselorId", studentInfoModel.StudentCounselorID);
            return View(studentInfoModel);
        }

        // GET: StudentManagement/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentInfoModel = await _context.StudentInfoModels
                .Include(s => s.AssignedEASuport)
                .Include(s => s.Counselor)
                .FirstOrDefaultAsync(m => m.StudentID == id);
            if (studentInfoModel == null)
            {
                return NotFound();
            }

            return View(studentInfoModel);
        }

        // POST: StudentManagement/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var studentInfoModel = await _context.StudentInfoModels.FindAsync(id);
            if (studentInfoModel != null)
            {
                _context.StudentInfoModels.Remove(studentInfoModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentInfoModelExists(int id)
        {
            return _context.StudentInfoModels.Any(e => e.StudentID == id);
        }
    }
}
