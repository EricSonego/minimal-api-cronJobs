using CronJobs.Models;
using CronJobs.Services;

namespace CronJobs.Endpoints
{
    public static class JobEndpoints
    {
        public static void MapJobEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/jobs");

            // post
            group.MapPost("/", async (JobModel job, JobService service) =>
            {
                if (!JobService.IsValidSchedule(job.Schedule))
                    return Results.BadRequest("invalid schedule. use the format cron, ex.: */5 * * * *");

                var createdJob = await service.CreateJob(job);
                return Results.Created($"/jobs/{createdJob.IdJob}", createdJob);
            });

            // get
            group.MapGet("/", async (JobService service) =>
            {
                var jobs = await service.GetJobs();
                return Results.Ok(jobs);
            });

            // get {id}
            group.MapGet("/{id:int}", async (int id, JobService service) =>
            {
                var job = await service.GetJobById(id);
                return job is null ? Results.NotFound() : Results.Ok(job);
            });

            // put
            group.MapPut("/{id:int}", async (int id, JobModel input, JobService service) =>
            {
                if (!JobService.IsValidSchedule(input.Schedule))
                    return Results.BadRequest("invalid schedule. use the format cron, ex.: */5 * * * *");

                return await service.UpdateJob(id, input) ? Results.NoContent() : Results.NotFound();
            });

            // delete
            group.MapDelete("/{id:int}", async (int id, JobService service) =>
                await service.DeleteJob(id) ? Results.NoContent() : Results.NotFound());
        }
    }
}