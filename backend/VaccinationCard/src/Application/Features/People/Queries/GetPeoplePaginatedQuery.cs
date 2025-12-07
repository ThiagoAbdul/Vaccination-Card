using Application.Common.Models;
using MediatR;

namespace Application.Features.People.Queries;

public record GetPeoplePaginatedQuery(int Page, int PageSize, string SearchTerm) : IRequest<Result<PageModel<PersonResponse>>>;
