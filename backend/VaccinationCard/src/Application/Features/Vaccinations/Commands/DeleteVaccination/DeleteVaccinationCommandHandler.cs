using Application.Common.Enums;
using Application.Common.Models;
using Application.Repositories;
using Common.Resources;
using MediatR;

namespace Application.Features.Vaccinations.Commands.DeleteVaccination;

public class DeleteVaccinationCommandHandler(IVaccinationRepository vaccinationRepository) : IRequestHandler<DeleteVaccinationCommand, Result>
{
    public async Task<Result> Handle(DeleteVaccinationCommand request, CancellationToken cancellationToken)
    {
        var vaccination = await vaccinationRepository.GetByIdAsync(request.VaccinationId);

        if(vaccination is null)
        {
            return Result.Failure(Messages.VaccinationNotFound, ResultStatus.NotFound);
        }

        await vaccinationRepository.DeleteAsync(vaccination.Id);

        return Result.Success(ResultStatus.NoContent);
    }
}
