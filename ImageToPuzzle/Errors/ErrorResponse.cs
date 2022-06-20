namespace ImageToPuzzle.Errors;

public class ErrorResponse
{
	public ErrorResponse(string message, int statusCode)
	{
		Message = message;
		StatusCode = statusCode;
	}

	public int StatusCode { get; }

	public string State => "error";

	public string Message { get; }

	public string Exception { get; set; }
}