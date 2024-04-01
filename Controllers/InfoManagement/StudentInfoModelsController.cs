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
    public class StudentInfoModelsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentInfoModelsController(ApplicationDbContext context)
        {
            _context = context;
        }
        

        //AUTOCOMPLETE
        public JsonResult Autocomplete(string prefix)
        {
            var students = (from student in _context.studentInfoModels
                            where student.StudentFirstNameMod.StartsWith(prefix)
                            select new
                            {
                                label = student.StudentFirstNameMod,
                                val = student.StudentID
                            }).ToList();
            return Json(students);
        }


        // GET: StudentInfoModels
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.studentInfoModels.Include(s => s.Counselor).Include(s => s.BellAttendances).Include(s => s.DailyAttendances).Include(s => s.Sem1StudSchedule).Include(s => s.Sem2StudSchedule).Include(s => s.HallPasses);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: StudentInfoModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentInfoModel = await _context.studentInfoModels
                .Include(s => s.AssignedEASuport)
                .Include(s => s.Counselor)
                .FirstOrDefaultAsync(m => m.StudentID == id);
            if (studentInfoModel == null)
            {
                return NotFound();
            }

            return View(studentInfoModel);
        }

        // GET: StudentInfoModels/Create
        public IActionResult Create()
        {
            ViewData["StudentEAID"] = new SelectList(_context.eASuportInfoModels, "EaID", "EaID");
            ViewData["StudentCounselorID"] = new SelectList(_context.counselorModels, "CounselorId", "CounselorId");
            return View();
        }

        // POST: StudentInfoModels/Create
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
            ViewData["StudentEAID"] = new SelectList(_context.eASuportInfoModels, "EaID", "EaID", studentInfoModel.StudentEAID);
            ViewData["StudentCounselorID"] = new SelectList(_context.counselorModels, "CounselorId", "CounselorId", studentInfoModel.StudentCounselorID);
            return View(studentInfoModel);
        }

        // GET: StudentInfoModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentInfoModel = await _context.studentInfoModels.FindAsync(id);
            if (studentInfoModel == null)
            {
                return NotFound();
            }
            ViewData["StudentEAID"] = new SelectList(_context.eASuportInfoModels, "EaID", "EaID", studentInfoModel.StudentEAID);
            ViewData["StudentCounselorID"] = new SelectList(_context.counselorModels, "CounselorId", "CounselorId", studentInfoModel.StudentCounselorID);
            return View(studentInfoModel);
        }

        // POST: StudentInfoModels/Edit/5
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
            ViewData["StudentEAID"] = new SelectList(_context.eASuportInfoModels, "EaID", "EaID", studentInfoModel.StudentEAID);
            ViewData["StudentCounselorID"] = new SelectList(_context.counselorModels, "CounselorId", "CounselorId", studentInfoModel.StudentCounselorID);
            return View(studentInfoModel);
        }

        // GET: StudentInfoModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentInfoModel = await _context.studentInfoModels
                .Include(s => s.AssignedEASuport)
                .Include(s => s.Counselor)
                .FirstOrDefaultAsync(m => m.StudentID == id);
            if (studentInfoModel == null)
            {
                return NotFound();
            }

            return View(studentInfoModel);
        }

        // POST: StudentInfoModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var studentInfoModel = await _context.studentInfoModels.FindAsync(id);
            if (studentInfoModel != null)
            {
                _context.studentInfoModels.Remove(studentInfoModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentInfoModelExists(int id)
        {
            return _context.studentInfoModels.Any(e => e.StudentID == id);
        }
    }
}
