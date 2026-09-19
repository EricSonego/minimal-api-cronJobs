using CronJobs.Data;
using CronJobs.Models;

namespace CronJobs.Services
{
    public class JobService
    {
        private readonly CronJobsDbContext _context;

        public JobService(CronJobsDbContext context)
        {
            _context = context;
        }

        public async Task<JobModel> CreateJob(JobModel job)
        {
            _context.Jobs.Add(job);
            await _context.SaveChangesAsync();
            return job;
        }
    }
}