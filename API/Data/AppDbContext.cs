using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<AppUser> Users { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<AppUser>().HasKey(u => u.Id);
        modelBuilder.Entity<AppUser>().Property(u => u.DisplayName).IsRequired();
        modelBuilder.Entity<AppUser>().Property(u => u.Email).IsRequired();
    }
}
