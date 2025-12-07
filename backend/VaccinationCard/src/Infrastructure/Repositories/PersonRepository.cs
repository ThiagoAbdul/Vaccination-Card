using Application.Repositories;
using Domain.Entities;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class PersonRepository(AppDbContext db) : RepositoryBase<Person, Guid>(db), IPersonRepository
{

    public Task<bool> ExistsByCPFAsync(string cpf)
    {
        return db.Persons.AnyAsync(x => x.CPF == cpf);
    }

    public async Task DeletePersonAndVaccinationsAsync(Guid personId)
    {

        // Apesar do ExecuteUpdateAsync ter sua própria transação,
        // ele aproveita a transação externa, no caso, os 2 aproveitariam a transação externa

        var transaction = await _db.Database.BeginTransactionAsync();
        try
        {

            await _dbSet
                .Where(x => x.Id == personId)
                .ExecuteUpdateAsync(setter =>
                {
                    setter.SetProperty(p => p.IsDeleted, true);
                });

            await _db.Set<Vaccination>()
                .Where(x => x.PersonId == personId)
                .ExecuteUpdateAsync(setter =>
                {
                    setter.SetProperty(p => p.IsDeleted, true);
                });

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}
