using CronJobs.Data;
using CronJobs.Models;
using Microsoft.EntityFrameworkCore;

namespace CronJobs.Services
{
    public class JobService
    {
        private readonly CronJobsDbContext _context;

        public JobService(CronJobsDbContext context)
        {
            _context = context;
        }

        // post
        public async Task<JobModel> CreateJob(JobModel job)
        {
            _context.Jobs.Add(job);
            await _context.SaveChangesAsync();
            return job;
        }

        // get
        public async Task<List<JobModel>> GetJobs()
        {
            return await _context.Jobs.ToListAsync();
        }
    }
}