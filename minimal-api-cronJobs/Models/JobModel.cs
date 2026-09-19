namespace CronJobs.Models
{
    public class JobModel
    {
        public int IdJob { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Desc { get; set; }

        public string Schedule { get; set; } = string.Empty;

        public string Type { get; set; } = string.Empty;

        public bool Active { get; set; } = true;

        public DateTime CreteadOn { get; set; }

        public DateTime? UpdateOn { get; set; }
    }
}