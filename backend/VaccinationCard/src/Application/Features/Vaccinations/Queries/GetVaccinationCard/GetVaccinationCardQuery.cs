using Application.Common.Models;
using MediatR;

namespace Application.Features.Vaccinations.Queries.GetVaccinationCardByPersonId;

public class GetVaccinationCardQuery(Guid personId) : IRequest<Result<GetVaccinationCardResponse>>
{
    public Guid PersonId { get; set; } = personId;
}
