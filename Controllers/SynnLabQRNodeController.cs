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
    public class SynnLabQRNodeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SynnLabQRNodeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SynnLabQRNode
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.synnLabQRNodeModels.Include(s => s.Room);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: SynnLabQRNode/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.synnLabQRNodeModels == null)
            {
                return NotFound();
            }

            var synnLabQRNodeModel = await _context.synnLabQRNodeModels
                .Include(s => s.Room)
                .FirstOrDefaultAsync(m => m.ScannerID == id);
            if (synnLabQRNodeModel == null)
            {
                return NotFound();
            }

            return View(synnLabQRNodeModel);
        }

        // GET: SynnLabQRNode/Create
        public IActionResult Create()
        {
            ViewData["SynnlabRoomIDMod"] = new SelectList(_context.roomLocationInfoModels, "RoomId", "RoomId");
            return View();
        }

        // POST: SynnLabQRNode/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ScannerID,SynnlabRoomIDMod,ScannerMacAddressMod,ModelNumberMod,ScannerDeviceIPAddressMod,ScannerLabelMod")] SynnLabQRNodeModel synnLabQRNodeModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(synnLabQRNodeModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SynnlabRoomIDMod"] = new SelectList(_context.roomLocationInfoModels, "RoomId", "RoomId", synnLabQRNodeModel.SynnlabRoomIDMod);
            return View(synnLabQRNodeModel);
        }

        // GET: SynnLabQRNode/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.synnLabQRNodeModels == null)
            {
                return NotFound();
            }

            var synnLabQRNodeModel = await _context.synnLabQRNodeModels.FindAsync(id);
            if (synnLabQRNodeModel == null)
            {
                return NotFound();
            }
            ViewData["SynnlabRoomIDMod"] = new SelectList(_context.roomLocationInfoModels, "RoomId", "RoomId", synnLabQRNodeModel.SynnlabRoomIDMod);
            return View(synnLabQRNodeModel);
        }

        // POST: SynnLabQRNode/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ScannerID,SynnlabRoomIDMod,ScannerMacAddressMod,ModelNumberMod,ScannerDeviceIPAddressMod,ScannerLabelMod")] SynnLabQRNodeModel synnLabQRNodeModel)
        {
            if (id != synnLabQRNodeModel.ScannerID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(synnLabQRNodeModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SynnLabQRNodeModelExists(synnLabQRNodeModel.ScannerID))
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
            ViewData["SynnlabRoomIDMod"] = new SelectList(_context.roomLocationInfoModels, "RoomId", "RoomId", synnLabQRNodeModel.SynnlabRoomIDMod);
            return View(synnLabQRNodeModel);
        }

        // GET: SynnLabQRNode/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.synnLabQRNodeModels == null)
            {
                return NotFound();
            }

            var synnLabQRNodeModel = await _context.synnLabQRNodeModels
                .Include(s => s.Room)
                .FirstOrDefaultAsync(m => m.ScannerID == id);
            if (synnLabQRNodeModel == null)
            {
                return NotFound();
            }

            return View(synnLabQRNodeModel);
        }

        // POST: SynnLabQRNode/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.synnLabQRNodeModels == null)
            {
                return Problem("Entity set 'ApplicationDbContext.synnLabQRNodeModels'  is null.");
            }
            var synnLabQRNodeModel = await _context.synnLabQRNodeModels.FindAsync(id);
            if (synnLabQRNodeModel != null)
            {
                _context.synnLabQRNodeModels.Remove(synnLabQRNodeModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SynnLabQRNodeModelExists(string id)
        {
          return (_context.synnLabQRNodeModels?.Any(e => e.ScannerID == id)).GetValueOrDefault();
        }
    }
}
