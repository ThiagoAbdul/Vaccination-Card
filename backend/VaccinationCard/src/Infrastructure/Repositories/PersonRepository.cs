using Application.Common.Models;
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

    public async Task<PageModel<Person>> GetPaginatedAsync(int page, int pageSize, string searchTerm)
    {

        page = Math.Max(1, page);
        pageSize = Math.Max(1, pageSize);

        searchTerm = $"{searchTerm.Trim().ToLower()}%"; // Salvo lower e busco lower, pra não usar ILIKE que é masi caro
                                                        // e nem todos SGBDs suportam 


        var query = _db.Persons.AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {

            // Buscar na coluna pesquisável pra não fazer OR com LIKE
            query = query.Where(p => EF.Functions.Like(p.NameSearchableColumn, searchTerm)); // Já protege contra SQL Injection
                                                                                   // Eu fui pesquisar por que fiquei preocupado
        }

        var totalCount = await query.CountAsync();

        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var skipAmount = (page - 1) * pageSize;

        var persons = await query
            .OrderBy(p => p.CreatedAt) // Curiosidade, se eu utilizasse o ID que é um GUID
                                       // O PostgreSQL (ou outro SGBD) iria fazer praticamente uma busca sequencial,
                                       // Pois UUID não é ordenável, então não se encaixa bem na B* TREE (estrutura de dados mais utilizada para indexar)
                                       // Existe o UUIDv7 que é ordenável
            .Skip(skipAmount)
            .Take(pageSize)
            .ToListAsync();

        return new PageModel<Person>
        {
            Items = persons,
            CurrentPage = page,
            PageSize = pageSize,
            TotalItems = totalCount,
            TotalPages = totalPages
        };
    }

}
