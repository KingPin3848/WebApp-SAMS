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
    public class ChosenBellScheduleController(ApplicationDbContext context) : Controller
    {
        private readonly ApplicationDbContext _context = context;

        // GET: ChosenBellSchedule
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _context.ChosenBellSchedModels.ToListAsync());
        }

        // GET: ChosenBellSchedule/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chosenBellSchedModel = await _context.ChosenBellSchedModels
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chosenBellSchedModel == null)
            {
                return NotFound();
            }

            return View(chosenBellSchedModel);
        }

        // GET: ChosenBellSchedule/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: ChosenBellSchedule/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] ChosenBellSchedModel chosenBellSchedModel)
        {
            if (ModelState.IsValid)
            {
                if (chosenBellSchedModel is null)
                {
                    return NotFound();
                }
                List<string> scheds = ["Daily Bell Schedule", "Pep Rally Bell Schedule", "2 Hour Delay Bell Schedule", "Extended Aves Bell Schedule", "Custom Bell Schedule"];
                if (scheds.Contains(chosenBellSchedModel.Name!))
                {
                    _context.Add(chosenBellSchedModel);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                } else
                {
                    return NotFound("Bell Schedule Mismatch. Please try again.");
                }
            }
            return View(chosenBellSchedModel);
        }

        // GET: ChosenBellSchedule/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chosenBellSchedModel = await _context.ChosenBellSchedModels.FindAsync(id);
            if (chosenBellSchedModel == null)
            {
                return NotFound();
            }
            return View(chosenBellSchedModel);
        }

        // POST: ChosenBellSchedule/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] ChosenBellSchedModel chosenBellSchedModel)
        {
            if (chosenBellSchedModel is null)
            {
                return NotFound();
            }

            if (id != chosenBellSchedModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    List<string> scheds = ["Daily Bell Schedule", "Pep Rally Bell Schedule", "2 Hour Delay Bell Schedule", "Extended Aves Bell Schedule", "Custom Bell Schedule"];
                    if (scheds.Contains(chosenBellSchedModel.Name!))
                    {
                        _context.Update(chosenBellSchedModel);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        return NotFound("Bell Schedule Mismatch. Please try again.");
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChosenBellSchedModelExists(chosenBellSchedModel.Id))
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
            return View(chosenBellSchedModel);
        }

        private bool ChosenBellSchedModelExists(int id)
        {
            return _context.ChosenBellSchedModels.Any(e => e.Id == id);
        }
    }
}
