using orkesterapp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using System.Text;

namespace orkesterapp.Data;

public class OrchesterContext : DbContext
{
    public OrchesterContext(DbContextOptions<OrchesterContext> options) : base(options)
    {
    }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var memberrole = new Role { ID = 1, RoleName = "Member"};
        var conductorrole = new Role { ID = 2, RoleName = "Conductor"};
        var adminrole = new Role { ID = 3, RoleName = "Admin"};

        var testorc = new Orchester { ID = 1, OrchestraName = "test"};

        string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: "root"!,
            salt: Encoding.ASCII.GetBytes("sol"),
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8));

        var adminuser = new User { ID = 1, FirstMidName = "root", LastName = "root", Email = "root@root.si", Geslo = hashed, RoleID = 3, OrchesterID = 1};

        modelBuilder.Entity<Performance>().HasKey(am =>new{
            am.ID
        });

        modelBuilder.Entity<Orchester>().HasData(testorc);
        modelBuilder.Entity<Role>().HasData(adminrole, memberrole, conductorrole);
        modelBuilder.Entity<User>().HasData(adminuser);

        modelBuilder.Entity<Performance>().HasOne(o=>o.Orchester).WithMany(am=>am.Performances).HasForeignKey(o=>o.OrchesterID);
        modelBuilder.Entity<Performance>().HasOne(o=>o.Venue).WithMany(am=>am.Performances).HasForeignKey(o=>o.VenueID);

        modelBuilder.Entity<User>().ToTable("Users");
        modelBuilder.Entity<Role>().ToTable("Roles");

        modelBuilder.Entity<Venue>().ToTable("Venues");
        modelBuilder.Entity<Performance>().ToTable("Performances");
        modelBuilder.Entity<Orchester>().ToTable("Orchesters");
    }

    public async void addAdmin(){
        User user = new User();
        user.Email = "root@root.si";
        user.Geslo = "root";
        user.FirstMidName = "root";
        user.LastName = "root";

        string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: user.Geslo!,
                salt: Encoding.ASCII.GetBytes("sol"),
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));
            
        user.Geslo = hashed;
        
        this.Add(user);
        await this.SaveChangesAsync();
    }

    public DbSet<orkesterapp.Models.Role>? Role { get; set; }

    public DbSet<orkesterapp.Models.Venue>? Venue { get; set; }

    public DbSet<orkesterapp.Models.Orchester>? Orchester { get; set; }

    public DbSet<orkesterapp.Models.Performance>? Performance { get; set; }
}
