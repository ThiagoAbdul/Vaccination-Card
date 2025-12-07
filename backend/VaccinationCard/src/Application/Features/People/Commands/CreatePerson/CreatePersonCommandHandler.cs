using Application.Common.Enums;
using Application.Common.Models;
using Application.Repositories;
using Application.Security;
using Common.Resources;
using Domain.Entities;
using MediatR;

namespace Application.Features.People.Commands.CreatePerson;

public class CreatePersonCommandHandler(IPersonRepository personRepository, IHashService hashService) : IRequestHandler<CreatePersonCommand, Result<CreatePersonResponse>>
{
    public async Task<Result<CreatePersonResponse>> Handle(CreatePersonCommand request, CancellationToken cancellationToken)
    {
        var person = request.ToEntity();

        person.CPF = hashService.GenerateDeterministicHash(person.CPF); // Determinístico para busca
        if(person.RG is not null) 
            person.RG = hashService.GenerateDeterministicHash(person.RG);
            

        var cpfExists = await personRepository.ExistsByCPFAsync(person.CPF);

        if (cpfExists)
        {
            return Result.Failure<CreatePersonResponse>(Messages.CPFAlreadyRegistered, ResultStatus.Conflict);
        }

        person.NameSearchableColumn = $"{person.Name.FullName.ToLower()}"; // para evitar buscas like

        await personRepository.AddAsync(person);
        await personRepository.SaveChangesAsync();

        return new CreatePersonResponse(person);
    }

}
