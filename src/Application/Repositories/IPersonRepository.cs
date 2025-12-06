using Domain.Entities;

namespace Application.Repositories;

public interface IPersonRepository : IRepositoryBase<Person, Guid>
{
    Task<bool> ExistsByCPFAsync(string cpf);
}
