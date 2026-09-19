using CronJobs.Models;
using CronJobs.Services;

namespace CronJobs.Endpoints
{
    public static class JobEndpoints
    {
        public static void MapJobEndpoints(this WebApplication app)
        {
            app.MapPost("/jobs", async (JobModel job, JobService service) =>
            {
                var createdJob = await service.CreateJob(job);
                return Results.Created($"/jobs/{createdJob.IdJob}", createdJob);
            });
        }
    }
}