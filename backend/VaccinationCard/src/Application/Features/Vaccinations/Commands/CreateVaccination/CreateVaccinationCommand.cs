using Application.Common.Models;
using Domain.Entities;
using Domain.Enums;
using Domain.ValueObjects;
using MediatR;

namespace Application.Features.Vaccinations.Commands.CreateVaccination;

public class CreateVaccinationCommand : IRequest<Result<CreateVaccinationResponse>>
{
    public Guid PersonId { get; set; }
    public Guid VaccineId { get; set; }
    public DateOnly VaccinationDate { get; set; }
    public VaccineDoseType DoseType { get; set; }
    public int DoseNumber { get; set; }

    public Vaccination ToEntity() => new()
    {
        Id = Guid.NewGuid(),
        PersonId = PersonId,
        VaccineId = VaccineId,
        VaccinationDate = VaccinationDate,
        Dose = GetDose()
    };

    public VaccinationDose GetDose() => new(DoseType, DoseNumber);

}
