using Application.Common.Models;
using Application.Features.Vaccines.Responses;
using Domain.Entities;
using MediatR;

namespace Application.Features.Vaccines.Commands.CreateVaccine;

public class CreateVaccineCommandHandler : IRequestHandler<CreateVaccineCommand, Result<CreateVaccineResponse>>
{
    public async Task<Result<CreateVaccineResponse>> Handle(CreateVaccineCommand request, CancellationToken cancellationToken)
    {
        return new CreateVaccineResponse(new Vaccine());
    }
}
