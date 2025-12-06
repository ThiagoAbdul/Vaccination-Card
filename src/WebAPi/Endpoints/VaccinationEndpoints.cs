using Application.Features.Vaccinations.Commands.CreateVaccination;
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

        return app;
    }

}
