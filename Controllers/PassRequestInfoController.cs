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
    public class PassRequestInfoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PassRequestInfoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PassRequestInfo
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.passRequestInfoModels.Include(p => p.AddressedByAdmin).Include(p => p.AddressedByAttendanceOfficeMember).Include(p => p.AddressedByCounselor).Include(p => p.AddressedByLawEnf).Include(p => p.AddressedByNurse).Include(p => p.AddressedByTeacher).Include(p => p.AssignedByAdmin).Include(p => p.AssignedByAttendanceOfficeMember).Include(p => p.AssignedByCounselor).Include(p => p.AssignedByLawEnf).Include(p => p.AssignedByNurse).Include(p => p.AssignedByTeacher).Include(p => p.Student);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: PassRequestInfo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var passRequestInfoModel = await _context.passRequestInfoModels
                .Include(p => p.AddressedByAdmin)
                .Include(p => p.AddressedByAttendanceOfficeMember)
                .Include(p => p.AddressedByCounselor)
                .Include(p => p.AddressedByLawEnf)
                .Include(p => p.AddressedByNurse)
                .Include(p => p.AddressedByTeacher)
                .Include(p => p.AssignedByAdmin)
                .Include(p => p.AssignedByAttendanceOfficeMember)
                .Include(p => p.AssignedByCounselor)
                .Include(p => p.AssignedByLawEnf)
                .Include(p => p.AssignedByNurse)
                .Include(p => p.AssignedByTeacher)
                .Include(p => p.Student)
                .FirstOrDefaultAsync(m => m.PassRequestId == id);
            if (passRequestInfoModel == null)
            {
                return NotFound();
            }

            return View(passRequestInfoModel);
        }

        // GET: PassRequestInfo/Create
        public IActionResult Create()
        {
            ViewData["HallPassAddressedBy"] = new SelectList(_context.adminInfoModels, "AdminID", "AdminID");
            ViewData["HallPassAddressedBy"] = new SelectList(_context.attendanceOfficeMemberModels, "AoMemberID", "AoMemberID");
            ViewData["HallPassAddressedBy"] = new SelectList(_context.counselorModels, "CounselorId", "CounselorId");
            ViewData["HallPassAddressedBy"] = new SelectList(_context.lawEnforcementInfoModels, "LawenfID", "LawenfID");
            ViewData["HallPassAddressedBy"] = new SelectList(_context.nurseInfoModels, "NurseID", "NurseID");
            ViewData["HallPassAddressedBy"] = new SelectList(_context.teacherInfoModels, "TeacherID", "TeacherID");
            ViewData["HallPassAssignedBy"] = new SelectList(_context.adminInfoModels, "AdminID", "AdminID");
            ViewData["HallPassAssignedBy"] = new SelectList(_context.attendanceOfficeMemberModels, "AoMemberID", "AoMemberID");
            ViewData["HallPassAssignedBy"] = new SelectList(_context.counselorModels, "CounselorId", "CounselorId");
            ViewData["HallPassAssignedBy"] = new SelectList(_context.lawEnforcementInfoModels, "LawenfID", "LawenfID");
            ViewData["HallPassAssignedBy"] = new SelectList(_context.nurseInfoModels, "NurseID", "NurseID");
            ViewData["HallPassAssignedBy"] = new SelectList(_context.teacherInfoModels, "TeacherID", "TeacherID");
            ViewData["StudentID"] = new SelectList(_context.studentInfoModels, "StudentID", "StudentID");
            return View();
        }

        // POST: PassRequestInfo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PassRequestId,StudentID,StudentNameMod,HallPassAssignedBy,HallPassAddressedBy,StartDateTime,EndDateTime,Duration,BellNumber,StartLocation,EndLocation,RequestStatus")] PassRequestInfoModel passRequestInfoModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(passRequestInfoModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["HallPassAddressedBy"] = new SelectList(_context.adminInfoModels, "AdminID", "AdminID", passRequestInfoModel.HallPassAddressedBy);
            ViewData["HallPassAddressedBy"] = new SelectList(_context.attendanceOfficeMemberModels, "AoMemberID", "AoMemberID", passRequestInfoModel.HallPassAddressedBy);
            ViewData["HallPassAddressedBy"] = new SelectList(_context.counselorModels, "CounselorId", "CounselorId", passRequestInfoModel.HallPassAddressedBy);
            ViewData["HallPassAddressedBy"] = new SelectList(_context.lawEnforcementInfoModels, "LawenfID", "LawenfID", passRequestInfoModel.HallPassAddressedBy);
            ViewData["HallPassAddressedBy"] = new SelectList(_context.nurseInfoModels, "NurseID", "NurseID", passRequestInfoModel.HallPassAddressedBy);
            ViewData["HallPassAddressedBy"] = new SelectList(_context.teacherInfoModels, "TeacherID", "TeacherID", passRequestInfoModel.HallPassAddressedBy);
            ViewData["HallPassAssignedBy"] = new SelectList(_context.adminInfoModels, "AdminID", "AdminID", passRequestInfoModel.HallPassAssignedBy);
            ViewData["HallPassAssignedBy"] = new SelectList(_context.attendanceOfficeMemberModels, "AoMemberID", "AoMemberID", passRequestInfoModel.HallPassAssignedBy);
            ViewData["HallPassAssignedBy"] = new SelectList(_context.counselorModels, "CounselorId", "CounselorId", passRequestInfoModel.HallPassAssignedBy);
            ViewData["HallPassAssignedBy"] = new SelectList(_context.lawEnforcementInfoModels, "LawenfID", "LawenfID", passRequestInfoModel.HallPassAssignedBy);
            ViewData["HallPassAssignedBy"] = new SelectList(_context.nurseInfoModels, "NurseID", "NurseID", passRequestInfoModel.HallPassAssignedBy);
            ViewData["HallPassAssignedBy"] = new SelectList(_context.teacherInfoModels, "TeacherID", "TeacherID", passRequestInfoModel.HallPassAssignedBy);
            ViewData["StudentID"] = new SelectList(_context.studentInfoModels, "StudentID", "StudentID", passRequestInfoModel.StudentID);
            return View(passRequestInfoModel);
        }

        // GET: PassRequestInfo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var passRequestInfoModel = await _context.passRequestInfoModels.FindAsync(id);
            if (passRequestInfoModel == null)
            {
                return NotFound();
            }
            ViewData["HallPassAddressedBy"] = new SelectList(_context.adminInfoModels, "AdminID", "AdminID", passRequestInfoModel.HallPassAddressedBy);
            ViewData["HallPassAddressedBy"] = new SelectList(_context.attendanceOfficeMemberModels, "AoMemberID", "AoMemberID", passRequestInfoModel.HallPassAddressedBy);
            ViewData["HallPassAddressedBy"] = new SelectList(_context.counselorModels, "CounselorId", "CounselorId", passRequestInfoModel.HallPassAddressedBy);
            ViewData["HallPassAddressedBy"] = new SelectList(_context.lawEnforcementInfoModels, "LawenfID", "LawenfID", passRequestInfoModel.HallPassAddressedBy);
            ViewData["HallPassAddressedBy"] = new SelectList(_context.nurseInfoModels, "NurseID", "NurseID", passRequestInfoModel.HallPassAddressedBy);
            ViewData["HallPassAddressedBy"] = new SelectList(_context.teacherInfoModels, "TeacherID", "TeacherID", passRequestInfoModel.HallPassAddressedBy);
            ViewData["HallPassAssignedBy"] = new SelectList(_context.adminInfoModels, "AdminID", "AdminID", passRequestInfoModel.HallPassAssignedBy);
            ViewData["HallPassAssignedBy"] = new SelectList(_context.attendanceOfficeMemberModels, "AoMemberID", "AoMemberID", passRequestInfoModel.HallPassAssignedBy);
            ViewData["HallPassAssignedBy"] = new SelectList(_context.counselorModels, "CounselorId", "CounselorId", passRequestInfoModel.HallPassAssignedBy);
            ViewData["HallPassAssignedBy"] = new SelectList(_context.lawEnforcementInfoModels, "LawenfID", "LawenfID", passRequestInfoModel.HallPassAssignedBy);
            ViewData["HallPassAssignedBy"] = new SelectList(_context.nurseInfoModels, "NurseID", "NurseID", passRequestInfoModel.HallPassAssignedBy);
            ViewData["HallPassAssignedBy"] = new SelectList(_context.teacherInfoModels, "TeacherID", "TeacherID", passRequestInfoModel.HallPassAssignedBy);
            ViewData["StudentID"] = new SelectList(_context.studentInfoModels, "StudentID", "StudentID", passRequestInfoModel.StudentID);
            return View(passRequestInfoModel);
        }

        // POST: PassRequestInfo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PassRequestId,StudentID,StudentNameMod,HallPassAssignedBy,HallPassAddressedBy,StartDateTime,EndDateTime,Duration,BellNumber,StartLocation,EndLocation,RequestStatus")] PassRequestInfoModel passRequestInfoModel)
        {
            if (id != passRequestInfoModel.PassRequestId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(passRequestInfoModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PassRequestInfoModelExists(passRequestInfoModel.PassRequestId))
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
            ViewData["HallPassAddressedBy"] = new SelectList(_context.adminInfoModels, "AdminID", "AdminID", passRequestInfoModel.HallPassAddressedBy);
            ViewData["HallPassAddressedBy"] = new SelectList(_context.attendanceOfficeMemberModels, "AoMemberID", "AoMemberID", passRequestInfoModel.HallPassAddressedBy);
            ViewData["HallPassAddressedBy"] = new SelectList(_context.counselorModels, "CounselorId", "CounselorId", passRequestInfoModel.HallPassAddressedBy);
            ViewData["HallPassAddressedBy"] = new SelectList(_context.lawEnforcementInfoModels, "LawenfID", "LawenfID", passRequestInfoModel.HallPassAddressedBy);
            ViewData["HallPassAddressedBy"] = new SelectList(_context.nurseInfoModels, "NurseID", "NurseID", passRequestInfoModel.HallPassAddressedBy);
            ViewData["HallPassAddressedBy"] = new SelectList(_context.teacherInfoModels, "TeacherID", "TeacherID", passRequestInfoModel.HallPassAddressedBy);
            ViewData["HallPassAssignedBy"] = new SelectList(_context.adminInfoModels, "AdminID", "AdminID", passRequestInfoModel.HallPassAssignedBy);
            ViewData["HallPassAssignedBy"] = new SelectList(_context.attendanceOfficeMemberModels, "AoMemberID", "AoMemberID", passRequestInfoModel.HallPassAssignedBy);
            ViewData["HallPassAssignedBy"] = new SelectList(_context.counselorModels, "CounselorId", "CounselorId", passRequestInfoModel.HallPassAssignedBy);
            ViewData["HallPassAssignedBy"] = new SelectList(_context.lawEnforcementInfoModels, "LawenfID", "LawenfID", passRequestInfoModel.HallPassAssignedBy);
            ViewData["HallPassAssignedBy"] = new SelectList(_context.nurseInfoModels, "NurseID", "NurseID", passRequestInfoModel.HallPassAssignedBy);
            ViewData["HallPassAssignedBy"] = new SelectList(_context.teacherInfoModels, "TeacherID", "TeacherID", passRequestInfoModel.HallPassAssignedBy);
            ViewData["StudentID"] = new SelectList(_context.studentInfoModels, "StudentID", "StudentID", passRequestInfoModel.StudentID);
            return View(passRequestInfoModel);
        }

        // GET: PassRequestInfo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var passRequestInfoModel = await _context.passRequestInfoModels
                .Include(p => p.AddressedByAdmin)
                .Include(p => p.AddressedByAttendanceOfficeMember)
                .Include(p => p.AddressedByCounselor)
                .Include(p => p.AddressedByLawEnf)
                .Include(p => p.AddressedByNurse)
                .Include(p => p.AddressedByTeacher)
                .Include(p => p.AssignedByAdmin)
                .Include(p => p.AssignedByAttendanceOfficeMember)
                .Include(p => p.AssignedByCounselor)
                .Include(p => p.AssignedByLawEnf)
                .Include(p => p.AssignedByNurse)
                .Include(p => p.AssignedByTeacher)
                .Include(p => p.Student)
                .FirstOrDefaultAsync(m => m.PassRequestId == id);
            if (passRequestInfoModel == null)
            {
                return NotFound();
            }

            return View(passRequestInfoModel);
        }

        // POST: PassRequestInfo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var passRequestInfoModel = await _context.passRequestInfoModels.FindAsync(id);
            if (passRequestInfoModel != null)
            {
                _context.passRequestInfoModels.Remove(passRequestInfoModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PassRequestInfoModelExists(int id)
        {
            return _context.passRequestInfoModels.Any(e => e.PassRequestId == id);
        }
    }
}
