using CronJobs.Data;
using CronJobs.Models;
using Cronos;
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

        // get {id}
        public async Task<JobModel?> GetJobById(int id)
        {
            return await _context.Jobs
                .FirstOrDefaultAsync(j => j.IdJob == id);
        }

        // update
        public async Task<bool> UpdateJob(int id, JobModel input)
        {
            var job = await _context.Jobs.FindAsync(id);
            if (job is null) return false;
            job.Name = input.Name;
            job.Desc = input.Desc;
            job.Schedule = input.Schedule;
            job.Type = input.Type;
            job.Active = input.Active;
            job.UpdateOn = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        // delete
        public async Task<bool> DeleteJob(int id)
        {
            var job = await _context.Jobs.FindAsync(id);
            if (job is null) return false;
            _context.Jobs.Remove(job);
            await _context.SaveChangesAsync();
            return true;
        }

        // validate crono
        public static bool IsValidSchedule(string schedule) =>
            CronExpression.TryParse(schedule, CronFormat.Standard, out _);
    }
}