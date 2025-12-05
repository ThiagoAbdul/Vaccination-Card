using Application.Common.Models;
using Common.Resources;
using FluentValidation;

namespace WebAPi.Filters;

public class ValidationFilter<T>(IValidator<T> validator) : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next) // Esse filtro eu uso em vários projetos,
                                                                                                                      // é bem útil pra integrar minimal API
                                                                                                                      // com FLuent Validation
    {

        var model = context.Arguments.OfType<T>().FirstOrDefault();
        if (model == null)
            return await next(context);

        var validationResult = await validator.ValidateAsync(model);

        if (validationResult.IsValid)
        {
            return await next(context);
        }

        var details = validationResult.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(e => e.ErrorMessage) as object
            );
        var errors = validationResult.Errors.Select(e => e.ErrorMessage);

        var response = new ResponseError(Messages.InvalidRequest) // Carteirinha vai ser em pt
        {
            Details = details,
            Errors = errors
        };

        return Results.BadRequest(response);
    }

}