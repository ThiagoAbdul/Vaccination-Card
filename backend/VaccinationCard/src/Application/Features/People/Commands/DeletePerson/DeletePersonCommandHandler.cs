using Application.Common.Enums;
using Application.Common.Models;
using Application.Repositories;
using Common.Resources;
using MediatR;

namespace Application.Features.People.Commands.DeletePerson;

public class DeletePersonCommandHandler(IPersonRepository personRepository) : IRequestHandler<DeletePersonCommand, Result>
{
    public async Task<Result> Handle(DeletePersonCommand request, CancellationToken cancellationToken)
    {
        var person = await personRepository.GetByIdAsync(request.PersonId);

        if(person is null)
            return Result.Failure(Messages.PersonNotFound, ResultStatus.NotFound);
        await personRepository.DeletePersonAndVaccinationsAsync(request.PersonId);

        return Result.Success(ResultStatus.NoContent);
    }
}
