# minimal-api-cronJobs - Cron Jobs Scheduler API

minimal-api-cronJobs is a RESTful API developed with C# and .NET 8 for registering and automatically running scheduled jobs. Jobs are defined with standard cron expressions, stored in a database, and executed by a background scheduler that runs inside the API process.

## Technologies Used

* Runtime: .NET 8 (SDK 8.0)
* Web Framework: ASP.NET Core (Minimal APIs)
* Database: SQL Server
* ORM: Entity Framework Core 8.0 (Code First with Migrations)
* Cron Parsing: Cronos
* Background Processing: BackgroundService (hosted service)
* Documentation: Swagger

## Architecture and Best Practices

Instead of concentrating everything in a single Program.cs file, this project adopts a clean and decoupled structure:

* Isolated Endpoints: Routes are mapped through Extension Methods and grouped under `/jobs` with `MapGroup`, keeping the startup file organized.
* Dependency Injection: Native use of the .NET container to manage the lifecycle of the database context and services.
* Service Layer (Services): CRUD and cron validation logic isolated in `JobService`, separate from the HTTP layer.
* Background Scheduler: `JobSchedulerService` is a `BackgroundService` that uses a `PeriodicTimer` to check for due jobs every 30 seconds. It creates a new DI scope on each tick, since the `DbContext` is scoped and the hosted service is a singleton.
* Cron Validation: Schedules are validated with Cronos on create and update, so invalid expressions never reach the database.
* Precomputed Next Run: The next execution time is stored in the `NextRun` column, so the scheduler only queries jobs that are actually due instead of evaluating every expression on every tick.
* Failure Isolation: An error in one job is logged and does not stop the other jobs from running.
* UTC Everywhere: All dates (`CreatedOn`, `UpdateOn`, `LastRun`, `NextRun`) are stored and calculated in UTC.

## Project Structure

```
minimal-api-cronJobs/
├── Data/          # CronJobsDbContext
├── Endpoints/     # JobEndpoints (route mapping)
├── Migrations/    # EF Core migrations
├── Models/        # JobModel
├── Services/      # JobService and JobSchedulerService
└── Program.cs
```

## Job Model

| Field | Type | Description |
|---|---|---|
| idJob | int | Job identifier (generated) |
| name | string | Job name |
| desc | string? | Optional description |
| schedule | string | Cron expression (5 fields) |
| type | string | Job type: `log` or `http` |
| target | string? | URL to call (required for `http` jobs) |
| active | bool | Whether the job is scheduled to run |
| createdOn | datetime | Creation date (UTC) |
| updateOn | datetime? | Last update date (UTC) |
| lastRun | datetime? | Last execution (UTC) |
| nextRun | datetime? | Next scheduled execution (UTC) |

## Cron Format

The API uses the standard 5-field cron format: `minute hour day-of-month month day-of-week`.

| Expression | Meaning |
|---|---|
| `* * * * *` | Every minute |
| `*/5 * * * *` | Every 5 minutes |
| `0 9 * * *` | Every day at 09:00 (UTC) |
| `0 9 * * 1-5` | Weekdays at 09:00 (UTC) |

## API Endpoints

All routes are documented by Swagger.

### 1. Create Job
* Route: POST /jobs
* Payload (JSON):
    ```json
    {
      "name": "ping",
      "desc": "Calls a public endpoint every 5 minutes",
      "schedule": "*/5 * * * *",
      "type": "http",
      "target": "https://httpbin.org/get",
      "active": true
    }
    ```
* Response (HTTP 201): Returns the created job with `nextRun` already calculated.
* Response (HTTP 400): Invalid cron expression, invalid type, or `http` job without a valid target URL.

### 2. List Jobs
* Route: GET /jobs
* Response (HTTP 200): Returns all registered jobs.

### 3. Get Job by Id
* Route: GET /jobs/{id}
* Response (HTTP 200): Returns the job.
* Response (HTTP 404): Job not found.

### 4. Update Job
* Route: PUT /jobs/{id}
* Payload: Same format as the create endpoint.
* Response (HTTP 204): Job updated and `nextRun` recalculated (or cleared if the job is inactive).
* Response (HTTP 400 / 404): Invalid data or job not found.

### 5. Delete Job
* Route: DELETE /jobs/{id}
* Response (HTTP 204): Job deleted.
* Response (HTTP 404): Job not found.

## How the Scheduler Works

1. When a job is created or updated, `nextRun` is calculated from its cron expression.
2. Every 30 seconds, the scheduler selects the jobs where `active = true` and `nextRun <= now`.
3. Each due job is executed according to its type:
    * `log`: writes an entry to the application log.
    * `http`: sends a GET request to the job's target URL.
4. After each execution, `lastRun` is set and `nextRun` is recalculated for the next occurrence.

Because the scheduler checks every 30 seconds, a job can run up to 30 seconds after its scheduled time.

## How to Run the Project Locally

### Prerequisites
* .NET 8 SDK installed.
* A SQL Server instance (SQL Server Express or LocalDB works).

### Step-by-Step Guide

1. Clone the repository:
    ```bash
    git clone https://github.com/EricSonego/minimal-api-cronJobs.git
    cd minimal-api-cronJobs/minimal-api-cronJobs
    ```

2. Set your SQL Server connection string in `appsettings.json` (or in user secrets, to keep credentials out of the repository).

3. Install the Entity Framework Core CLI tool (if not already installed):
    ```bash
    dotnet tool install --global dotnet-ef
    ```

4. Run Migrations to create the database:
    ```bash
    dotnet ef database update
    ```

5. Run the application:
    ```bash
    dotnet run
    ```

6. Access the documentation:
    The API will open the Swagger UI in your default browser. If it does not open, navigate to:
    https://localhost:7159/swagger

## Next Steps (Future Improvements)
* Add an execution history table (start time, duration, status and error message) with a `GET /jobs/{id}/executions` endpoint.
* Introduce request and response DTOs, so clients cannot set fields such as `nextRun` and `lastRun`.
* Restrict allowed target URLs for `http` jobs to prevent SSRF, and add API key authentication.
* Support per-job time zones instead of UTC only.
* Add retry policies and overlapping-execution control for long-running jobs.
