namespace CronJobs.Models
{
    public class JobModel
    {
        public int Id { get; set; }

        public string Nome { get; set; } = string.Empty;

        public string? Descricao { get; set; }

        public string Schedule { get; set; } = string.Empty;

        public string Tipo { get; set; } = string.Empty;

        public bool Ativo { get; set; } = true;

        public DateTime CriadoEm { get; set; }

        public DateTime? AtualizadoEm { get; set; }
    }
}