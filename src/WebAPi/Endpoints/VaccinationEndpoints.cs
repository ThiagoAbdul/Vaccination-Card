using Application.Features.Vaccinations.Commands.CreateVaccination;
using Application.Features.Vaccinations.Commands.DeleteVaccination;
using Application.Features.Vaccinations.Queries.GetVaccinationCardByPersonId;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebAPi.Extensions;

namespace WebAPi.Endpoints;

public static class VaccinationEndpoints
{
    public static IEndpointRouteBuilder MapVaccinationEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/vaccinations")
           .WithTags("Vaccinations");

        group.MapPost("/", async ([FromBody] CreateVaccinationCommand command, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(command);

            if(result.IsSuccess)
                return Results.Created($"/api/vaccinations/{result.Value!.Id}", result.Value);

            return result.ToHttpResult();

        })
        .AddValidation<CreateVaccinationCommand>()
        .Produces<CreateVaccinationResponse>(201);

        group.MapGet("/person/{personId}/vaccination-card", async ([FromRoute] Guid personId, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new GetVaccinationCardQuery(personId));

            return result.ToHttpResult();
        })
        .Produces<GetVaccinationCardResponse>(200);

        group.MapDelete("/{vaccinationId}", async ([FromRoute] Guid vaccinationId, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new DeleteVaccinationCommand(vaccinationId));

            return result.ToHttpResult();
        })
        .Produces(204);
        
        return app;
    }

}
