using Serilog.Events;

namespace RecImage.Infrastructure.Logger.Settings;

internal sealed class TelegramLoggerSettings
{
    public string? BotId { get; set; }
    public string? ChatId { get; set; }
    public LogEventLevel? LogEventLevel { get; set; }
    public List<string>? ResponsibleDeveloperLogins { get; set; }
}