using Application.Common.Models;
using Application.Vaccines.Responses;
using Domain.Entities;
using MediatR;

namespace Application.Vaccines.Commands.CreateVaccine;

public class CreateVaccineCommandHandler : IRequestHandler<CreateVaccineCommand, Result<CreateVaccineResponse>>
{
    public async Task<Result<CreateVaccineResponse>> Handle(CreateVaccineCommand request, CancellationToken cancellationToken)
    {
        return new CreateVaccineResponse(new Vaccine());
    }
}
