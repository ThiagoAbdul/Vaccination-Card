using Application.Common.Models;
using Application.Features.Vaccines.GetVaccines;
using Application.Repositories;
using MediatR;

namespace Application.Features.Vaccines.GetAllVaccines;

public class GetAllVaccinesQueryHandler(IVaccineRepository vaccineRepository) : IRequestHandler<GetAllVaccinesQuery, Result<IEnumerable<VaccineResponse>>>
{
    public async Task<Result<IEnumerable<VaccineResponse>>> Handle(GetAllVaccinesQuery request, CancellationToken cancellationToken)
    {
        // Vacina listará todos pois não serão muitas vacinas, caso contrário seria paginado

        var vaccines = await vaccineRepository.GetAllAsync();

        var response =  vaccines.Select(x => new VaccineResponse(x));

        return Result.Success(response);
    }
}
