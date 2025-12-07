
namespace Application.Repositories;

public interface IRepositoryBase<T, ID>
{
    Task AddAsync(T entity);
    Task<bool> ExistsByIdAsync(ID id);
    Task<T?> GetByIdAsync(ID id);
    Task SaveChangesAsync();
}
