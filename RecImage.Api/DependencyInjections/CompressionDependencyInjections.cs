using System.IO.Compression;
using Microsoft.AspNetCore.ResponseCompression;
using RecImage.Api.Providers;

namespace RecImage.Api.DependencyInjections;

internal static class CompressionDependencyInjections
{
    public static IServiceCollection AddRecImageCompression(this IServiceCollection services)
    {
        services
            .Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Optimal)
            .Configure<BrotliCompressionProviderOptions>(options => options.Level = CompressionLevel.Optimal)
            .AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
                options.Providers.Add<GzipCompressionProvider>();
                options.Providers.Add<BrotliCompressionProvider>();
                options.Providers.Add<DeflateCompressionProvider>();

                options.MimeTypes =
                    ResponseCompressionDefaults.MimeTypes.Concat(new[]
                    {
                        "application/javascript",
                        "application/json",
                        "application/xml",
                        "text/css",
                        "text/html",
                        "text/json",
                        "text/plain",
                        "text/xml",
                        "image/png",
                        "image/webp"
                    });
            });

        return services;
    }
}