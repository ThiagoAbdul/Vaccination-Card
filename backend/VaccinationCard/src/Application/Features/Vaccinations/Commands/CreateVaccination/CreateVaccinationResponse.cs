using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Vaccinations.Commands.CreateVaccination;

public class CreateVaccinationResponse(Vaccination vaccination)
{
    public Guid Id { get;  } = vaccination.Id;
    public Guid PersonId { get; } = vaccination.PersonId;
    public Guid VaccineId { get; } = vaccination.VaccineId;
    public DateOnly VaccinationDate { get; } = vaccination.VaccinationDate;
    public VaccineDoseType DoseType { get;  } = vaccination.Dose.Type;
    public int DoseNumber { get; } = vaccination.Dose.DoseNumber;
}
