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
    public class HallPassInfoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HallPassInfoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: HallPassInfo
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.hallPassInfoModels.Include(h => h.AddressedByAdmin).Include(h => h.AddressedByAttendanceOfficeMember).Include(h => h.AddressedByCounselor).Include(h => h.AddressedByLawEnf).Include(h => h.AddressedByNurse).Include(h => h.AddressedByTeacher).Include(h => h.AssignedByAdmin).Include(h => h.AssignedByAttendanceOfficeMember).Include(h => h.AssignedByCounselor).Include(h => h.AssignedByLawEnf).Include(h => h.AssignedByNurse).Include(h => h.AssignedByTeacher).Include(h => h.Student);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: HallPassInfo/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.hallPassInfoModels == null)
            {
                return NotFound();
            }

            var hallPassInfoModel = await _context.hallPassInfoModels
                .Include(h => h.AddressedByAdmin)
                .Include(h => h.AddressedByAttendanceOfficeMember)
                .Include(h => h.AddressedByCounselor)
                .Include(h => h.AddressedByLawEnf)
                .Include(h => h.AddressedByNurse)
                .Include(h => h.AddressedByTeacher)
                .Include(h => h.AssignedByAdmin)
                .Include(h => h.AssignedByAttendanceOfficeMember)
                .Include(h => h.AssignedByCounselor)
                .Include(h => h.AssignedByLawEnf)
                .Include(h => h.AssignedByNurse)
                .Include(h => h.AssignedByTeacher)
                .Include(h => h.Student)
                .FirstOrDefaultAsync(m => m.HallPassID == id);
            if (hallPassInfoModel == null)
            {
                return NotFound();
            }

            return View(hallPassInfoModel);
        }

        // GET: HallPassInfo/Create
        public IActionResult Create()
        {
            ViewData["HallPassAddressedByID"] = new SelectList(_context.adminInfoModels, "AdminID", "AdminID");
            ViewData["HallPassAddressedByID"] = new SelectList(_context.attendanceOfficeMemberModels, "AoMemberID", "AoMemberID");
            ViewData["HallPassAddressedByID"] = new SelectList(_context.counselorModels, "CounselorId", "CounselorId");
            ViewData["HallPassAddressedByID"] = new SelectList(_context.lawEnforcementInfoModels, "LawenfID", "LawenfID");
            ViewData["HallPassAddressedByID"] = new SelectList(_context.nurseInfoModels, "NurseID", "NurseID");
            ViewData["HallPassAddressedByID"] = new SelectList(_context.teacherInfoModels, "TeacherID", "TeacherID");
            ViewData["HallPassAssignedByID"] = new SelectList(_context.adminInfoModels, "AdminID", "AdminID");
            ViewData["HallPassAssignedByID"] = new SelectList(_context.attendanceOfficeMemberModels, "AoMemberID", "AoMemberID");
            ViewData["HallPassAssignedByID"] = new SelectList(_context.counselorModels, "CounselorId", "CounselorId");
            ViewData["HallPassAssignedByID"] = new SelectList(_context.lawEnforcementInfoModels, "LawenfID", "LawenfID");
            ViewData["HallPassAssignedByID"] = new SelectList(_context.nurseInfoModels, "NurseID", "NurseID");
            ViewData["HallPassAssignedByID"] = new SelectList(_context.teacherInfoModels, "TeacherID", "TeacherID");
            ViewData["StudentID"] = new SelectList(_context.studentInfoModels, "StudentID", "StudentID");
            return View();
        }

        // POST: HallPassInfo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StudentID,HallPassID,StartDateTime,EndDateTime,Duration,BellNumber,HallPassAssignedByID,HallPassAddressedByID,StartLocation,EndLocation")] HallPassInfoModel hallPassInfoModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hallPassInfoModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["HallPassAddressedByID"] = new SelectList(_context.adminInfoModels, "AdminID", "AdminID", hallPassInfoModel.HallPassAddressedByID);
            ViewData["HallPassAddressedByID"] = new SelectList(_context.attendanceOfficeMemberModels, "AoMemberID", "AoMemberID", hallPassInfoModel.HallPassAddressedByID);
            ViewData["HallPassAddressedByID"] = new SelectList(_context.counselorModels, "CounselorId", "CounselorId", hallPassInfoModel.HallPassAddressedByID);
            ViewData["HallPassAddressedByID"] = new SelectList(_context.lawEnforcementInfoModels, "LawenfID", "LawenfID", hallPassInfoModel.HallPassAddressedByID);
            ViewData["HallPassAddressedByID"] = new SelectList(_context.nurseInfoModels, "NurseID", "NurseID", hallPassInfoModel.HallPassAddressedByID);
            ViewData["HallPassAddressedByID"] = new SelectList(_context.teacherInfoModels, "TeacherID", "TeacherID", hallPassInfoModel.HallPassAddressedByID);
            ViewData["HallPassAssignedByID"] = new SelectList(_context.adminInfoModels, "AdminID", "AdminID", hallPassInfoModel.HallPassAssignedByID);
            ViewData["HallPassAssignedByID"] = new SelectList(_context.attendanceOfficeMemberModels, "AoMemberID", "AoMemberID", hallPassInfoModel.HallPassAssignedByID);
            ViewData["HallPassAssignedByID"] = new SelectList(_context.counselorModels, "CounselorId", "CounselorId", hallPassInfoModel.HallPassAssignedByID);
            ViewData["HallPassAssignedByID"] = new SelectList(_context.lawEnforcementInfoModels, "LawenfID", "LawenfID", hallPassInfoModel.HallPassAssignedByID);
            ViewData["HallPassAssignedByID"] = new SelectList(_context.nurseInfoModels, "NurseID", "NurseID", hallPassInfoModel.HallPassAssignedByID);
            ViewData["HallPassAssignedByID"] = new SelectList(_context.teacherInfoModels, "TeacherID", "TeacherID", hallPassInfoModel.HallPassAssignedByID);
            ViewData["StudentID"] = new SelectList(_context.studentInfoModels, "StudentID", "StudentID", hallPassInfoModel.StudentID);
            return View(hallPassInfoModel);
        }

        // GET: HallPassInfo/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.hallPassInfoModels == null)
            {
                return NotFound();
            }

            var hallPassInfoModel = await _context.hallPassInfoModels.FindAsync(id);
            if (hallPassInfoModel == null)
            {
                return NotFound();
            }
            ViewData["HallPassAddressedByID"] = new SelectList(_context.adminInfoModels, "AdminID", "AdminID", hallPassInfoModel.HallPassAddressedByID);
            ViewData["HallPassAddressedByID"] = new SelectList(_context.attendanceOfficeMemberModels, "AoMemberID", "AoMemberID", hallPassInfoModel.HallPassAddressedByID);
            ViewData["HallPassAddressedByID"] = new SelectList(_context.counselorModels, "CounselorId", "CounselorId", hallPassInfoModel.HallPassAddressedByID);
            ViewData["HallPassAddressedByID"] = new SelectList(_context.lawEnforcementInfoModels, "LawenfID", "LawenfID", hallPassInfoModel.HallPassAddressedByID);
            ViewData["HallPassAddressedByID"] = new SelectList(_context.nurseInfoModels, "NurseID", "NurseID", hallPassInfoModel.HallPassAddressedByID);
            ViewData["HallPassAddressedByID"] = new SelectList(_context.teacherInfoModels, "TeacherID", "TeacherID", hallPassInfoModel.HallPassAddressedByID);
            ViewData["HallPassAssignedByID"] = new SelectList(_context.adminInfoModels, "AdminID", "AdminID", hallPassInfoModel.HallPassAssignedByID);
            ViewData["HallPassAssignedByID"] = new SelectList(_context.attendanceOfficeMemberModels, "AoMemberID", "AoMemberID", hallPassInfoModel.HallPassAssignedByID);
            ViewData["HallPassAssignedByID"] = new SelectList(_context.counselorModels, "CounselorId", "CounselorId", hallPassInfoModel.HallPassAssignedByID);
            ViewData["HallPassAssignedByID"] = new SelectList(_context.lawEnforcementInfoModels, "LawenfID", "LawenfID", hallPassInfoModel.HallPassAssignedByID);
            ViewData["HallPassAssignedByID"] = new SelectList(_context.nurseInfoModels, "NurseID", "NurseID", hallPassInfoModel.HallPassAssignedByID);
            ViewData["HallPassAssignedByID"] = new SelectList(_context.teacherInfoModels, "TeacherID", "TeacherID", hallPassInfoModel.HallPassAssignedByID);
            ViewData["StudentID"] = new SelectList(_context.studentInfoModels, "StudentID", "StudentID", hallPassInfoModel.StudentID);
            return View(hallPassInfoModel);
        }

        // POST: HallPassInfo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("StudentID,HallPassID,StartDateTime,EndDateTime,Duration,BellNumber,HallPassAssignedByID,HallPassAddressedByID,StartLocation,EndLocation")] HallPassInfoModel hallPassInfoModel)
        {
            if (id != hallPassInfoModel.HallPassID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hallPassInfoModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HallPassInfoModelExists(hallPassInfoModel.HallPassID))
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
            ViewData["HallPassAddressedByID"] = new SelectList(_context.adminInfoModels, "AdminID", "AdminID", hallPassInfoModel.HallPassAddressedByID);
            ViewData["HallPassAddressedByID"] = new SelectList(_context.attendanceOfficeMemberModels, "AoMemberID", "AoMemberID", hallPassInfoModel.HallPassAddressedByID);
            ViewData["HallPassAddressedByID"] = new SelectList(_context.counselorModels, "CounselorId", "CounselorId", hallPassInfoModel.HallPassAddressedByID);
            ViewData["HallPassAddressedByID"] = new SelectList(_context.lawEnforcementInfoModels, "LawenfID", "LawenfID", hallPassInfoModel.HallPassAddressedByID);
            ViewData["HallPassAddressedByID"] = new SelectList(_context.nurseInfoModels, "NurseID", "NurseID", hallPassInfoModel.HallPassAddressedByID);
            ViewData["HallPassAddressedByID"] = new SelectList(_context.teacherInfoModels, "TeacherID", "TeacherID", hallPassInfoModel.HallPassAddressedByID);
            ViewData["HallPassAssignedByID"] = new SelectList(_context.adminInfoModels, "AdminID", "AdminID", hallPassInfoModel.HallPassAssignedByID);
            ViewData["HallPassAssignedByID"] = new SelectList(_context.attendanceOfficeMemberModels, "AoMemberID", "AoMemberID", hallPassInfoModel.HallPassAssignedByID);
            ViewData["HallPassAssignedByID"] = new SelectList(_context.counselorModels, "CounselorId", "CounselorId", hallPassInfoModel.HallPassAssignedByID);
            ViewData["HallPassAssignedByID"] = new SelectList(_context.lawEnforcementInfoModels, "LawenfID", "LawenfID", hallPassInfoModel.HallPassAssignedByID);
            ViewData["HallPassAssignedByID"] = new SelectList(_context.nurseInfoModels, "NurseID", "NurseID", hallPassInfoModel.HallPassAssignedByID);
            ViewData["HallPassAssignedByID"] = new SelectList(_context.teacherInfoModels, "TeacherID", "TeacherID", hallPassInfoModel.HallPassAssignedByID);
            ViewData["StudentID"] = new SelectList(_context.studentInfoModels, "StudentID", "StudentID", hallPassInfoModel.StudentID);
            return View(hallPassInfoModel);
        }

        // GET: HallPassInfo/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.hallPassInfoModels == null)
            {
                return NotFound();
            }

            var hallPassInfoModel = await _context.hallPassInfoModels
                .Include(h => h.AddressedByAdmin)
                .Include(h => h.AddressedByAttendanceOfficeMember)
                .Include(h => h.AddressedByCounselor)
                .Include(h => h.AddressedByLawEnf)
                .Include(h => h.AddressedByNurse)
                .Include(h => h.AddressedByTeacher)
                .Include(h => h.AssignedByAdmin)
                .Include(h => h.AssignedByAttendanceOfficeMember)
                .Include(h => h.AssignedByCounselor)
                .Include(h => h.AssignedByLawEnf)
                .Include(h => h.AssignedByNurse)
                .Include(h => h.AssignedByTeacher)
                .Include(h => h.Student)
                .FirstOrDefaultAsync(m => m.HallPassID == id);
            if (hallPassInfoModel == null)
            {
                return NotFound();
            }

            return View(hallPassInfoModel);
        }

        // POST: HallPassInfo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.hallPassInfoModels == null)
            {
                return Problem("Entity set 'ApplicationDbContext.hallPassInfoModels'  is null.");
            }
            var hallPassInfoModel = await _context.hallPassInfoModels.FindAsync(id);
            if (hallPassInfoModel != null)
            {
                _context.hallPassInfoModels.Remove(hallPassInfoModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HallPassInfoModelExists(string id)
        {
          return (_context.hallPassInfoModels?.Any(e => e.HallPassID == id)).GetValueOrDefault();
        }
    }
}
