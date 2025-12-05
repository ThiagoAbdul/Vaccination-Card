namespace Domain.Entities;

public class Vaccination : AuditableEntity
{
    public Guid Id { get; set; }
    public Guid PersonId { get; set; }
    public Person? Person { get; set; }
    public Guid VaccineId { get; set; }
    public Vaccine? Vaccine { get; set; }
    public DateOnly VaccinationDate { get; set; }

}
