namespace RecImage.Infrastructure.Commons;

public interface IResult
{
    bool Success { get; set; }
    string? Message { get; set; }
    int StatusCode { get; set; }
}

public interface IResult<TResult> : IResult
{
    public TResult? Data { get; set; }
}