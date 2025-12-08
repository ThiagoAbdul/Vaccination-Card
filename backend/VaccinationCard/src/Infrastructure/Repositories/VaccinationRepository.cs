using Application.Repositories;
using Domain.Entities;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class VaccinationRepository(AppDbContext db) : RepositoryBase<Vaccination, Guid>(db), IVaccinationRepository
{
    public async Task<Vaccination?> GetLastVaccinationByPersonAndVaccine(Guid personId, Guid vaccineId)
    {
        var vaccinations = await _dbSet
            .Where(x => x.PersonId == personId && x.VaccineId == vaccineId)
            .ToListAsync(); // Não serão muitas,
                            // vou trazer em memória
                            // e passar lógica de ordenação
                            // para aplicação
        return vaccinations.OrderByDescending(x => x.Dose)
            .FirstOrDefault(); // Sobrecarga da ordenação, pois Vaccination : IComparable
    }

    public async Task DeleteAsync(Vaccination vaccination)
    {
        _db.Attach(vaccination);
        vaccination.IsDeleted = true;
    }

    public Task<List<Vaccination>> GetSubsequentVaccinationsAsync(Guid personId, Guid vaccineId, DateOnly date)
    {
        return _dbSet
            .Where(x => x.PersonId == personId && x.VaccineId == vaccineId && x.VaccinationDate > date)
            .ToListAsync();
    }

}
