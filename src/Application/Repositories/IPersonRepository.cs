using Domain.Entities;

namespace Application.Repositories;

public interface IPersonRepository : IRepositoryBase<Person>
{
    Task<bool> ExistsByCPFAsync(string cpf);
}
