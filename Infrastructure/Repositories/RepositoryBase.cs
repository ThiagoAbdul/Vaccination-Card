using Application.Repositories;
using Infrastructure.Context;

namespace Infrastructure.Repositories;

public abstract class RepositoryBase<T>(AppDbContext db) : IRepositoryBase<T>
{
    protected readonly AppDbContext _db = db;

    public async Task AddAsync(T entity)
    {
        await _db.AddAsync(entity);
    }

    public Task SaveChangesAsync() => _db.SaveChangesAsync();
}
