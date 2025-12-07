using Application.Common.Models;
using Application.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Features.Vaccines.Commands.CreateVaccine;

public class CreateVaccineCommandHandler(IVaccineRepository vaccineRepository) : IRequestHandler<CreateVaccineCommand, Result<CreateVaccineResponse>>
{
    public async Task<Result<CreateVaccineResponse>> Handle(CreateVaccineCommand request, CancellationToken cancellationToken)
    {
        var vaccine = request.ToEntity();
        await vaccineRepository.AddAsync(vaccine);
        await vaccineRepository.SaveChangesAsync();

        var response = new CreateVaccineResponse(vaccine);

        return response;
    }
}
