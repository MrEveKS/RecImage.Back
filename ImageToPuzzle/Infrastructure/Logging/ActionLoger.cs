using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Serilog;
using System;

namespace ImageToPuzzle.Infrastructure.Logging
{
	public class ActionLoger : IActionLoger
	{
		private readonly ILogger _loger;

		public ActionLoger()
		{
			_loger = Log.Logger;
		}

		public void Information(string message, object value)
		{
			_loger.Information("{MESSAGE}: {VALUE}", message, value);
		}

		public void InformationObject<T>(T obj)
		{
			_loger.Information("{@TYPE} {NAME}: {VALUE}", typeof(T), nameof(obj), JsonConvert.SerializeObject(obj));
		}

		public void InformationObject<T>(string message, T obj)
		{
			_loger.Information("{MESSAGE}: {@TYPE} {NAME}: {VALUE}",
				message, typeof(T), nameof(obj), JsonConvert.SerializeObject(obj));
		}

		public void ErrorObject<T>(Exception exception, T obj)
		{
			switch (obj)
			{
				case null:
					_loger.Error(exception, "{@TYPE} {NAME}: {VALUE}", typeof(T), nameof(obj), "value is null");
					break;
				case IFormFile formFile:
					ErrorFormFile(exception, formFile);
					break;
				default:
					_loger.Error(exception, "{@TYPE} {NAME}", typeof(T), nameof(obj));
					break;
			}
		}

		private void ErrorFormFile(Exception exception, IFormFile formFile)
		{
			_loger.Error(exception, "{@TYPE} {NAME}: ContentType: {CONTENT_TYPE}, FileName: {FILE_NAME}, Length: {LENGTH}",
				typeof(IFormFile), nameof(formFile), formFile.ContentType, formFile.FileName, formFile.Length);
		}
	}
}
