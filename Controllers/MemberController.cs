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

using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using System.Text;

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
                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: user.Geslo!,
                        salt: Encoding.ASCII.GetBytes("sol"),
                        prf: KeyDerivationPrf.HMACSHA256,
                        iterationCount: 100000,
                        numBytesRequested: 256 / 8));
                    
                user.Geslo = hashed;

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
        
        var memberContext = await _context.Performance.Include(u => u.Orchester).Include(v => v.Venue).ToListAsync();
        memberContext.RemoveAll(b => b.OrchesterID != oID);

        return View(memberContext);
    }

    private bool SignedIn()
    {
        if(HttpContext.User.Identity.Name != null && HttpContext.User.FindFirstValue(ClaimTypes.Role) == "Member"){
            return true;
        }
        return false;
    }
}