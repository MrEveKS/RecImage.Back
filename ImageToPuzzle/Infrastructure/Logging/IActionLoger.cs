using System;

namespace ImageToPuzzle.Infrastructure.Logging
{
	public interface IActionLoger
	{
		void ErrorObject<T>(Exception exception, T obj);
		void Information(string message, object value);
		void InformationObject<T>(T obj);
	}
}