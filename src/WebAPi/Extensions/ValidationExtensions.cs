using WebAPi.Filters;

namespace WebAPi.Extensions;

public static class ValidationExtension
{
    public static RouteHandlerBuilder AddValidation<T>(this RouteHandlerBuilder builder)
    {
        return builder.AddEndpointFilter<ValidationFilter<T>>();
    }
}