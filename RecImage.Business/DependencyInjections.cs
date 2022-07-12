using MediatR;
using Microsoft.Extensions.DependencyInjection;
using RecImage.Business.Services;
using RecImage.ColoringService;
using RecImage.Infrastructure.Logger;

namespace RecImage.Business;

public static class DependencyInjections
{
    public static IServiceCollection AddBusiness(this IServiceCollection services)
    {
        services
            .AddActionLogger()
            .AddImageServices()
            .AddTransient<IFileService, FileService>()
            .AddTransient<IDirectoryService, DirectoryService>()
            .AddMediatR(typeof(DependencyInjections));

        return services;
    }
}