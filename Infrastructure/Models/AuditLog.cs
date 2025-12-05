using Application.Common.Enums;


namespace Application.Common.Models
{
    public class AuditLog
    {
        public Guid Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string? CreatedBy { get; set; }
        public CrudOperation Operation { get; set; }
        public string RefrredTable { get; set; } = string.Empty;
        public string RowIds { get; set; } = string.Empty;
        public string? ClientIp { get; set; }
        public string? CurrentJsonSnapshot { get; set; }
        public string? PreviousJsonSnapshot { get; set; }
    }
}