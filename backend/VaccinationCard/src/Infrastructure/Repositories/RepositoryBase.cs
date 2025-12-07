using Application.Repositories;
using Domain.Entities;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public abstract class RepositoryBase<T, ID>(AppDbContext db) : IRepositoryBase<T, ID> where T : EntityBase<ID> where ID : IEquatable<ID>
{
    protected readonly AppDbContext _db = db;
    protected readonly DbSet<T> _dbSet = db.Set<T>();

    public Task<T?> GetByIdAsync(ID id)
    {
        return _dbSet.FirstOrDefaultAsync(x => x.Id.Equals(id)); // EF traduz de boa
    }
    public Task<bool> ExistsByIdAsync(ID id)
    {
        return _dbSet.AnyAsync(x => x.Id.Equals(id));
    }
    public async Task AddAsync(T entity)
    {
        await _db.AddAsync(entity);
    }

    public Task SaveChangesAsync() => _db.SaveChangesAsync();

    public Task<List<T>> GetAllAsync() // Em raríssimos casos e ainda com filtro de quem está deletado
    {
        return _dbSet.ToListAsync();
    } 


}
