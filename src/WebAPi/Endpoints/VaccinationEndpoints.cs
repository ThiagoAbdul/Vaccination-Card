namespace WebAPi.Endpoints;

public static class VaccinationEndpoints
{
    public static IEndpointRouteBuilder MapVaccinationEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/vaccinations")
           .WithTags("Vaccinations");

        return app;
    }

}
