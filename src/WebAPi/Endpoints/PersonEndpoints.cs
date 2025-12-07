using Application.Features.People.Commands.CreatePerson;
using Application.Features.People.Commands.DeletePerson;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebAPi.Extensions;

namespace WebAPi.Endpoints;

public static class PersonEndpoints
{
    public static IEndpointRouteBuilder MapPersonEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/persons")
           .WithTags("Persons");

        group.MapPost("/", async ([FromBody] CreatePersonCommand command, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(command);

            if (result.IsSuccess)
                return Results.Created($"/api/persons/{result.Value!.Id}", result.Value);

            return result.ToHttpResult();
        })
        .AddValidation<CreatePersonCommand>()
        .Produces<CreatePersonResponse>(201);

        group.MapDelete("/{personId}", async ([FromRoute] Guid personId, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new DeletePersonCommand(personId));

            return result.ToHttpResult();
        })
        .Produces(204)
        .Produces(404);

        return app;
    }

}
