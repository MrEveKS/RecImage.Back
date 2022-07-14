using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using RecImage.Business.Behaviours;
using RecImage.Business.Services;
using RecImage.ColoringService;
using RecImage.Infrastructure.Logger;
using RecImage.Test.Commons.Helpers;
using Serilog;
using Xunit.Abstractions;

namespace RecImage.Business.Tests.BaseTests;

public abstract class BaseBusinessTests
{
    protected readonly ITestOutputHelper Output;
    protected readonly TimeSpan StopAfter = TimeSpan.FromSeconds(45);

    protected ServiceProvider ServiceProvider = null!;

    protected BaseBusinessTests(ITestOutputHelper output)
    {
        Output = output;
        Initialize();
    }

    private void Initialize()
    {
        var stream = ImageGenerate.GenerateGradientImage();
        var testFileInfos = new DirectoryInfo(Environment.CurrentDirectory)
            .GetFiles();

        var mockDirectoryService = new Mock<IDirectoryService>();
        var mockFileService = new Mock<IFileService>();
        mockDirectoryService.Setup(x => x.GetFiles(It.IsAny<string>()))
            .Returns(testFileInfos);
        mockFileService.Setup(x => x.OpenRead(It.IsAny<string>()))
            .Returns(stream);

        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        Log.Logger = Bootstrapper.GetSerilogLogger(configuration);

        var services = new ServiceCollection();

        services
            .AddActionLogger()
            .AddImageServices()
            .AddTransient(_ => mockFileService.Object)
            .AddTransient(_ => mockDirectoryService.Object)
            .AddMediatR(typeof(DependencyInjections));

        services
            .AddValidatorsFromAssembly(typeof(DependencyInjections).Assembly)
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        ;

        ServiceProvider = services.BuildServiceProvider();
    }
}