using orkesterapp.Models;
using Microsoft.EntityFrameworkCore;

namespace orkesterapp.Data;

public class OrchesterContext : DbContext
{
    public OrchesterContext(DbContextOptions<OrchesterContext> options) : base(options)
    {
    }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().ToTable("Users");
        modelBuilder.Entity<Role>().ToTable("Roles");

        modelBuilder.Entity<Venue>().ToTable("Venues");
        modelBuilder.Entity<Performance>().ToTable("Performances");
        modelBuilder.Entity<Orchester>().ToTable("Orchesters");
    }

    public DbSet<orkesterapp.Models.Role>? Role { get; set; }

    public DbSet<orkesterapp.Models.Venue>? Venue { get; set; }

    public DbSet<orkesterapp.Models.Orchester>? Orchester { get; set; }

    public DbSet<orkesterapp.Models.Performance>? Performance { get; set; }
}
