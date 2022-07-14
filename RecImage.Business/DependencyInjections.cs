using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using RecImage.Business.Behaviours;
using RecImage.Business.Services;
using RecImage.ColoringService;
using RecImage.Infrastructure.Logger;

namespace RecImage.Business;

public static class DependencyInjections
{
    static DependencyInjections()
    {
        TypeAdapterConfig.GlobalSettings.Scan(
            typeof(DependencyInjections).Assembly);
    }

    public static IServiceCollection AddBusiness(this IServiceCollection services)
    {
        services
            .AddActionLogger()
            .AddImageServices()
            .AddTransient<IFileService, FileService>()
            .AddTransient<IDirectoryService, DirectoryService>()
            .AddMediatR(typeof(DependencyInjections));

        services
            .AddValidatorsFromAssembly(typeof(DependencyInjections).Assembly)
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        return services;
    }
}