using BodyQuest.Models;
using Microsoft.EntityFrameworkCore;

namespace BodyQuest.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<BodyModule> Modules => Set<BodyModule>();
    public DbSet<Lesson> Lessons => Set<Lesson>();
    public DbSet<Organ> Organs => Set<Organ>();
    public DbSet<Badge> Badges => Set<Badge>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<BodyModule>(e =>
        {
            e.HasIndex(m => m.Slug).IsUnique();
            e.Property(m => m.Slug).HasMaxLength(50);
            e.Property(m => m.Title).HasMaxLength(100);
            e.Property(m => m.Emoji).HasMaxLength(10);
            e.Property(m => m.ColorHex).HasMaxLength(9);
            e.HasMany(m => m.Lessons).WithOne(l => l.BodyModule!)
                .HasForeignKey(l => l.BodyModuleId).OnDelete(DeleteBehavior.Cascade);
            e.HasMany(m => m.Organs).WithOne(o => o.BodyModule!)
                .HasForeignKey(o => o.BodyModuleId).OnDelete(DeleteBehavior.Cascade);
        });

        b.Entity<Lesson>().Property(l => l.Title).HasMaxLength(150);
        b.Entity<Organ>().Property(o => o.Name).HasMaxLength(100);
        b.Entity<Organ>().Property(o => o.SvgId).HasMaxLength(60);
        b.Entity<Badge>().Property(x => x.Name).HasMaxLength(100);
    }
}
