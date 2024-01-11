using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using orkesterapp.Models;
using orkesterapp.Data;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Security.Cryptography;

namespace orkesterapp.Controllers;

public class ConductorController : Controller
{
    private readonly OrchesterContext _context;

    public ConductorController(OrchesterContext context)
    {
        _context = context;
    }

    public async void LogOut()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        Response.Redirect("/home");
    }

    public async Task<IActionResult> Edit()
    {
        if(!SignedIn()){
            return RedirectToAction("Index", "Home", new { area = "" });
        }
        
        int uID = int.Parse(HttpContext.User.FindFirstValue("UserID"));

        var user = await _context.Users.FindAsync(uID);
        return View(user);
    }

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
                throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(user);
    }

    public async Task<IActionResult> Index()
    {
        if(!SignedIn()){
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        int oID = int.Parse(HttpContext.User.FindFirstValue("orchestraID"));
    
        var conductorContext = _context.Users.ToList();

        conductorContext.RemoveAll(b => b.OrchesterID != oID);
        conductorContext.RemoveAll(b => b.RoleID != 1);

        var perfContext = await _context.Performance.Include(u => u.Orchester).Include(v => v.Venue).ToListAsync();
        perfContext.RemoveAll(b => b.OrchesterID != oID);

        TempData["performances"] = perfContext;

        return View(conductorContext);
    }

    public async Task<IActionResult> Add()
    {
        if(!SignedIn()){
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        TempData["OID"] = int.Parse(HttpContext.User.FindFirstValue("orchestraID"));

        return View();
    }

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
        return RedirectToAction(nameof(Add));
    }

    // POST: User/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
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

    // POST: User/DeletePerformance/5
    [HttpPost, ActionName("DeletePerformance")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeletePerformance(int id)
    {
        if (_context.Performance == null)
        {
            return Problem("Entity set 'OrchesterContext.Users'  is null.");
        }
        var perf = await _context.Performance.FindAsync(id);
        
        if (perf != null)
        {
            _context.Performance.Remove(perf);
        }
        
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> AddPerformance()
    {
        if(!SignedIn()){
            return RedirectToAction("Index", "Home", new { area = "" });
        }
        
        ViewData["VenueID"] = new SelectList(_context.Venue, "ID", "VenueName");
        TempData["OID"] = int.Parse(HttpContext.User.FindFirstValue("orchestraID"));

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
     public async Task<IActionResult> CreatePerformance([Bind("ID,Date,OrchesterID,VenueID")] Performance performance)
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
            ViewData["VenueID"] = new SelectList(_context.Venue, "ID", "VenueName", performance.VenueID);
            return View(performance);
        }

    private bool SignedIn()
    {
        if(HttpContext.User.Identity.Name != null && HttpContext.User.FindFirstValue(ClaimTypes.Role) == "Conductor"){
            return true;
        }
        return false;
    }
}