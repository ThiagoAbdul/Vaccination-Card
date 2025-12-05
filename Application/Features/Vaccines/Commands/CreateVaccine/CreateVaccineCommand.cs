using Application.Common.Models;
using Application.Features.Vaccines.Responses;
using Domain.Entities;
using MediatR;

namespace Application.Features.Vaccines.Commands.CreateVaccine;

public class CreateVaccineCommand : IRequest<Result<CreateVaccineResponse>>
{
    public string Name { get; set; }
    public int Doses { get; set; }
    public int BoosterDoses { get; set; }


}
