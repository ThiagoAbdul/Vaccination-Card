using Domain.Entities;

namespace Application.Repositories;

public interface IVaccinationRepository : IRepositoryBase<Vaccination, Guid>
{
    Task<Vaccination?> GetLastVaccinationByPersonAndVaccine(Guid personId, Guid vaccineId);
}
