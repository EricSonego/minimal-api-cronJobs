using CronJobs.Models;
using Microsoft.EntityFrameworkCore;

namespace CronJobs.Data
{
    public class CronJobsDbContext : DbContext
    {
        public CronJobsDbContext(DbContextOptions<CronJobsDbContext> options) : base(options) { }

        public DbSet<JobModel> Jobs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<JobModel>()
                .HasKey(j => j.IdJob);
        }
    }
}