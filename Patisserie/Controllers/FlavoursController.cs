using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Patisserie.Data;
using Patisserie.Models;

namespace Patisserie.Controllers
{
    public class FlavoursController : Controller
    {
        private readonly PatisserieContext _context;

        public FlavoursController(PatisserieContext context)
        {
            _context = context;
        }

        // GET: Flavours
        public async Task<IActionResult> Index()
        {
            return View(await _context.Flavour.ToListAsync());
        }

        // GET: Flavours/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flavour = await _context.Flavour
                .Include(f => f.ProductFlavours)
                .ThenInclude(f => f.Product)
                .FirstOrDefaultAsync(m => m.FlavourId == id);
            if (flavour == null)
            {
                return NotFound();
            }

            return View(flavour);
        }

        // GET: Flavours/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Flavours/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FlavourId,Name,Description,FlavourImage")] Flavour flavour)
        {
            if (ModelState.IsValid)
            {
                _context.Add(flavour);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(flavour);
        }

        // GET: Flavours/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flavour = await _context.Flavour.FindAsync(id);
            if (flavour == null)
            {
                return NotFound();
            }
            return View(flavour);
        }

        // POST: Flavours/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FlavourId,Name,Description,FlavourImage")] Flavour flavour)
        {
            if (id != flavour.FlavourId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(flavour);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FlavourExists(flavour.FlavourId))
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
            return View(flavour);
        }

        // GET: Flavours/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flavour = await _context.Flavour
                .FirstOrDefaultAsync(m => m.FlavourId == id);
            if (flavour == null)
            {
                return NotFound();
            }

            return View(flavour);
        }

        // POST: Flavours/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var flavour = await _context.Flavour.FindAsync(id);
            if (flavour != null)
            {
                _context.Flavour.Remove(flavour);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FlavourExists(int id)
        {
            return _context.Flavour.Any(e => e.FlavourId == id);
        }
    }
}
