namespace WebAPi.Endpoints;

public static class VaccineEndpoints
{
    public static IEndpointRouteBuilder MapVaccineEndpoints(this IEndpointRouteBuilder app) // Não vou versionar a API para não poluir o código,
                                                                    // Da pra adicionar versionamento posteriormente
                                                                    // deixando a V1 como default
    {

        var group = app.MapGroup("api/vaccines")
           .WithTags("Vaccines");



        group.MapGet("/", () =>
        {
            return Results.Ok(new[] { "Vaccine A", "Vaccine B", "Vaccine C" });
        })
        .WithName("GetVaccines");


        return app;
    }
}
