using Domain.Entities;
using Domain.Enums;
using Domain.ValueObjects;

namespace Application.Features.People.Commands.CreatePerson;

public class CreatePersonResponse(Person person)
{
    public Guid Id { get; set; } = person.Id;
    public Name Name { get; set; } = person.Name;
    public Gender Gender { get; set; } = person.Gender;
    public DateOnly BirthDate { get; set; } = person.BirthDate;
}
