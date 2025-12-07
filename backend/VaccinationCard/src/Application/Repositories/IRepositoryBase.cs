
namespace Application.Repositories;

public interface IRepositoryBase<T, ID>
{
    Task AddAsync(T entity);
    Task<bool> ExistsByIdAsync(ID id);
    Task<List<T>> GetAllAsync(); // Em raríssimos casos e ainda com filtro de quem está deletado
    Task<T?> GetByIdAsync(ID id);
    Task SaveChangesAsync();
}
