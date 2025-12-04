using Application.Common.Models;
using Application.Vaccines.Responses;
using Domain.Entities;
using MediatR;

namespace Application.Vaccines.Commands.CreateVaccine;

public class CreateVaccineCommand : IRequest<Result<CreateVaccineResponse>>
{
    public string Name { get; set; }
    public int Doses { get; set; }
    public int BoosterDoses { get; set; }


}
