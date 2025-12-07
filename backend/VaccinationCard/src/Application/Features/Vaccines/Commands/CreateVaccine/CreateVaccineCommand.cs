using Application.Common.Models;
using Domain.Entities;
using MediatR;

namespace Application.Features.Vaccines.Commands.CreateVaccine;

public class CreateVaccineCommand : IRequest<Result<CreateVaccineResponse>>
{
    public string Name { get; set; }
    public int Doses { get; set; }
    public int BoosterDoses { get; set; }

    public Vaccine ToEntity() => new()
    {
        Id = Guid.NewGuid(),
        Name = Name,
        Doses = Doses,
        BoosterDoses = BoosterDoses,
    };


}
