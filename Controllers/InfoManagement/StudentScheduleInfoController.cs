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
    public class StudentScheduleInfoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentScheduleInfoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: StudentScheduleInfo
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.studentScheduleInfoModels.Include(s => s.Student);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: StudentScheduleInfo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentScheduleInfoModel = await _context.studentScheduleInfoModels
                .Include(s => s.Student)
                .FirstOrDefaultAsync(m => m.StudentID == id);
            if (studentScheduleInfoModel == null)
            {
                return NotFound();
            }

            return View(studentScheduleInfoModel);
        }

        // GET: StudentScheduleInfo/Create
        public IActionResult Create()
        {
            ViewData["StudentID"] = new SelectList(_context.studentInfoModels, "StudentID", "StudentID");
            return View();
        }

        // POST: StudentScheduleInfo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StudentID,Bell1EnrollmentCodeMod,Bell2EnrollmentCodeMod,Bell3EnrollmentCodeMod,Bell4EnrollmentCodeMod,Bell5EnrollmentCodeMod,Bell6EnrollmentCodeMod,Bell7EnrollmentCodeMod,AvesBellRoomCodeMod,LunchCodeMod")] StudentScheduleInfoModel studentScheduleInfoModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(studentScheduleInfoModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["StudentID"] = new SelectList(_context.studentInfoModels, "StudentID", "StudentID", studentScheduleInfoModel.StudentID);
            return View(studentScheduleInfoModel);
        }

        // GET: StudentScheduleInfo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentScheduleInfoModel = await _context.studentScheduleInfoModels.FindAsync(id);
            if (studentScheduleInfoModel == null)
            {
                return NotFound();
            }
            ViewData["StudentID"] = new SelectList(_context.studentInfoModels, "StudentID", "StudentID", studentScheduleInfoModel.StudentID);
            return View(studentScheduleInfoModel);
        }

        // POST: StudentScheduleInfo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StudentID,Bell1EnrollmentCodeMod,Bell2EnrollmentCodeMod,Bell3EnrollmentCodeMod,Bell4EnrollmentCodeMod,Bell5EnrollmentCodeMod,Bell6EnrollmentCodeMod,Bell7EnrollmentCodeMod,AvesBellRoomCodeMod,LunchCodeMod")] StudentScheduleInfoModel studentScheduleInfoModel)
        {
            if (id != studentScheduleInfoModel.StudentID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(studentScheduleInfoModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentScheduleInfoModelExists(studentScheduleInfoModel.StudentID))
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
            ViewData["StudentID"] = new SelectList(_context.studentInfoModels, "StudentID", "StudentID", studentScheduleInfoModel.StudentID);
            return View(studentScheduleInfoModel);
        }

        // GET: StudentScheduleInfo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentScheduleInfoModel = await _context.studentScheduleInfoModels
                .Include(s => s.Student)
                .FirstOrDefaultAsync(m => m.StudentID == id);
            if (studentScheduleInfoModel == null)
            {
                return NotFound();
            }

            return View(studentScheduleInfoModel);
        }

        // POST: StudentScheduleInfo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var studentScheduleInfoModel = await _context.studentScheduleInfoModels.FindAsync(id);
            if (studentScheduleInfoModel != null)
            {
                _context.studentScheduleInfoModels.Remove(studentScheduleInfoModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentScheduleInfoModelExists(int id)
        {
            return _context.studentScheduleInfoModels.Any(e => e.StudentID == id);
        }
    }
}
