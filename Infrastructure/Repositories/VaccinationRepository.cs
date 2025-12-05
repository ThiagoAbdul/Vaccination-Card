using Application.Repositories;
using Domain.Entities;
using Infrastructure.Context;

namespace Infrastructure.Repositories;

public class VaccinationRepository(AppDbContext db) : RepositoryBase<Vaccination>(db), IVaccinationRepository
{
    
}
