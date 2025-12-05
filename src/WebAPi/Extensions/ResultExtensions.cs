using Application.Common.Models;
using Application.Common.Enums;

namespace WebAPi.Extensions;

public static class ResultExtensions // Aqui é mais um ponto legal do Result Pattern,
                                     // ele facilita abstrair a resposta nos controllers  minimal APIs
                                     // Map centralizado
{
    public static IResult ToHttpResult(this Result result)
    {
        return result.Status switch
        {
            ResultStatus.Ok => Results.Ok(),
            ResultStatus.Created => Results.StatusCode(StatusCodes.Status201Created),
            ResultStatus.Accpeted => Results.StatusCode(StatusCodes.Status202Accepted),
            ResultStatus.NoContent => Results.NoContent(),

            ResultStatus.BadRequest => Results.BadRequest(result.Error),
            ResultStatus.NotFound => Results.NotFound(result.Error),
            ResultStatus.Validation => Results.BadRequest(result.Error),
            ResultStatus.Conflict => Results.Conflict(result.Error),
            ResultStatus.Unauthorized => Results.Unauthorized(),
            ResultStatus.Forbidden => Results.Forbid(),

            ResultStatus.InternalError => Results.Problem(result.Error?.Message, statusCode: StatusCodes.Status500InternalServerError),

            _ => Results.Problem(result.Error?.Message, statusCode: StatusCodes.Status500InternalServerError)
        };
    }

    public static IResult ToHttpResult<T>(this Result<T> result)
    {
        return result.Status switch
        {
            ResultStatus.Ok => Results.Ok(result.Value),
            ResultStatus.Created => Results.Created(string.Empty, result.Value),
            ResultStatus.Accpeted => Results.Accepted(string.Empty, result.Value),
            ResultStatus.NoContent => Results.NoContent(),

            ResultStatus.BadRequest => Results.BadRequest(result.Error),
            ResultStatus.NotFound => Results.NotFound(result.Error),
            ResultStatus.Validation => Results.BadRequest(result.Error),
            ResultStatus.Conflict => Results.Conflict(result.Error),
            ResultStatus.Unauthorized => Results.Unauthorized(),
            ResultStatus.Forbidden => Results.Forbid(),

            ResultStatus.InternalError => Results.Problem(result.Error?.Message, statusCode: StatusCodes.Status500InternalServerError),

            _ => Results.Problem(result.Error?.Message, statusCode: StatusCodes.Status500InternalServerError)
        };
    }
}
