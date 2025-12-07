using Domain.Entities;
using Domain.Enums;
using Domain.ValueObjects;

namespace Application.Features.People.Queries;

public class PersonResponse(Person person)
{
    public Guid Id { get; set; } = person.Id;
    public string FirstName { get; set; } = person.Name.FirstName;
    public string LastName { get; set; } = person.Name.LastName;
    public Gender Gender { get; set; } = person.Gender;
    public DateOnly BirthDate { get; set; } = person.BirthDate;
}
