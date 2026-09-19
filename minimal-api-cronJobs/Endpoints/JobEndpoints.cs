using CronJobs.Models;
using CronJobs.Services;

namespace CronJobs.Endpoints
{
    public static class JobEndpoints
    {
        public static void MapJobEndpoints(this WebApplication app)
        {
            // post
            app.MapPost("/jobs", async (JobModel job, JobService service) =>
            {
                var createdJob = await service.CreateJob(job);
                return Results.Created($"/jobs/{createdJob.IdJob}", createdJob);
            });

            // get
            app.MapGet("/jobs", async (JobService service) =>
            {
                var jobs = await service.GetJobs();

                return Results.Ok(jobs);
            });

            // get {id}
            app.MapGet("/jobs/{id}", async (int id, JobService service) =>
            {
                var job = await service.GetJobById(id);

                if (job is null)
                    return Results.NotFound();

                return Results.Ok(job);
            });
        }
    }
}