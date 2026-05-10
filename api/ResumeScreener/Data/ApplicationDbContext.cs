using Microsoft.EntityFrameworkCore;
using ResumeScreener.Models;

namespace ResumeScreener.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();

    public DbSet<Job> Jobs => Set<Job>();

    public DbSet<JobSkill> JobSkills => Set<JobSkill>();
    public DbSet<Resume> Resumes => Set<Resume>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        //yunique email
        builder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        //unique Google ID
        builder.Entity<User>()
            .HasIndex(u => u.GoogleId)
            .IsUnique()
            .HasFilter("[GoogleId] IS NOT NULL");

        builder.Entity<Job>()
        .Property(j => j.MinimumScore)
        .HasPrecision(5, 2);
    }
}
