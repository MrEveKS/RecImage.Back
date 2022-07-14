using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace RecImage.ColoringService.Test.BaseTests;

public abstract class BaseImageTests
{
    protected readonly ITestOutputHelper Output;

    protected ServiceProvider ServiceProvider = null!;

    protected BaseImageTests(ITestOutputHelper output)
    {
        Output = output;
        Initialize();
    }

    private void Initialize()
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection
            .AddImageServices();

        ServiceProvider = serviceCollection.BuildServiceProvider();
    }
}