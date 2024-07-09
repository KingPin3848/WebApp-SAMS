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
    public class SynnLabQRNodeController(IServiceScopeFactory serviceScopeFactory) : Controller
    {
        private readonly IServiceScopeFactory _scopeFactory = serviceScopeFactory;

        // GET: SynnLabQRNode
        public async Task<IActionResult> Index()
        {
            using var scope = _scopeFactory.CreateAsyncScope();
            using var _context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var applicationDbContext = _context.HandheldScannerNodeModels.Include(h => h.Room);
            return View(await applicationDbContext.ToListAsync().ConfigureAwait(true));
        }

        // GET: SynnLabQRNode/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            using var scope = _scopeFactory.CreateAsyncScope();
            using var _context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            if (id == null)
            {
                return NotFound();
            }

            var handheldScannerNodeModel = await _context.HandheldScannerNodeModels
                .Include(h => h.Room)
                .FirstOrDefaultAsync(m => m.ScannerID == id).ConfigureAwait(true);
            if (handheldScannerNodeModel == null)
            {
                return NotFound();
            }

            return View(handheldScannerNodeModel);
        }

        // GET: SynnLabQRNode/Create
        public IActionResult Create()
        {
            using var scope = _scopeFactory.CreateAsyncScope();
            using var _context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            ViewData["RoomIDMod"] = new SelectList(_context.RoomLocationInfoModels, "RoomNumberMod", "RoomNumberMod");
            return View();
        }

        // POST: SynnLabQRNode/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ScannerID,RoomIDMod,SerialNumberMod")] HandheldScannerNodeModel handheldScannerNodeModel)
        {
            if (handheldScannerNodeModel == null)
            {
                return NotFound();
            }

            using var scope = _scopeFactory.CreateAsyncScope();
            using var _context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            if (ModelState.IsValid)
            {
                _context.Add(handheldScannerNodeModel);
                await _context.SaveChangesAsync().ConfigureAwait(true);
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoomIDMod"] = new SelectList(_context.RoomLocationInfoModels, "RoomNumberMod", "RoomNumberMod", handheldScannerNodeModel.RoomIDMod);
            return View(handheldScannerNodeModel);
        }

        // GET: SynnLabQRNode/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            using var scope = _scopeFactory.CreateAsyncScope();
            using var _context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var handheldScannerNodeModel = await _context.HandheldScannerNodeModels.FindAsync(id).ConfigureAwait(true);
            if (handheldScannerNodeModel == null)
            {
                return NotFound();
            }
            ViewData["RoomIDMod"] = new SelectList(_context.RoomLocationInfoModels, "RoomNumberMod", "RoomNumberMod", handheldScannerNodeModel.RoomIDMod);
            return View(handheldScannerNodeModel);
        }

        // POST: SynnLabQRNode/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ScannerID,RoomIDMod,SerialNumberMod")] HandheldScannerNodeModel handheldScannerNodeModel)
        {
            if (handheldScannerNodeModel is null)
            {
                return NotFound();
            }

            if (id != handheldScannerNodeModel.ScannerID)
            {
                return NotFound();
            }

            using var scope = _scopeFactory.CreateAsyncScope();
            using var _context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(handheldScannerNodeModel);
                    await _context.SaveChangesAsync().ConfigureAwait(true);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HandheldScannerNodeModelExists(handheldScannerNodeModel.ScannerID))
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
            ViewData["RoomIDMod"] = new SelectList(_context.RoomLocationInfoModels, "RoomNumberMod", "RoomNumberMod", handheldScannerNodeModel.RoomIDMod);
            return View(handheldScannerNodeModel);
        }

        // GET: SynnLabQRNode/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            using var scope = _scopeFactory.CreateAsyncScope();
            using var _context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var handheldScannerNodeModel = await _context.HandheldScannerNodeModels
                .Include(h => h.Room)
                .FirstOrDefaultAsync(m => m.ScannerID == id).ConfigureAwait(true);
            if (handheldScannerNodeModel == null)
            {
                return NotFound();
            }

            return View(handheldScannerNodeModel);
        }

        // POST: SynnLabQRNode/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            using var scope = _scopeFactory.CreateAsyncScope();
            using var _context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var handheldScannerNodeModel = await _context.HandheldScannerNodeModels.FindAsync(id).ConfigureAwait(true);
            if (handheldScannerNodeModel != null)
            {
                _context.HandheldScannerNodeModels.Remove(handheldScannerNodeModel);
            }

            await _context.SaveChangesAsync().ConfigureAwait(true);
            return RedirectToAction(nameof(Index));
        }

        private bool HandheldScannerNodeModelExists(int id)
        {
            using var scope = _scopeFactory.CreateAsyncScope();
            using var _context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            return _context.HandheldScannerNodeModels.Any(e => e.ScannerID == id);
        }
    }
}
