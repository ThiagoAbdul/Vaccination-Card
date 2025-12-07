using Domain.Entities;

namespace Application.Repositories;

public interface IVaccineRepository : IRepositoryBase<Vaccine, Guid>
{
    // repository methods can be added here
    Task<List<Vaccine>> GetAllVaccinesWithVaccinationsForPersonAsync(Guid personId);
}
