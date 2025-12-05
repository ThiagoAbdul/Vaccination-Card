namespace Domain.Entities;

public class Vaccine : AuditableEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int Doses { get; set; }
    public int BoosterDoses { get; set; }

}
