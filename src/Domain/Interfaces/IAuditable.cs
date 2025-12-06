namespace Domain.Interfaces;

public interface IAuditable : ISoftDeletable
{
    public string? CreatedById { get; set; } // string pra ter maior desacoplamento com a autenticação
    public string? CreatedByName { get; set; } // pode ser o email ou username, quero algo imutável e identificável
    public DateOnly CreatedAt { get; set; }
    public string? LastUpdatedById { get; set; }
    public string? LastUpdatedByName { get; set; }
    public DateOnly LastUpdatedAt { get; set; }
}
