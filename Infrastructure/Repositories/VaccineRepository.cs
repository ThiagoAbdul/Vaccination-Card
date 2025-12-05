using Application.Repositories;
using Domain.Entities;
using Infrastructure.Context;

namespace Infrastructure.Repositories;

public class VaccineRepository(AppDbContext db) : RepositoryBase<Vaccine>(db), IVaccineRepository
{
}
