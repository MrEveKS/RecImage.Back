using Serilog.Events;
using System.Collections.Generic;

namespace ImageToPuzzle.Infrastructure.Logging
{
	public class TelegramLoggerConfigModel
	{
		public string BotId { get; set; }

		public string ChatId { get; set; }

		public LogEventLevel? LogEventLevel { get; set; }

		public List<string> ResponsibleDeveloperLogins { get; set; }
	}
}
