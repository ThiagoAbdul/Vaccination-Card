using Application.Common.Models;
using Application.Features.Vaccinations.Queries.GetVaccinationCard;
using Application.Repositories;
using MediatR;

namespace Application.Features.Vaccinations.Queries.GetVaccinationCardByPersonId;

public class GetVaccinationCardQueryHandler(IVaccineRepository vaccineRepository) : IRequestHandler<GetVaccinationCardQuery, Result<GetVaccinationCardResponse>>
{
    public async Task<Result<GetVaccinationCardResponse>> Handle(GetVaccinationCardQuery request, CancellationToken cancellationToken)
    {
        // Fazer em uma query
        var vaccinesWithVaccinations = await vaccineRepository
            .GetAllVaccinesWithVaccinationsForPersonAsync(request.PersonId);

        return GetVaccinationCardResponseMapper.Map(vaccinesWithVaccinations);
        
    }
}
