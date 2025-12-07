using Domain.Entities;

namespace Application.Repositories;

public interface IPersonRepository : IRepositoryBase<Person, Guid>
{
    Task DeletePersonAndVaccinationsAsync(Guid personId);
    Task<bool> ExistsByCPFAsync(string cpf);
}
