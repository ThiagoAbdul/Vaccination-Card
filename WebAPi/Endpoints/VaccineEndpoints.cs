using Application.Vaccines.Commands.CreateVaccine;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebAPi.Extensions;

namespace WebAPi.Endpoints;

public static class VaccineEndpoints
{
    public static IEndpointRouteBuilder MapVaccineEndpoints(this IEndpointRouteBuilder app) // Não vou versionar a API para não poluir o código,
                                                                    // Da pra adicionar versionamento posteriormente
                                                                    // deixando a V1 como default
    {

        var group = app.MapGroup("api/vaccines")
           .WithTags("Vaccines");



        group.MapPost("/", async ([FromBody] CreateVaccineCommand command, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(command);
            if(result.IsSuccess) return Results.Created($"/api/vaccines/{result.Value!.Id}", result.Value);
            return result.ToHttpResult();
        })
        .AddValidation<CreateVaccineCommand>() // No .NET 10 as data Annotations funcionam bem com minimal APIs
                                                   // mas ainda não dá MUITO controle,
                                                   // Então mantive Fluent Validation com um filtro personalizado
        .WithName("GetVaccines");


        return app;
    }
}
