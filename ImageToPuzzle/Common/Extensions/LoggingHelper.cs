using ImageToPuzzle.Infrastructure.Logging;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Telegram;
using Serilog.Sinks.Telegram.Client;
using System;
using System.Linq;
using System.Text;

namespace ImageToPuzzle.Common.Extensions
{
	public static class LoggingHelper
	{
		public static LoggerConfiguration SetTelegramLogger(this LoggerConfiguration сonfiguration, IConfiguration configurationProvider)
		{
			var config = configurationProvider
				.GetSection("Logging:Telegram")
				.Get<TelegramLoggerConfigModel>();

			if (config?.BotId == null || config.ChatId == null)
			{
				return сonfiguration;
			}

			return сonfiguration
				.WriteTo.Async(a =>
					a.Telegram(config.BotId,
					config.ChatId,
					logEvent => RenderMessage(logEvent, config),
					config.LogEventLevel ?? LogEventLevel.Fatal)
				, bufferSize: 50);
		}

		private static TelegramMessage RenderMessage(LogEvent logEvent, TelegramLoggerConfigModel tgConfig)
		{
			var sb = new StringBuilder();
			sb.AppendLine($"{GetEmoji(logEvent)} {logEvent.RenderMessage()}");

			if (logEvent.Exception != null)
			{
				var envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Unidentified ENV";

				sb.AppendLine($"<strong>Message</strong>: <i>{logEvent.Exception.Message}</i>");
				sb.AppendLine($"<strong>ENV</strong>: <code>{envName}</code>\n");

				sb.AppendLine($"<strong>Type</strong>: <code>{logEvent.Exception.GetType().Name}</code>\n");
				sb.AppendLine($"<strong>Stack Trace</strong>\n<pre>{logEvent.Exception}</pre>");

				if (tgConfig.ResponsibleDeveloperLogins != null && tgConfig.ResponsibleDeveloperLogins.Any())
				{
					sb.AppendLine("\n" + string.Join(" ", tgConfig.ResponsibleDeveloperLogins.Select(x => $"@{x}")));
				}
			}

			return new TelegramMessage(sb.ToString(), TelegramParseModeTypes.Html);
		}

		private static string GetEmoji(LogEvent log)
		{
			switch (log.Level)
			{
				case LogEventLevel.Debug:

					return "👉";
				case LogEventLevel.Error:

					return "❗";
				case LogEventLevel.Fatal:

					return "‼";
				case LogEventLevel.Information:

					return "ℹ";
				case LogEventLevel.Verbose:

					return "⚡";
				case LogEventLevel.Warning:

					return "⚠";
				default:

					return "";
			}
		}
	}
}
