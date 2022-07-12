using RecImage.Api.Constants;

namespace RecImage.Api.DependencyInjections;

internal static class CorsDependencyInjections
{
    public static IServiceCollection AddRecImageCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(CorsConstants.TestPolicy,
                builder => builder
                    .WithOrigins("http://localhost:4200", "https://localhost:4200", "http://192.168.0.103:8080")
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());

            options.AddPolicy(CorsConstants.ProductionPolicy,
                builder => builder
                    .WithOrigins("https://recimage.ru", "http://recimage.ru", "https://www.recimage.ru",
                        "http://www.recimage.ru")
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
        });

        return services;
    }
}