namespace RecImage.Infrastructure.Commons;

public class Result : IResult
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public int StatusCode { get; set; }

    public static IResult Ok()
    {
        return Create(200, true);
    }

    public static IResult Bad(string message)
    {
        return Create(400, message: message);
    }

    public static IResult Failed(string message, int code = 500)
    {
        return Create(code, message: message);
    }

    private static IResult Create(int statusCode, bool success = false, string? message = null)
    {
        return new Result { Message = message, Success = success, StatusCode = statusCode };
    }
}

public sealed class Result<TResult> : Result, IResult<TResult>
{
    public TResult? Data { get; set; }

    public static IResult<TResult> Ok(TResult? data)
    {
        return Create(200, true, data);
    }

    public new static IResult<TResult> Failed(string message, int code = 500)
    {
        return Create(code, message: message);
    }

    private static IResult<TResult> Create(int statusCode, bool success = false, TResult? data = default, string? message = null)
    {
        return new Result<TResult> { Data = data, Message = message, Success = success, StatusCode = statusCode };
    }
}