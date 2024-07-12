namespace pruebaFGRP.Models
{
    public class Log
    {
        public int Id { get; set; }
        public string? Message { get; set; }
        public LogLevel Level { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public enum LogLevel
    {
        Information,
        Warning,
        Error,
        Critical
    }
}
