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
    public class Performanceontroller : Controller
    {
        private readonly OrchesterContext _context;

        public Performanceontroller(OrchesterContext context)
        {
            _context = context;
        }

        // GET: Performanceontroller
        public async Task<IActionResult> Index()
        {
              return _context.Performance != null ? 
                          View(await _context.Performance.ToListAsync()) :
                          Problem("Entity set 'OrchesterContext.Performance'  is null.");
        }

        // GET: Performanceontroller/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Performance == null)
            {
                return NotFound();
            }

            var performance = await _context.Performance
                .FirstOrDefaultAsync(m => m.ID == id);
            if (performance == null)
            {
                return NotFound();
            }

            return View(performance);
        }

        // GET: Performanceontroller/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Performanceontroller/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,OrchesterID,VenueID,Date")] Performance performance)
        {
            if (ModelState.IsValid)
            {
                _context.Add(performance);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(performance);
        }

        // GET: Performanceontroller/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Performance == null)
            {
                return NotFound();
            }

            var performance = await _context.Performance.FindAsync(id);
            if (performance == null)
            {
                return NotFound();
            }
            return View(performance);
        }

        // POST: Performanceontroller/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,OrchesterID,VenueID,Date")] Performance performance)
        {
            if (id != performance.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(performance);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PerformanceExists(performance.ID))
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
            return View(performance);
        }

        // GET: Performanceontroller/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Performance == null)
            {
                return NotFound();
            }

            var performance = await _context.Performance
                .FirstOrDefaultAsync(m => m.ID == id);
            if (performance == null)
            {
                return NotFound();
            }

            return View(performance);
        }

        // POST: Performanceontroller/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Performance == null)
            {
                return Problem("Entity set 'OrchesterContext.Performance'  is null.");
            }
            var performance = await _context.Performance.FindAsync(id);
            if (performance != null)
            {
                _context.Performance.Remove(performance);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PerformanceExists(int id)
        {
          return (_context.Performance?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
