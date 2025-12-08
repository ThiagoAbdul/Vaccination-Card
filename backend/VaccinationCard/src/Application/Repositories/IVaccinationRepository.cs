using Domain.Entities;

namespace Application.Repositories;

public interface IVaccinationRepository : IRepositoryBase<Vaccination, Guid>
{
    Task DeleteAsync(Vaccination vaccination);
    Task<Vaccination?> GetLastVaccinationByPersonAndVaccine(Guid personId, Guid vaccineId);
    Task<List<Vaccination>> GetSubsequentVaccinationsAsync(Guid personId, Guid vaccineId, DateOnly date);
}
