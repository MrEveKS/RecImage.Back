namespace RecImage.Infrastructure.Commons;

public sealed class Result : IResult
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public int StatusCode { get; set; }

    public static IResult Failed(string message, int code = 500)
    {
        return Create(code, message: message);
    }

    private static IResult Create(int statusCode, bool success = false, string? message = null)
    {
        return new Result { Message = message, Success = success, StatusCode = statusCode };
    }
}

public sealed class Result<T> : IResult<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public string? Message { get; set; }
    public int StatusCode { get; set; }

    public static IResult<T> Ok(T? data)
    {
        return Create(200, true, data);
    }

    public static IResult<T> Failed(string message, int code = 500)
    {
        return Create(code, message: message);
    }

    private static IResult<T> Create(int statusCode, bool success = false, T? data = default, string? message = null)
    {
        return new Result<T> { Data = data, Message = message, Success = success, StatusCode = statusCode };
    }
}