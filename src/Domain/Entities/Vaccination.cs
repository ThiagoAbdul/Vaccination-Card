using Domain.Enums;
using Domain.ValueObjects;

namespace Domain.Entities;

public class Vaccination : AuditableEntity<Guid>
{
    public Guid PersonId { get; set; }
    public Person? Person { get; set; }
    public Guid VaccineId { get; set; }
    public Vaccine? Vaccine { get; set; }
    public DateOnly VaccinationDate { get; set; }
    public VaccinationDose Dose { get; set; }

}
