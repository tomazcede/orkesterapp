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
    public class PerformanceController : Controller
    {
        private readonly OrchesterContext _context;

        public PerformanceController(OrchesterContext context)
        {
            _context = context;
        }

        // GET: Performance
        public async Task<IActionResult> Index()
        {
            var orchesterContext = _context.Performance.Include(p => p.Orchester).Include(p => p.Venue);
            return View(await orchesterContext.ToListAsync());
        }

        // GET: Performance/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Performance == null)
            {
                return NotFound();
            }

            var performance = await _context.Performance
                .Include(p => p.Orchester)
                .Include(p => p.Venue)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (performance == null)
            {
                return NotFound();
            }

            return View(performance);
        }

        // GET: Performance/Create
        public IActionResult Create()
        {
            ViewData["OrchesterID"] = new SelectList(_context.Orchester, "ID", "OrchestraName");
            ViewData["VenueID"] = new SelectList(_context.Venue, "ID", "VenueName");
            return View();
        }

        // POST: Performance/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Date,OrchesterID,VenueID")] Performance performance)
        {
            foreach (var modelState in ViewData.ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    Console.WriteLine(error.ErrorMessage);
                    Console.WriteLine(error.Exception);
                }
            }
            
            if (ModelState.IsValid)
            {
                _context.Add(performance);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OrchesterID"] = new SelectList(_context.Orchester, "ID", "OrchestraName", performance.OrchesterID);
            ViewData["VenueID"] = new SelectList(_context.Venue, "ID", "VenueName", performance.VenueID);
            return View(performance);
        }

        // GET: Performance/Edit/5
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
            ViewData["OrchesterID"] = new SelectList(_context.Orchester, "ID", "OrchestraName", performance.OrchesterID);
            ViewData["VenueID"] = new SelectList(_context.Venue, "ID", "VenueName", performance.VenueID);
            return View(performance);
        }

        // POST: Performance/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Date,OrchesterID,VenueID")] Performance performance)
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
            ViewData["OrchesterID"] = new SelectList(_context.Orchester, "ID", "OrchestraName", performance.OrchesterID);
            ViewData["VenueID"] = new SelectList(_context.Venue, "ID", "VenueName", performance.VenueID);
            return View(performance);
        }

        // GET: Performance/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Performance == null)
            {
                return NotFound();
            }

            var performance = await _context.Performance
                .Include(p => p.Orchester)
                .Include(p => p.Venue)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (performance == null)
            {
                return NotFound();
            }

            return View(performance);
        }

        // POST: Performance/Delete/5
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
