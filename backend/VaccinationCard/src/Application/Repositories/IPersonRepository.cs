using Application.Common.Models;
using Domain.Entities;

namespace Application.Repositories;

public interface IPersonRepository : IRepositoryBase<Person, Guid>
{
    Task DeletePersonAndVaccinationsAsync(Guid personId);
    Task<bool> ExistsByCPFAsync(string cpf);
    Task<PageModel<Person>> GetPaginatedAsync(int page, int pageSize, string searchTerm);
}
