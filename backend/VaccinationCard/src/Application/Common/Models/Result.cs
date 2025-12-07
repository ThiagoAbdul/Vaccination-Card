using Application.Common.Enums;

namespace Application.Common.Models;


// Gosto do result pattern e já li no microsoft learn que lançar exceções degradam a performance da API

public sealed record Result
{
    public bool IsSuccess { get; init;  }
    public bool IsFailure => !IsSuccess;
    public ResultStatus Status { get; init; }
    public ResponseError? Error { get; set; }


    public static Result Success(ResultStatus status = ResultStatus.Ok) => new()
    {
        IsSuccess = true,
        Status = status,
    };

    public static Result Failure(ResponseError error, ResultStatus status = ResultStatus.Accepted) => new()
    {
        IsSuccess = false,
        Status = status,
        Error = error
    };

    public static Result Failure(string errorMessage, ResultStatus status = ResultStatus.BadRequest) => new()
    {
        IsSuccess = false,
        Status = status,
        Error = new ResponseError(errorMessage)
    };

    public static implicit operator Result(ResponseError error) => Failure(error);

    public static implicit operator Result(string error) => Failure(new ResponseError(error)); // Primeira vez que uso o Result Pattern com implicit,
                                                                                               // achei BEM MAIS limpo
                                                                                               // Mas fica sempre com o Status Code default

    public static Result<T> Failure<T>(string errorMessage, ResultStatus status = ResultStatus.BadRequest) => Result<T>.Failure(status, new ResponseError(errorMessage));

    public static Result<T> Success<T>(T value, ResultStatus code = ResultStatus.Ok) => Result<T>.Success(value, code);
    public Result<T> FailureAs<T>() => Failure<T>(Error!.Message, Status);

}

public sealed record Result<T>
{
    public bool IsSuccess { get; init; }
    public bool IsFailure => !IsSuccess;
    public ResultStatus Status { get; init; }
    public ResponseError? Error { get; init; }
    public T? Value { get; init;  }

    private Result(){}


    private Result(bool isSuccess, ResultStatus status, T? value, ResponseError? error)
    {
        IsSuccess = isSuccess;
        Status = status;
        Value = value;
        Error = error;
    }

    public Result<R> Map<R>(Func<T, R> map)
    {
        return new Result<R>()
        {
            IsSuccess = true,
            Error = Error,
            Status = Status,
            Value = Value is null? default : map(Value)
        };
    }

    public static Result<T> Success(T value, ResultStatus code = ResultStatus.Ok) =>
        new(true, code, value, null);

    public static Result<T> Failure(ResultStatus status, ResponseError error) =>
        new(false, status, default, error);


    public static implicit operator Result<T>(T value) =>
        Success(value, ResultStatus.Ok);
}




