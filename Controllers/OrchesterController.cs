using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using orkesterapp.Data;
using orkesterapp.Models;

namespace orkesterapp.Controllers
{
    public class OrchesterController : Controller
    {
        private readonly OrchesterContext _context;

        public OrchesterController(OrchesterContext context)
        {
            _context = context;
        }

        // GET: Orchester
        public async Task<IActionResult> Index()
        {
              return _context.Orchester != null ? 
                          View(await _context.Orchester.ToListAsync()) :
                          Problem("Entity set 'OrchesterContext.Orchester'  is null.");
        }

        // GET: Orchester/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Orchester == null)
            {
                return NotFound();
            }

            var orchester = await _context.Orchester
                .FirstOrDefaultAsync(m => m.ID == id);
            if (orchester == null)
            {
                return NotFound();
            }

            return View(orchester);
        }

        // GET: Orchester/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Orchester/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,OrchestraName")] Orchester orchester)
        {
            if (ModelState.IsValid)
            {
                _context.Add(orchester);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(orchester);
        }

        // GET: Orchester/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Orchester == null)
            {
                return NotFound();
            }

            var orchester = await _context.Orchester.FindAsync(id);
            if (orchester == null)
            {
                return NotFound();
            }
            return View(orchester);
        }

        // POST: Orchester/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,OrchestraName")] Orchester orchester)
        {
            if (id != orchester.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orchester);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrchesterExists(orchester.ID))
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
            return View(orchester);
        }

        // GET: Orchester/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Orchester == null)
            {
                return NotFound();
            }

            var orchester = await _context.Orchester
                .FirstOrDefaultAsync(m => m.ID == id);
            if (orchester == null)
            {
                return NotFound();
            }

            return View(orchester);
        }

        // POST: Orchester/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Orchester == null)
            {
                return Problem("Entity set 'OrchesterContext.Orchester'  is null.");
            }
            var orchester = await _context.Orchester.FindAsync(id);
            if (orchester != null)
            {
                _context.Orchester.Remove(orchester);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrchesterExists(int id)
        {
          return (_context.Orchester?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
