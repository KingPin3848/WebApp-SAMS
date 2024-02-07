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
    public class AttendanceOfficeMemberController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AttendanceOfficeMemberController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AttendanceOfficeMember
        public async Task<IActionResult> Index()
        {
            return View("~/Views/InfoManagement/AttendanceOfficeMember/Index.cshtml", await _context.attendanceOfficeMemberModels.ToListAsync());
        }

        // GET: AttendanceOfficeMember/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attendanceOfficeMemberModel = await _context.attendanceOfficeMemberModels
                .FirstOrDefaultAsync(m => m.AoMemberID == id);
            if (attendanceOfficeMemberModel == null)
            {
                return NotFound();
            }

            return View("~/Views/InfoManagement/AttendanceOfficeMember/Details.cshtml", attendanceOfficeMemberModel);
        }

        // GET: AttendanceOfficeMember/Create
        public IActionResult Create()
        {
            return View("~/Views/InfoManagement/AttendanceOfficeMember/Create.cshtml");
        }

        // POST: AttendanceOfficeMember/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AoMemberID,AoMemberFirstNameMod,AoMemberMiddleNameMod,AoMemberLastNameMod,AoMemberPreferredNameMod,AoMemberEmailMod,AoMemberPhoneMod")] AttendanceOfficeMemberModel attendanceOfficeMemberModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(attendanceOfficeMemberModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View("~/Views/InfoManagement/AttendanceOfficeMember/Create.cshtml", attendanceOfficeMemberModel);
        }

        // GET: AttendanceOfficeMember/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attendanceOfficeMemberModel = await _context.attendanceOfficeMemberModels.FindAsync(id);
            if (attendanceOfficeMemberModel == null)
            {
                return NotFound();
            }
            return View("~/Views/InfoManagement/AttendanceOfficeMember/Edit.cshtml", attendanceOfficeMemberModel);
        }

        // POST: AttendanceOfficeMember/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("AoMemberID,AoMemberFirstNameMod,AoMemberMiddleNameMod,AoMemberLastNameMod,AoMemberPreferredNameMod,AoMemberEmailMod,AoMemberPhoneMod")] AttendanceOfficeMemberModel attendanceOfficeMemberModel)
        {
            if (id != attendanceOfficeMemberModel.AoMemberID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(attendanceOfficeMemberModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AttendanceOfficeMemberModelExists(attendanceOfficeMemberModel.AoMemberID))
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
            return View("~/Views/InfoManagement/AttendanceOfficeMember/Edit.cshtml", attendanceOfficeMemberModel);
        }

        // GET: AttendanceOfficeMember/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attendanceOfficeMemberModel = await _context.attendanceOfficeMemberModels
                .FirstOrDefaultAsync(m => m.AoMemberID == id);
            if (attendanceOfficeMemberModel == null)
            {
                return NotFound();
            }

            return View("~/Views/InfoManagement/AttendanceOfficeMember/Delete.cshtml", attendanceOfficeMemberModel);
        }

        // POST: AttendanceOfficeMember/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var attendanceOfficeMemberModel = await _context.attendanceOfficeMemberModels.FindAsync(id);
            if (attendanceOfficeMemberModel != null)
            {
                _context.attendanceOfficeMemberModels.Remove(attendanceOfficeMemberModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AttendanceOfficeMemberModelExists(string id)
        {
            return _context.attendanceOfficeMemberModels.Any(e => e.AoMemberID == id);
        }
    }
}
