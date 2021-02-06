using System;

namespace ImageToPuzzle.Infrastructure.Logging
{
	public interface IActionLogger
	{
		void Error(Exception exception, string message);
		void ErrorObject<T>(Exception exception, T obj);
		void FatalObjectMessage<T>(string message, T obj);
		void Information(string message, object value);
		void InformationObject<T>(T obj);
	}
}