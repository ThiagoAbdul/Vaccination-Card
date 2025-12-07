using Application.Common.Models;
using Application.Features.People.Commands.CreatePerson;
using Application.Features.People.Commands.DeletePerson;
using Application.Features.People.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebAPi.Extensions;

namespace WebAPi.Endpoints;

public static class PersonEndpoints
{
    public static IEndpointRouteBuilder MapPersonEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/people")
           .WithTags("People")
           .RequireAuthorization();

        group.MapPost("/", async ([FromBody] CreatePersonCommand command, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(command);

            if (result.IsSuccess)
                return Results.Created($"/api/people/{result.Value!.Id}", result.Value);

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

        group.MapGet("/", async ([FromServices] IMediator mediator, [FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string name = "") =>
        {
            var result = await mediator.Send(new GetPeoplePaginatedQuery(page, pageSize, name));

            return result.ToHttpResult();

        })
        .AddValidation<GetPeoplePaginatedQuery>()
        .Produces<PageModel<PersonResponse>>(200);

        return app;
    }

}
