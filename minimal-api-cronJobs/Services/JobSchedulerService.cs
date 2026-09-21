using CronJobs.Data;
using CronJobs.Models;
using Cronos;
using Microsoft.EntityFrameworkCore;

namespace CronJobs.Services
{
    public class JobSchedulerService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<JobSchedulerService> _logger;

        public JobSchedulerService(IServiceScopeFactory scopeFactory, ILogger<JobSchedulerService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var timer = new PeriodicTimer(TimeSpan.FromSeconds(30));
            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                try
                {
                    await RunDueJobs(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while checking due jobs");
                }
            }
        }

        private async Task RunDueJobs(CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<CronJobsDbContext>();
            var now = DateTime.UtcNow;
            var dueJobs = await context.Jobs
                .Where(j => j.Active && j.NextRun != null && j.NextRun <= now)
                .ToListAsync(ct);

            foreach (var job in dueJobs)
            {
                try
                {
                    await ExecuteJob(job, ct);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Job {Id} failed", job.IdJob);
                }

                job.LastRun = now;
                job.NextRun = CronExpression.Parse(job.Schedule, CronFormat.Standard)
                    .GetNextOccurrence(now, TimeZoneInfo.Utc);
            }
            await context.SaveChangesAsync(ct);
        }

        private Task ExecuteJob(JobModel job, CancellationToken ct)
        {
            _logger.LogInformation("Running job {Id} - {Name} (type: {Type})", job.IdJob, job.Name, job.Type);
            return Task.CompletedTask;
        }
    }
}