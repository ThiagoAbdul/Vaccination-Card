using Application.Common.Models;
using MediatR;

namespace Application.Features.People.Commands.DeletePerson;

public class DeletePersonCommand(Guid personId) : IRequest<Result>
{
    public Guid PersonId { get; set; } = personId;
}
