using Domain.Interfaces;

namespace Domain.Entities;

public class AuditableEntity<ID> : EntityBase<ID>, IAuditable where ID : IEquatable<ID>
{
    public string? CreatedById { get; set; } // string pra ter maior desacoplamento com a autenticação
    public string? CreatedByName { get; set; } // pode ser o email ou username, quero algo imutável e identificável
    public DateOnly CreatedAt { get; set; }
    public string? LastUpdatedById { get; set; }
    public string? LastUpdatedByName { get; set; }
    public DateOnly LastUpdatedAt { get; set; }
    public bool IsDeleted { get; set; } // false é o default


    // Da pra perceber um padrão em CreatedBy e UpdatedBy,mas não vejo necessidade de abstrair e criar uma classe separada
    // (e consequentemente definir como ComplexType no EF Core pois seria uma classe mas não uma tabela).
    // Complexidade desnecessária.

}
