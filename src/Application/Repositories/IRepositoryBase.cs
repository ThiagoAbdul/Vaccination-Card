
namespace Application.Repositories;

public interface IRepositoryBase<T>
{
    Task AddAsync(T entity);
    Task SaveChangesAsync();
}
