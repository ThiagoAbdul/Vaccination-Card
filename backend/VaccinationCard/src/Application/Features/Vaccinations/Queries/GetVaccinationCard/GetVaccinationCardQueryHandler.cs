using Application.Common.Enums;
using Application.Common.Models;
using Application.Features.Vaccinations.Queries.GetVaccinationCard;
using Application.Repositories;
using Common.Resources;
using MediatR;

namespace Application.Features.Vaccinations.Queries.GetVaccinationCardByPersonId;

public class GetVaccinationCardQueryHandler(IVaccineRepository vaccineRepository, IPersonRepository personRepository) : IRequestHandler<GetVaccinationCardQuery, Result<GetVaccinationCardResponse>>
{
    public async Task<Result<GetVaccinationCardResponse>> Handle(GetVaccinationCardQuery request, CancellationToken cancellationToken)
    {

        var person = await personRepository.GetByIdAsync(request.PersonId);

        if (person is null)
        {
            return Result.Failure<GetVaccinationCardResponse>(Messages.PersonNotFound, ResultStatus.NotFound);
        }

        // Fazer em uma query
        var vaccinesWithVaccinations = await vaccineRepository
            .GetAllVaccinesWithVaccinationsForPersonAsync(request.PersonId);

        return GetVaccinationCardResponseMapper.Map(vaccinesWithVaccinations, person);
        
    }
}
