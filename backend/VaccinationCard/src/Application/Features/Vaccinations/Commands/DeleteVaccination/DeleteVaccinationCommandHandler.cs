using Application.Common.Enums;
using Application.Common.Models;
using Application.Repositories;
using Common.Resources;
using Domain.Entities;
using MediatR;

namespace Application.Features.Vaccinations.Commands.DeleteVaccination;

public class DeleteVaccinationCommandHandler(IVaccinationRepository vaccinationRepository) : IRequestHandler<DeleteVaccinationCommand, Result>
{
    public async Task<Result> Handle(DeleteVaccinationCommand request, CancellationToken cancellationToken)
    {
        var vaccination = await vaccinationRepository.GetByIdAsync(request.VaccinationId);

        if (vaccination is null)
        {
            return Result.Failure(Messages.VaccinationNotFound, ResultStatus.NotFound);
        }

        var nextVaccinations = await vaccinationRepository
            .GetSubsequentVaccinationsAsync(personId: vaccination.PersonId, 
                                            vaccineId: vaccination.VaccineId, 
                                            date: vaccination.VaccinationDate);

        AdjustSubsequentDoses(vaccination, nextVaccinations);

        await vaccinationRepository.DeleteAsync(vaccination);

        await vaccinationRepository.SaveChangesAsync();

        return Result.Success(ResultStatus.NoContent);
    }

    private static void AdjustSubsequentDoses(Vaccination vaccination, List<Vaccination> nextVaccinations)
    {
        // próximo esperado
        var currentDose = vaccination.Dose;

        foreach (var next in nextVaccinations.OrderBy(v => v.VaccinationDate)) // Assumir que as datas das doses estão coerentes
        {
            next.Dose.DoseNumber = currentDose.DoseNumber;
            next.Dose.Type = currentDose.Type;
            currentDose = next.Dose;
        }

    }

}
