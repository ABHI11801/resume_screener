using Microsoft.EntityFrameworkCore;
using ResumeScreener.Models;

namespace ResumeScreener.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Unique email constraint
        builder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        // Unique Google ID (when present)
        builder.Entity<User>()
            .HasIndex(u => u.GoogleId)
            .IsUnique()
            .HasFilter("[GoogleId] IS NOT NULL");
    }
}
