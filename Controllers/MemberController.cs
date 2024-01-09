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

namespace orkesterapp.Controllers;

public class MemberController : Controller
{
    private readonly OrchesterContext _context;

    public MemberController(OrchesterContext context)
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
        if(HttpContext.User.Identity == null){
            Response.Redirect("/home");
        }

        int oID = int.Parse(HttpContext.User.FindFirstValue("orchestraID"));
        
        var memberContext = await _context.Performance.Include(u => u.Orchester).Include(v => v.Venue).ToListAsync();
        foreach(var cont in memberContext){
            if(cont.OrchesterID != oID)
                memberContext.Remove(cont);
        }

        return View(memberContext);
    }
}