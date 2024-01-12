using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using orkesterapp.Data;
using orkesterapp.Models;

namespace orkesterapp.Controllers
{
    public class UserController : Controller
    {
        private readonly OrchesterContext _context;

        public UserController(OrchesterContext context)
        {
            _context = context;
        }

        // GET: User
        public async Task<IActionResult> Index()
        {
            if(!SignedIn()){
                return RedirectToAction("Index", "Home", new { area = "" });
            }

            var orchesterContext = _context.Users.Include(u => u.Orchester).Include(u => u.Role);
            return View(await orchesterContext.ToListAsync());
        }

        // GET: User/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if(!SignedIn()){
                return RedirectToAction("Index", "Home", new { area = "" });
            }

            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.Orchester)
                .Include(u => u.Role)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: User/Create
        public IActionResult Create()
        {
            if(!SignedIn()){
                return RedirectToAction("Index", "Home", new { area = "" });
            }

            ViewData["OrchesterID"] = new SelectList(_context.Orchester, "ID", "OrchestraName");
            ViewData["RoleID"] = new SelectList(_context.Role, "ID", "RoleName");
            return View();
        }

        // POST: User/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,LastName,FirstMidName,Email,Geslo,RoleID,OrchesterID")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OrchesterID"] = new SelectList(_context.Orchester, "ID", "OrchestraName", user.OrchesterID);
            ViewData["RoleID"] = new SelectList(_context.Role, "ID", "RoleName", user.RoleID);
            return View(user);
        }

        // GET: User/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if(!SignedIn()){
                return RedirectToAction("Index", "Home", new { area = "" });
            }

            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ViewData["OrchesterID"] = new SelectList(_context.Orchester, "ID", "OrchestraName", user.OrchesterID);
            ViewData["RoleID"] = new SelectList(_context.Role, "ID", "RoleName", user.RoleID);
            return View(user);
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,LastName,FirstMidName,Email,Geslo,RoleID,OrchesterID")] User user)
        {
            if (id != user.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.ID))
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
            ViewData["OrchesterID"] = new SelectList(_context.Orchester, "ID", "OrchestraName", user.OrchesterID);
            ViewData["RoleID"] = new SelectList(_context.Role, "ID", "RoleName", user.RoleID);
            return View(user);
        }

        // GET: User/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if(!SignedIn()){
                return RedirectToAction("Index", "Home", new { area = "" });
            }

            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.Orchester)
                .Include(u => u.Role)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if(!SignedIn()){
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            
            if (_context.Users == null)
            {
                return Problem("Entity set 'OrchesterContext.Users'  is null.");
            }
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
          return (_context.Users?.Any(e => e.ID == id)).GetValueOrDefault();
        }

        private bool SignedIn()
        {
            if(HttpContext.User.Identity.Name != null && HttpContext.User.FindFirstValue(ClaimTypes.Role) == "Admin"){
                return true;
            }
            return false;
        }
    }
}
