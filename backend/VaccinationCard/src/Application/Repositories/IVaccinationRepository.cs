using Domain.Entities;

namespace Application.Repositories;

public interface IVaccinationRepository : IRepositoryBase<Vaccination, Guid>
{
    Task DeleteAsync(Guid vaccinationId);
    Task<Vaccination?> GetLastVaccinationByPersonAndVaccine(Guid personId, Guid vaccineId);
}
