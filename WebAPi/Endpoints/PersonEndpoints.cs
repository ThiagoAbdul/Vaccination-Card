namespace WebAPi.Endpoints;

public static class PersonEndpoints
{
    public static IEndpointRouteBuilder MapPersonEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/persons")
           .WithTags("Persons");

        return app;
    }

}
