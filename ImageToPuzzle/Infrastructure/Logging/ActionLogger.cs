using System;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Serilog;

namespace ImageToPuzzle.Infrastructure.Logging;

internal sealed class ActionLogger : IActionLogger
{
	private readonly ILogger _logger;

	public ActionLogger()
	{
		_logger = Log.Logger;
	}

	public void Information(string message, object value)
	{
		_logger.Information("{MESSAGE}: {VALUE}", message, value);
	}

	public void InformationObject<T>(T obj)
	{
		_logger.Information("{@TYPE} {NAME}: {VALUE}", typeof(T), nameof(obj), JsonConvert.SerializeObject(obj));
	}

	public void Error(Exception exception, string message)
	{
		_logger.Error(exception, "message: {MSG}", message);
	}

	public void ErrorObject<T>(Exception exception, T obj)
	{
		switch (obj)
		{
			case null:
				_logger.Error(exception, "{@TYPE} {NAME}: {VALUE}", typeof(T), nameof(obj), "value is null");

				break;
			case IFormFile formFile:
				ErrorFormFile(exception, formFile);

				break;
			default:
				_logger.Error(exception, "{@TYPE} {NAME}", typeof(T), nameof(obj));

				break;
		}
	}

	public void FatalObjectMessage<T>(string message, T obj)
	{
		_logger.Fatal("{MESSAGE}: {@TYPE} {NAME}: {VALUE}",
			message,
			typeof(T),
			nameof(obj),
			JsonConvert.SerializeObject(obj));
	}

	public void InformationObject<T>(string message, T obj)
	{
		_logger.Information("{MESSAGE}: {@TYPE} {NAME}: {VALUE}",
			message,
			typeof(T),
			nameof(obj),
			JsonConvert.SerializeObject(obj));
	}

	private void ErrorFormFile(Exception exception, IFormFile formFile)
	{
		_logger.Error(exception,
			"{@TYPE} {NAME}: ContentType: {CONTENT_TYPE}, FileName: {FILE_NAME}, Length: {LENGTH}",
			typeof(IFormFile),
			nameof(formFile),
			formFile.ContentType,
			formFile.FileName,
			formFile.Length);
	}
}