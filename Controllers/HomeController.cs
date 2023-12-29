using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using orkesterapp.Models;
using orkesterapp.Data;
using Microsoft.EntityFrameworkCore;

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
        if(notfound != false)
            TempData["notfound"] = notfound;  
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
        
        if(search.RoleID == 1)
            return View("Member", search);
        if(search.RoleID == 2)
            return View("Member", search); // admin
        if(search.RoleID == 3)
            return View("Member", search); // conductor
        
        return View("Index", true);

    }

    public IActionResult Member(User user)
    {
        return View(user);
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
