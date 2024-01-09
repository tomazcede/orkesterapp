using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

using orkesterapp.Models;
using orkesterapp.Data;
using Microsoft.EntityFrameworkCore;
using NuGet.Versioning;

namespace orkesterapp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly OrchesterContext _context;

    public HomeController(ILogger<HomeController> logger, OrchesterContext context)
    {
        _logger = logger;

        _context = context;
    }

    public IActionResult Index(bool notfound = false)
    {
        if(HttpContext.User.Identity.Name != null){
            Response.Redirect("/"+HttpContext.User.FindFirstValue(ClaimTypes.Role));
        }

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public async Task<IActionResult> AuthUser(string email, string pass)
    {
        List<User> users = await _context.Users.ToListAsync();

        User search = null;

        foreach (User user in users)
        {
            if(user.Email == email && user.Geslo == pass){
                search = user;
                break;
            }
        }
    
        if(search == null)
            return View("Index", true);

        Role r = await _context.Role.FirstOrDefaultAsync(a => a.ID ==search.RoleID);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, search.Email),
            new Claim("UserID", search.ID.ToString()),
            new Claim(ClaimTypes.Role, r.RoleName),
            new Claim("orchestraID", search.OrchesterID.ToString())
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        var authProperties = new AuthenticationProperties
        {
            //AllowRefresh = <bool>,
            // Refreshing the authentication session should be allowed.

            //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
            // The time at which the authentication ticket expires. A 
            // value set here overrides the ExpireTimeSpan option of 
            // CookieAuthenticationOptions set with AddCookie.

            //IsPersistent = true,
            // Whether the authentication session is persisted across 
            // multiple requests. When used with cookies, controls
            // whether the cookie's lifetime is absolute (matching the
            // lifetime of the authentication ticket) or session-based.

            //IssuedUtc = <DateTimeOffset>,
            // The time at which the authentication ticket was issued.

            //RedirectUri = <string>
            // The full path or absolute URI to be used as an http 
            // redirect response value.
        };


        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme, 
            new ClaimsPrincipal(claimsIdentity), 
            authProperties);
        
        Response.Redirect("/"+HttpContext.User.FindFirstValue(ClaimTypes.Role));
        
        return View("Index", true);

    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public IActionResult EditUser(int id)
    {
        return View();
    }
}
