using Microsoft.EntityFrameworkCore;
using DistributedJobManagementSystem.Server.Models;

namespace DistributedJobManagementSystem.Server.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Job> Jobs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Add indexes for performance on job queue queries
            modelBuilder.Entity<Job>()
                .HasIndex(j => j.Status);
            modelBuilder.Entity<Job>()
                .HasIndex(j => j.Priority);
        }
    }
} 