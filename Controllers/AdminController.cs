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

public class AdminController : Controller
{
    private readonly OrchesterContext _context;

    public AdminController(OrchesterContext context)
    {
        _context = context;
    }

    public async void LogOut()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        Response.Redirect("/home");
    }

    public async Task<IActionResult> Index()
    {
        if(!SignedIn()){
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        return View();
    }

    private bool SignedIn()
    {
        if(HttpContext.User.Identity.Name != null && HttpContext.User.FindFirstValue(ClaimTypes.Role) == "Admin"){
            return true;
        }
        return false;
    }
}