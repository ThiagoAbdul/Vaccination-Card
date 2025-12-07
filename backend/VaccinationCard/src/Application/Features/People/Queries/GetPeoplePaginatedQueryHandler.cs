using Application.Common.Models;
using Application.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Features.People.Queries;

public class GetPeoplePaginatedQueryHandler(IPersonRepository personRepository) : IRequestHandler<GetPeoplePaginatedQuery, Result<PageModel<PersonResponse>>>
{
    public async Task<Result<PageModel<PersonResponse>>> Handle(GetPeoplePaginatedQuery request, CancellationToken cancellationToken)
    {
        PageModel<Person> page = await personRepository.GetPaginatedAsync(request.Page, request.PageSize, request.SearchTerm);

        return page.Map(person => new PersonResponse(person));
    }
}
