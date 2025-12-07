using Application.Repositories;
using Domain.Entities;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class VaccineRepository(AppDbContext db) : RepositoryBase<Vaccine, Guid>(db), IVaccineRepository
{
    public Task<List<Vaccine>> GetAllVaccinesWithVaccinationsForPersonAsync(Guid personId)
    {
        return _dbSet
            .Include(v => v.Vaccinations.Where(vc => vc.PersonId == personId))
            .ToListAsync();
    }
}
