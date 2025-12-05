using Application.Repositories;
using Domain.Entities;
using Infrastructure.Context;

namespace Infrastructure.Repositories;

public class PersonRepository(AppDbContext db) : RepositoryBase<Person>(db), IPersonRepository
{


}
