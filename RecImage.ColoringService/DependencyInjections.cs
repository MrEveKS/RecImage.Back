using Microsoft.Extensions.DependencyInjection;
using RecImage.ColoringService.Services;

namespace RecImage.ColoringService;

public static class DependencyInjections
{
    public static IServiceCollection AddImageServices(this IServiceCollection services)
    {
        services.AddScoped<IImageConverter, ImageConverter>();

        return services;
    }
}