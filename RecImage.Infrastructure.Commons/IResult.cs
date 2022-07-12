namespace RecImage.Infrastructure.Commons;

public interface IResult
{
    bool Success { get; set; }
    string? Message { get; set; }
    int StatusCode { get; set; }
}

public interface IResult<T> : IResult
{
    public T? Data { get; set; }
}