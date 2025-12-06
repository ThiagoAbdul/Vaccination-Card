using Application.Repositories;
using Domain.Entities;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class PersonRepository(AppDbContext db) : RepositoryBase<Person>(db), IPersonRepository
{

    public Task<bool> ExistsByCPFAsync(string cpf)
    {
        return db.Persons.AnyAsync(x => x.CPF == cpf);
    }
}
