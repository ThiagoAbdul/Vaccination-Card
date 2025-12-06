using Application.Common.Models;
using Domain.Entities;
using Domain.Enums;
using Domain.ValueObjects;
using MediatR;

namespace Application.Features.People.Commands.CreatePerson;

public class CreatePersonCommand : IRequest<Result<CreatePersonResponse>>
{
    public Name Name { get; set; }
    public string CPF { get; set; } 
    public string? RG { get; set; }
    public Gender Gender { get; set; }
    public DateOnly BirthDate { get; set; }

    public Person ToEntity() => new()
    {
        Id = Guid.NewGuid(),
        Name = Name,
        CPF = CPF,
        RG = RG,
        Gender = Gender,
        BirthDate = BirthDate,
    };
}
