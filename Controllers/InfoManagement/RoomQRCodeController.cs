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
    public class RoomQRCodeController(ApplicationDbContext context) : Controller
    {
        private readonly ApplicationDbContext _context = context;

        // GET: RoomQRCode
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.RoomQRCodeModels.Include(r => r.Room);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: RoomQRCode/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roomQRCodeModel = await _context.RoomQRCodeModels
                .Include(r => r.Room)
                .FirstOrDefaultAsync(m => m.RoomId == id);
            if (roomQRCodeModel == null)
            {
                return NotFound();
            }

            return View(roomQRCodeModel);
        }

        // GET: RoomQRCode/Create
        public IActionResult Create()
        {
            ViewData["RoomId"] = new SelectList(_context.RoomLocationInfoModels, "RoomNumberMod", "RoomNumberMod");
            return View();
        }

        // POST: RoomQRCode/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RoomId,Code")] RoomQRCodeModel roomQRCodeModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(roomQRCodeModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoomId"] = new SelectList(_context.RoomLocationInfoModels, "RoomNumberMod", "RoomNumberMod", roomQRCodeModel.RoomId);
            return View(roomQRCodeModel);
        }

        // GET: RoomQRCode/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roomQRCodeModel = await _context.RoomQRCodeModels.FindAsync(id);
            if (roomQRCodeModel == null)
            {
                return NotFound();
            }
            ViewData["RoomId"] = new SelectList(_context.RoomLocationInfoModels, "RoomNumberMod", "RoomNumberMod", roomQRCodeModel.RoomId);
            return View(roomQRCodeModel);
        }

        // POST: RoomQRCode/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RoomId,Code")] RoomQRCodeModel roomQRCodeModel)
        {
            if (id != roomQRCodeModel.RoomId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(roomQRCodeModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomQRCodeModelExists(roomQRCodeModel.RoomId))
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
            ViewData["RoomId"] = new SelectList(_context.RoomLocationInfoModels, "RoomNumberMod", "RoomNumberMod", roomQRCodeModel.RoomId);
            return View(roomQRCodeModel);
        }

        // GET: RoomQRCode/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roomQRCodeModel = await _context.RoomQRCodeModels
                .Include(r => r.Room)
                .FirstOrDefaultAsync(m => m.RoomId == id);
            if (roomQRCodeModel == null)
            {
                return NotFound();
            }

            return View(roomQRCodeModel);
        }

        // POST: RoomQRCode/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var roomQRCodeModel = await _context.RoomQRCodeModels.FindAsync(id);
            if (roomQRCodeModel != null)
            {
                _context.RoomQRCodeModels.Remove(roomQRCodeModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RoomQRCodeModelExists(int id)
        {
            return _context.RoomQRCodeModels.Any(e => e.RoomId == id);
        }
    }
}
