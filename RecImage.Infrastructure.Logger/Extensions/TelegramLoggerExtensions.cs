using System.Text;
using Microsoft.Extensions.Configuration;
using RecImage.Infrastructure.Logger.Settings;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Telegram;
using Serilog.Sinks.Telegram.Client;

namespace RecImage.Infrastructure.Logger.Extensions;

internal static class TelegramLoggerExtensions
{
    public static LoggerConfiguration SetTelegramLogger(this LoggerConfiguration configuration,
        IConfiguration configurationProvider)
    {
        var config = configurationProvider
            .GetSection("Logging:Telegram")
            .Get<TelegramLoggerSettings>();

        if (config?.BotId == null || config.ChatId == null)
        {
            return configuration;
        }

        return configuration
            .WriteTo.Async(a =>
                    a.Telegram(config.BotId,
                        config.ChatId,
                        logEvent => RenderMessage(logEvent, config),
                        config.LogEventLevel ?? LogEventLevel.Fatal),
                50);
    }

    private static TelegramMessage RenderMessage(LogEvent logEvent,
        TelegramLoggerSettings tgConfig)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"{GetEmoji(logEvent)} {logEvent.RenderMessage()}");

        if (logEvent.Exception == null) return new TelegramMessage(sb.ToString(), TelegramParseModeTypes.Html);

        var envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Unidentified ENV";

        sb.AppendLine($"<strong>Message</strong>: <i>{logEvent.Exception.Message}</i>");
        sb.AppendLine($"<strong>ENV</strong>: <code>{envName}</code>\n");

        sb.AppendLine($"<strong>Type</strong>: <code>{logEvent.Exception.GetType().Name}</code>\n");
        sb.AppendLine($"<strong>Stack Trace</strong>\n<pre>{logEvent.Exception}</pre>");

        if (tgConfig.ResponsibleDeveloperLogins != null && tgConfig.ResponsibleDeveloperLogins.Any())
        {
            sb.AppendLine("\n" + string.Join(" ", tgConfig.ResponsibleDeveloperLogins.Select(x => $"@{x}")));
        }

        return new TelegramMessage(sb.ToString(), TelegramParseModeTypes.Html);
    }

    private static string GetEmoji(LogEvent log)
    {
        return log.Level switch
        {
            LogEventLevel.Debug => "👉",
            LogEventLevel.Error => "❗",
            LogEventLevel.Fatal => "‼",
            LogEventLevel.Information => "ℹ",
            LogEventLevel.Verbose => "⚡",
            LogEventLevel.Warning => "⚠",
            _ => ""
        };
    }
}