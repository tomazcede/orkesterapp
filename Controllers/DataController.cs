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

[ApiController]
[Route("[controller]")]
public class DataController : ControllerBase
{
    private readonly OrchesterContext _context;

    public DataController(OrchesterContext context)
    {
        _context = context;
    }

    [Route("user")]
    public async Task<ActionResult<User>> User(string email, string pass){
        List<User> users = await _context.Users.ToListAsync();

        User search = null;

        string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: pass!,
            salt: Encoding.ASCII.GetBytes("sol"),
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8));

        foreach (User user in users)
        {
            if(user.Email == email && user.Geslo == hashed){
                search = user;
                break;
            }
        }

        if(search == null)
            return NotFound();

        
        Role r = await _context.Role.FindAsync(search.RoleID);
        r.Users = null;

        search.Role = r;
        return CreatedAtAction(nameof(User),search);
    }

    [Route("performances")]
    public async Task<ActionResult<List<Performance>>> Performance(int id){
        var performances = await _context.Performance.Include(v => v.Venue).ToListAsync();
        performances.RemoveAll(b => b.OrchesterID != id);
        performances.ForEach(p => p.Venue.Performances = null);

        if(performances == null)
            return NotFound();

        
        return CreatedAtAction(nameof(performances),performances);
    }
}