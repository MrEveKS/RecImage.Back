using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RecImage.Infrastructure.Logger.Extensions;
using RecImage.Infrastructure.Logger.Services;
using Serilog;

namespace RecImage.Infrastructure.Logger;

public static class Bootstrapper
{
    public static ILogger GetSerilogLogger(IConfiguration configuration)
    {
        return new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .SetTelegramLogger(configuration)
            .CreateLogger();
    }

    public static IServiceCollection AddActionLogger(this IServiceCollection services)
    {
        services.AddScoped<IActionLogger, ActionLogger>();
        return services;
    }
}